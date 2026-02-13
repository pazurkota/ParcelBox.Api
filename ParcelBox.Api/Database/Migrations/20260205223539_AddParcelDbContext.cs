using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ParcelBox.Api.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddParcelDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parcels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PickupCode = table.Column<string>(type: "text", nullable: false),
                    ParcelSize = table.Column<int>(type: "integer", nullable: false),
                    InitialLockerId = table.Column<int>(type: "integer", nullable: false),
                    TargetLockerId = table.Column<int>(type: "integer", nullable: false),
                    InitialLockerBoxId = table.Column<int>(type: "integer", nullable: false),
                    TargetLockerBoxId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcels", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parcels");
        }
    }
}
