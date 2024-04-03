using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Stevebot_DB.DB_Content.HellDiver.API_Objects
{
    public class PlanetStatus
    { //PlanetStatusSchema
        [Key]
        public int id { get; set; }
        //public DateTime TimeStamp { get; set; } = DateTime.Now;
        public float health { get; set; }
        public float liberation { get; set; }
        public required string owner { get; set; }
        [ForeignKey("FK_PlanetId")]
        public required Planet planet { get; set; }
        [Required]
        public int FK_PlanetId { get; set; }
        public float players { get; set; }
        public float regen_per_second { get; set; }
    }
}
