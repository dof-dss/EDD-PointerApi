using Microsoft.EntityFrameworkCore.Migrations;

namespace PointerApi.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pointer",
                columns: table => new
                {
                    Organisation_Name = table.Column<string>(nullable: true),
                    Sub_Building_Name = table.Column<string>(nullable: true),
                    Building_Name = table.Column<string>(nullable: true),
                    Building_Number = table.Column<string>(nullable: true),
                    Primary_Thorfare = table.Column<string>(nullable: true),
                    Alt_Thorfare_Name1 = table.Column<string>(nullable: true),
                    Secondary_Thorfare = table.Column<string>(nullable: true),
                    Locality = table.Column<string>(nullable: true),
                    Townland = table.Column<string>(nullable: true),
                    Town = table.Column<string>(nullable: true),
                    County = table.Column<string>(nullable: true),
                    Postcode = table.Column<string>(nullable: true),
                    BLPU = table.Column<string>(nullable: true),
                    Unique_Building_ID = table.Column<int>(nullable: false),
                    UPRN = table.Column<int>(nullable: false),
                    USRN = table.Column<int>(nullable: false),
                    Local_Council = table.Column<string>(nullable: true),
                    X_COR = table.Column<int>(nullable: false),
                    Y_COR = table.Column<int>(nullable: false),
                    Temp_Coords = table.Column<string>(nullable: true),
                    Building_Status = table.Column<string>(nullable: true),
                    Address_Status = table.Column<string>(nullable: true),
                    Classification = table.Column<string>(nullable: true),
                    Creation_Date = table.Column<string>(nullable: true),
                    Commencement_Date = table.Column<string>(nullable: true),
                    Archived_Date = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    UDPRN = table.Column<string>(nullable: true),
                    Posttown = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pointer");
        }
    }
}
