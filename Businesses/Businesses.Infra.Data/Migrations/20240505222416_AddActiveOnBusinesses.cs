using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Businesses.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveOnBusinesses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Business",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Business");
        }
    }
}
