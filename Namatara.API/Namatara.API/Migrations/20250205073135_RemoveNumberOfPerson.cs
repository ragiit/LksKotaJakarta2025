using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Namatara.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNumberOfPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfPersons",
                table: "TicketBookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfPersons",
                table: "TicketBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
