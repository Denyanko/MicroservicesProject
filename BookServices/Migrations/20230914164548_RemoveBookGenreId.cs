using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookServices.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBookGenreId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "BookGenre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BookGenre",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
