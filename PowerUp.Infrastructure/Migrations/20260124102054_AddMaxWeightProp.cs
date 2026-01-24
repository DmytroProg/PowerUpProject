using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerUp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMaxWeightProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaxWeight",
                table: "ExerciseHistory",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxWeight",
                table: "ExerciseHistory");
        }
    }
}
