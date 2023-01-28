using MultiTurnPromptBot.NLP.Models;
using System.Threading.Tasks;

namespace MultiTurnPromptBot.NLP
{
    public interface INLPService
    {
        Task<AmbiguityCheckResponse> CheckRequirementAmbiguity(string userRequirement);
    }
}