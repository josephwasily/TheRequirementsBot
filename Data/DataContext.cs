using FileContextCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using static OpenAI.GPT3.ObjectModels.SharedModels.IOpenAiModels;

namespace MultiTurnPromptBot.Data
{
    public class DataContext : DbContext
    {
        public string ConnectionString { get; set; }
        //public DataContext(IConfiguration configuration)
        //{
        //    ConnectionString = configuration.GetConnectionString("WebApiDatabase");
        //}

        public DataContext()
        {
        }
        public DataContext(DbContextOptions<DataContext> option) : base(option)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlServer("Server=.\\SQLExpress;Database=RequirementsBotDb;Trusted_Connection=Yes;TrustServerCertificate=True;");

        }

        public DbSet<UserRequirement> Requirements { get; set; }

    }
}
