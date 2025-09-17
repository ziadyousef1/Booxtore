using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booxtore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBorrowedByCurrentUserProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBorrowedByCurrentUser",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBorrowedByCurrentUser",
                table: "Books");
        }
    }
}
