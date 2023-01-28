using System.ComponentModel.DataAnnotations;

namespace MultiTurnPromptBot.Data
{
    public class UserRequirement
    {
        [Key]
        public long Id { get; set; }
        public string AuthorName { get; set; }
        public string User { get; set;  }
        public string NoOfUsers { get; set; }
        public string Feature { get; set; }
        public string RelatedModule { get; set;}
        public string Risk { get; set; }
        public int TechnicalComplexity { get; set; }
        public int BusinessImportance { get; set; }

    }
}
