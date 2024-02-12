using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmRating.Models.EntityFramework
{
    [Table("t_j_notation_not")]
    public class Notation
    {
        [Key]
        [Column("utl_id")]
        public int UtilisateurId { get; set; }

        [Key]
        [Column("flm_id")]
        public int FilmId { get; set; }

        [Column("not_note")]
        [Range(0, 5)]
        public int Note { get; set; }

        [ForeignKey(nameof(FilmId))]
        [InverseProperty("NotesFilm")]
        public virtual Film FilmNote { get; set; }

        [ForeignKey(nameof(UtilisateurId))]
        [InverseProperty("NotesUtilisateur")]
        public virtual Utilisateur UtilisateurNotant { get; set; }
    }
}
