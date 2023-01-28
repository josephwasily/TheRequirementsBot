// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using MultiTurnPromptBot.NLP;

namespace Microsoft.BotBuilderSamples
{
    public class ElicitRequirementTextDialog : ComponentDialog
    {
        public ElicitRequirementTextDialog()
            : base(nameof(ElicitRequirementTextDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
                {
                    SelectionStepAsync,
                    LoopStepAsync,
                }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> SelectionStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            var isRepeated = stepContext.Options != null ? (bool)stepContext.Options : false;
            PromptOptions promptOptions;
            if (isRepeated)
            {
                promptOptions = new PromptOptions
                {
                    Prompt = MessageFactory.Text($"Please rewrite the requirement of this functionality in less ambigious way"),
                };
            }
            else
            {
                promptOptions = new PromptOptions
                {
                    Prompt = MessageFactory.Text($"So what the user would like system to do? "),
                };  
            }
            
            return await stepContext.PromptAsync(
                nameof(TextPrompt),
                promptOptions,
                cancellationToken
            );
        }

        private async Task<DialogTurnResult> LoopStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            var ambigiousCheck = await OpenAIHelpers.CheckRequirementAmbiguity(stepContext.Result.ToString());
            if (ambigiousCheck.Ambiguous)
            {
                await stepContext.Context.SendActivityAsync(
                "It seems that the requirement you've entered is ambiguous \n"
                +
                "Words like " + string.Join(" or ", ambigiousCheck.AmbiguousWords.Select(z => $"\"{z}\"")) + " can cause confusion during implementation \n"
                +
                " the statement can be written as the following: \"" + ambigiousCheck.Rewrite + "\"", cancellationToken: cancellationToken);

                // Otherwise, repeat this dialog, passing in the list from this iteration.
                return await stepContext.ReplaceDialogAsync(nameof(ElicitRequirementTextDialog), true,cancellationToken);
            }
            else
            {
                //persist 
                return await stepContext.EndDialogAsync(stepContext.Result.ToString(), cancellationToken);
            }
        }
    }
}
