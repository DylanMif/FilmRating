using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmRating.Migrations
{
    public partial class DeleteRestrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_notation_film",
                table: "t_j_notation_not");

            migrationBuilder.DropForeignKey(
                name: "fk_notation_utilisateur",
                table: "t_j_notation_not");

            migrationBuilder.AddForeignKey(
                name: "fk_notation_film",
                table: "t_j_notation_not",
                column: "flm_id",
                principalTable: "t_e_film_flm",
                principalColumn: "flm_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_notation_utilisateur",
                table: "t_j_notation_not",
                column: "utl_id",
                principalTable: "t_e_utilisateur_utl",
                principalColumn: "utl_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_notation_film",
                table: "t_j_notation_not");

            migrationBuilder.DropForeignKey(
                name: "fk_notation_utilisateur",
                table: "t_j_notation_not");

            migrationBuilder.AddForeignKey(
                name: "fk_notation_film",
                table: "t_j_notation_not",
                column: "flm_id",
                principalTable: "t_e_film_flm",
                principalColumn: "flm_id");

            migrationBuilder.AddForeignKey(
                name: "fk_notation_utilisateur",
                table: "t_j_notation_not",
                column: "utl_id",
                principalTable: "t_e_utilisateur_utl",
                principalColumn: "utl_id");
        }
    }
}
