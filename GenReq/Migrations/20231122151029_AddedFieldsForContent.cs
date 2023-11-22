using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenReq.Migrations
{
    /// <inheritdoc />
    public partial class AddedFieldsForContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "GenRequest",
                newName: "GeneratedTitle");

            migrationBuilder.AddColumn<string>(
                name: "GeneratedContent",
                table: "GenRequest",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneratedContent",
                table: "GenRequest");

            migrationBuilder.RenameColumn(
                name: "GeneratedTitle",
                table: "GenRequest",
                newName: "Content");
        }
    }
}
