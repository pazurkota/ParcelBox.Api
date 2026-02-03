using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelBox.Api.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddLockerBoxes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LockerBox_Lockers_LockerId",
                table: "LockerBox");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LockerBox",
                table: "LockerBox");

            migrationBuilder.RenameTable(
                name: "LockerBox",
                newName: "LockerBoxes");

            migrationBuilder.RenameIndex(
                name: "IX_LockerBox_LockerId",
                table: "LockerBoxes",
                newName: "IX_LockerBoxes_LockerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LockerBoxes",
                table: "LockerBoxes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LockerBoxes_Lockers_LockerId",
                table: "LockerBoxes",
                column: "LockerId",
                principalTable: "Lockers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LockerBoxes_Lockers_LockerId",
                table: "LockerBoxes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LockerBoxes",
                table: "LockerBoxes");

            migrationBuilder.RenameTable(
                name: "LockerBoxes",
                newName: "LockerBox");

            migrationBuilder.RenameIndex(
                name: "IX_LockerBoxes_LockerId",
                table: "LockerBox",
                newName: "IX_LockerBox_LockerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LockerBox",
                table: "LockerBox",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LockerBox_Lockers_LockerId",
                table: "LockerBox",
                column: "LockerId",
                principalTable: "Lockers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
