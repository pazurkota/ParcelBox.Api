using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelBox.Api.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddParcelStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParcelStatus",
                table: "Parcels",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParcelStatus",
                table: "Parcels");
        }
    }
}
