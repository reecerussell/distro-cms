using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Migrations.Migrations
{
    public partial class StoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var fileNames = Assembly.GetExecutingAssembly()
                .GetManifestResourceNames()
                .Where(x => x.Contains("StoredProcedures") && x.EndsWith(".sql"));

            foreach (var filename in fileNames)
            {
                using var stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(filename);
                using var reader = new StreamReader(stream);
                var sql = reader.ReadToEnd();

                migrationBuilder.Sql(sql);
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
