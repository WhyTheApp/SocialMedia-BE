using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddedNameAndUsernameSeparationInUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
            
            migrationBuilder.Sql(@"
                UPDATE ""Users""
                SET ""Name"" = ""Username"" 
                ");
            migrationBuilder.Sql(@"
                UPDATE ""Users""
                SET ""Username"" = LOWER(""Username"");
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");
        }
    }
}
