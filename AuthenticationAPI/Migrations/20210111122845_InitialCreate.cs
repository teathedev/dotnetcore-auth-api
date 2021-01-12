using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthenticationAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "shared");

            migrationBuilder.CreateSequence<int>(
                name: "sq_user",
                schema: "shared");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR shared.sq_user"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.UniqueConstraint("AK_User_Email", x => x.Email);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropSequence(
                name: "sq_user",
                schema: "shared");
        }
    }
}
