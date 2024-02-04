using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sucre_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editTableTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChannaleId",
                table: "DeviceTags",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChannaleId",
                table: "DeviceTags");
        }
    }
}
