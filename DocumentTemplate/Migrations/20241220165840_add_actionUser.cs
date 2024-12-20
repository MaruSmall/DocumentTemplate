using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentTemplate.Migrations
{
    /// <inheritdoc />
    public partial class add_actionUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Action",
                table: "Loggings",
                newName: "ActionUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActionUser",
                table: "Loggings",
                newName: "Action");
        }
    }
}
