using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Review.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessIdOnReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessId",
                table: "Review",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewPhoto_ReviewId",
                table: "ReviewPhoto",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewPhoto_Review_ReviewId",
                table: "ReviewPhoto",
                column: "ReviewId",
                principalTable: "Review",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewPhoto_Review_ReviewId",
                table: "ReviewPhoto");

            migrationBuilder.DropIndex(
                name: "IX_ReviewPhoto_ReviewId",
                table: "ReviewPhoto");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Review");
        }
    }
}
