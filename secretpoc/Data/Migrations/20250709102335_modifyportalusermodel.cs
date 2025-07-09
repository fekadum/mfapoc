using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace secretpoc.Data.Migrations
{
    /// <inheritdoc />
    public partial class modifyportalusermodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthenticatorKey",
                table: "PortalUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthenticatorKey",
                table: "PortalUser");
        }
    }
}
