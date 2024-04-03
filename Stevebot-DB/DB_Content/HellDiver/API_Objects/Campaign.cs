using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Stevebot_DB.DB_Content.HellDiver.API_Objects {
    public class Campaign {
        public string description { get; set; } = "";
        public int Count { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [ForeignKey("FK_PlanetId")]
        public required Planet planet { get; set; }
        public int type { get; set; }
    }
}
