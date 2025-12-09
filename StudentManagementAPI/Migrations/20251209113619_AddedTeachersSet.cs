using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedTeachersSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 56L);

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Passwordhash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Email);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "Age", "Grade", "StudentName" },
                values: new object[,]
                {
                    { 27L, 23, "A", "Eswar Reddy" },
                    { 56L, 23, "B", "Deeksha banoth" }
                });
        }
    }
}
