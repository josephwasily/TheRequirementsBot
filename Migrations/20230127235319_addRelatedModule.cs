using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiTurnPromptBot.Migrations
{
    /// <inheritdoc />
    public partial class addRelatedModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RelatedModule",
                table: "Requirements",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelatedModule",
                table: "Requirements");
        }
    }
}
