using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiTurnPromptBot.Migrations
{
    /// <inheritdoc />
    public partial class addRequiremnets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requirements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoOfUsers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Feature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Risk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TechnicalComplexity = table.Column<int>(type: "int", nullable: false),
                    BusinessImportance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requirements", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requirements");
        }
    }
}
