using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Stevebot_DB.Framework_ENT;

namespace Stevebot_DB.DB_Content.HellDiver.API_Objects
{
    public class Planet
    { //PlanetSchema
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int index { get; set; }
        public bool disabled { get; set; } = false;
        public float hash { get; set; }
        public required string initial_owner { get; set; }
        public int max_health { get; set; }
        public required string name { get; set; }
        [ForeignKey("FK_Position")]
        public required Position position { get; set; }
        [Required]
        public int FK_Position { get; set; }
        public string? sector { get; set; }
        public required int[] waypoints { get; set; } //planet indexes that link to this one
        public override string ToString()
        {
            return sector + ":" + name;
        }
    }
}
