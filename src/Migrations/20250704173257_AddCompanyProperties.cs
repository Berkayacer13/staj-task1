using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyService.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeCount",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ARACI_KURUM",
                columns: table => new
                {
                    ARACI_KURUM_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ARACI_KURUM_ADI = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ARACI_KURUM", x => x.ARACI_KURUM_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ARACI_KURUM");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "EmployeeCount",
                table: "Companies");
        }
    }
}
