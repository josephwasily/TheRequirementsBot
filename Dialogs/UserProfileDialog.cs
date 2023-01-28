// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using MultiTurnPromptBot.Data;
using MultiTurnPromptBot.NLP;

namespace Microsoft.BotBuilderSamples
{
    public class UserProfileDialog : ComponentDialog
    {
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;

        public UserProfileDialog(UserState userState)
            : base(nameof(UserProfileDialog))
        {
            _userProfileAccessor = userState.CreateProperty<UserProfile>("UserProfile");

            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                NameStep,
                ConfirmStep,
                
                RequirementInvolvedUsersConfirmStepAsync,
                RequirementsNumberOfUsers,
                StartRequirementElicitationProcess,
                RequirementsRelatedRisks,
                RequirementsBusinessImportance,
                RequirementsTechnicalComplexity,
                RequirementsSummary
            };
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new ElicitRequirementTextDialog());
            // Add named dialogs to the DialogSet. These names are saved in the dialog state.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
           

            //AddDialog(new AttachmentPrompt(nameof(AttachmentPrompt), PicturePromptValidatorAsync));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

       
     
        private static async Task<DialogTurnResult> NameStep(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken
        )
        {
            return await stepContext.PromptAsync(
                nameof(TextPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Hello, What is your name please?"),
                },
                cancellationToken
            );
        }

        private static async Task<DialogTurnResult> ConfirmStep(WaterfallStepContext stepContext,
           CancellationToken cancellationToken)
        {
            stepContext.Values["name"] = stepContext.Result.ToString();
            return await stepContext.PromptAsync(
               nameof(ChoicePrompt),
               new PromptOptions
               {
                   Choices = new List<Choice>(2) { new Choice("Yes"), new Choice("No")},
                   Prompt = MessageFactory.Text(
                       "Would you like to give to start gathering requirements process?"
                   )
               },
               cancellationToken
           );
        }
        private async Task<DialogTurnResult> RequirementRelatedSystemModuleConfirmStepAsync(
               WaterfallStepContext stepContext,
               CancellationToken cancellationToken
           )
        {
            if (((FoundChoice)stepContext.Result).Value.ToString() == "Yes")
            {
                // User said "yes" so we will be prompting for the age.
                // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.

                return await stepContext.PromptAsync(
                    nameof(ChoicePrompt),
                    new PromptOptions
                    {
                        Prompt = MessageFactory.Text(
                            "Please choose the related system Module for this feature"
                        ),
                        Choices = ChoiceFactory.ToChoices(
                            new List<string> { "Module-A", "Module-B"}
                        ),
                    },
                    cancellationToken
                );
            }
            else
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text(
                        $"Thanks {stepContext.Result}. You choose no for the previous confirmation, currently this bot only supports requirement process, we will add more features in the future"
                    ),
                    cancellationToken
                );
                // User said "no" so we will skip the next step. Give -1 as the age.
                return await stepContext.EndDialogAsync();
            }
        }
        private async Task<DialogTurnResult> RequirementInvolvedUsersConfirmStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken
        )
        {
            stepContext.Values["req_relatedModule"] = stepContext.Result?.ToString();
                return await stepContext.PromptAsync(
                    nameof(ChoicePrompt),
                    new PromptOptions
                    {
                        Prompt = MessageFactory.Text(
                            "Please choose the involved user from the list of preconfigured users"
                        ),
                        Choices = ChoiceFactory.ToChoices(
                            new List<string> { "Admin", "User", "Secretary" }
                        ),
                    },
                    cancellationToken
                );
           
        }
        private async Task<DialogTurnResult> StartRequirementElicitationProcess(
        WaterfallStepContext stepContext,
        CancellationToken cancellationToken
    )
        {
            stepContext.Values["req_user"] = (stepContext.Result as FoundChoice).Value;
            return await stepContext.BeginDialogAsync(nameof(ElicitRequirementTextDialog), null, cancellationToken);
        }



        private async Task<DialogTurnResult> RequirementsNumberOfUsers(
   WaterfallStepContext stepContext,
   CancellationToken cancellationToken
)
        {
            stepContext.Values["req_involvedUser"] = (stepContext.Result as string);

            return await stepContext.PromptAsync(
               nameof(ChoicePrompt),
               new PromptOptions
               {
                   Prompt = MessageFactory.Text($"How many users do you think will use such feature?"),
                   Choices = ChoiceFactory.ToChoices(
                            new List<string> { "1-20", "20-100", "100-1000", "1000+" }
                        ),
               },
               cancellationToken
           );   

            // This condition is our validation rule. You can also change the value at this point.
        }

        private async Task<DialogTurnResult> RequirementsRelatedRisks(
   WaterfallStepContext stepContext,
   CancellationToken cancellationToken)
        {
            stepContext.Values["req_feature"] = (stepContext.Result as string);

            await stepContext.Context.SendActivityAsync($"Great, So to summarize, {stepContext.Values["req_user"]} is the main actor in this feature, and the user feature is {stepContext.Result.ToString()}", cancellationToken: cancellationToken);

            return await stepContext.PromptAsync(
               nameof(TextPrompt),
               new PromptOptions
               {
                   Prompt = MessageFactory.Text($"Is there any business or technological risks related to that feature?"),
                
               },
               cancellationToken
           );

            // This condition is our validation rule. You can also change the value at this point.
        }
        private async Task<DialogTurnResult> RequirementsBusinessImportance(
 WaterfallStepContext stepContext,
 CancellationToken cancellationToken)
        {
            stepContext.Values["req_risks"] = stepContext.Result.ToString();

            return await stepContext.PromptAsync(
               nameof(NumberPrompt<int>),
               new PromptOptions
               {
                   Prompt = MessageFactory.Text($"On Scale from 1-5 How you evaluate the business impact of this feature (1 being of highest importance and 5 is of lowest importance)"),

               },
               cancellationToken
           );

            // This condition is our validation rule. You can also change the value at this point.
        }
        private async Task<DialogTurnResult> RequirementsTechnicalComplexity(
 WaterfallStepContext stepContext,
 CancellationToken cancellationToken)
        {
            stepContext.Values["req_importance"] = stepContext.Result.ToString();

            return await stepContext.PromptAsync(
               nameof(NumberPrompt<int>),
               new PromptOptions
               {
                   Prompt = MessageFactory.Text($"On Scale from 1-5 How you evaluate the technological complxity of this feature (1 being of lowest complexity and 5 is of highest complexity)"),

               },
               cancellationToken
           );

            // This condition is our validation rule. You can also change the value at this point.
        }
        private async Task<DialogTurnResult> RequirementsSummary(
WaterfallStepContext stepContext,
CancellationToken cancellationToken)
        {
            stepContext.Values["req_complexity"] = stepContext.Result.ToString();

            //save to db
            using(var dbContext = new DataContext())
            {
                try
                {
                    await dbContext.Requirements.AddAsync(new UserRequirement
                    {
                        NoOfUsers = stepContext.Values["req_user"]?.ToString(),
                        Feature = stepContext.Values["req_feature"]?.ToString(),
                        AuthorName = stepContext.Values["name"]?.ToString(),
                        BusinessImportance = int.Parse(stepContext.Values["req_importance"]?.ToString()),
                        TechnicalComplexity = int.Parse(stepContext.Values["req_complexity"]?.ToString()),
                        Risk = stepContext.Values["req_risks"]?.ToString(),
                        User = stepContext.Values["req_InvolvedUser"]?.ToString(),
                        RelatedModule = stepContext.Values["req_relatedModule"]?.ToString()
                    }) ;
                  
                    await stepContext.Context.SendActivityAsync($"Thanks for Submitting your requirements, your requirement is stored now in product backlog", cancellationToken: cancellationToken);

                    await dbContext.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return await stepContext.EndDialogAsync();

            // This condition is our validation rule. You can also change the value at this point.
        }


    }
}
