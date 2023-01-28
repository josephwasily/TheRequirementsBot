using Microsoft.Bot.Builder.Adapters;
using MultiTurnPromptBot.NLP.Models;
using Newtonsoft.Json;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiTurnPromptBot.NLP
{
    public static class OpenAIHelpers 
    {
        
        public async static Task<AmbiguityCheckResponse> CheckRequirementAmbiguity(string userRequirement)
        {
            var openAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = "<key>"
            });

            var prompt = $"\"{userRequirement}\"is this user requirement ambigious? respond in JSON format {{\"ambigious\": true,\"ambiguousWords\":[] , \"rewrite\":\"\" }}, provide if ambigious or not, and the list of ambigious words, and rewrite it in less ambigious way";
            var completionResult = await openAiService.Completions.CreateCompletion(new CompletionCreateRequest()
            {
                Prompt = prompt,
                Model = "text-davinci-003",
                Temperature =  0,
                MaxTokens = 256,
            });
            if(completionResult.Successful && completionResult.Choices.Any())
            {
                return JsonConvert.DeserializeObject<AmbiguityCheckResponse>(completionResult.Choices[0].Text.Replace("\n","").Replace("\r",""));

            }
            return new AmbiguityCheckResponse
            {
                Ambiguous = true
            };
        }
    }
}
