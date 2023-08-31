using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicLearn.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_courses2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseGroups_SubGroup",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "SubGroup",
                table: "Courses",
                newName: "SubGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_SubGroup",
                table: "Courses",
                newName: "IX_Courses_SubGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseGroups_SubGroupId",
                table: "Courses",
                column: "SubGroupId",
                principalTable: "CourseGroups",
                principalColumn: "CourseGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseGroups_SubGroupId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "SubGroupId",
                table: "Courses",
                newName: "SubGroup");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_SubGroupId",
                table: "Courses",
                newName: "IX_Courses_SubGroup");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseGroups_SubGroup",
                table: "Courses",
                column: "SubGroup",
                principalTable: "CourseGroups",
                principalColumn: "CourseGroupId");
        }
    }
}
