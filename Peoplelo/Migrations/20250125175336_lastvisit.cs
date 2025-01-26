using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Peoplelo.Migrations
{
    /// <inheritdoc />
    public partial class lastvisit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastLoginTime",
                table: "AspNetUsers",
                newName: "LastVisit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastVisit",
                table: "AspNetUsers",
                newName: "LastLoginTime");
        }
    }
}
