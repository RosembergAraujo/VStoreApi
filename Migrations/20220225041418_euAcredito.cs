using Microsoft.EntityFrameworkCore.Migrations;

namespace VStoreAPI.Migrations
{
    public partial class euAcredito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserForeignKey",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserForeignKey",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserForeignKey",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Orders",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Orders",
                newName: "UserId1");

            migrationBuilder.AddColumn<int>(
                name: "UserForeignKey",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserForeignKey",
                table: "Orders",
                column: "UserForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserForeignKey",
                table: "Orders",
                column: "UserForeignKey",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
