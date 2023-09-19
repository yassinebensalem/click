using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Infra.Data.Migrations
{
    public partial class addSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlRolesFile = Path.Combine(@"Scripts/insert-roles.Sql");
            migrationBuilder.Sql(File.ReadAllText(sqlRolesFile));

            var sqlLanguagesFile = Path.Combine(@"Scripts/insert-languages.Sql");
            migrationBuilder.Sql(File.ReadAllText(sqlLanguagesFile));

            var sqlCountriesFile = Path.Combine("Scripts/insert-countries.Sql");
            migrationBuilder.Sql(File.ReadAllText(sqlCountriesFile));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
