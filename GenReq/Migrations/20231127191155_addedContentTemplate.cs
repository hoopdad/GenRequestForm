using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenReq.Migrations
{
    /// <inheritdoc />
    public partial class addedContentTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentTemplate",
                table: "GenRequest",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentTemplate",
                table: "GenRequest");
        }
    }
}
