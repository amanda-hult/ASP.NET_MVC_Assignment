using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectUserDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_Projects_ProjectId",
                table: "ProjectUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_Projects_ProjectId",
                table: "ProjectUsers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_Projects_ProjectId",
                table: "ProjectUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_Projects_ProjectId",
                table: "ProjectUsers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
