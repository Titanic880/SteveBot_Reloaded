using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Stevebot_DB.Framework_ENT;

namespace Stevebot_DB.DB_Content.HellDiver.API_Objects
{
    public class JointOperations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public int hq_node_index { get; set; }
        [ForeignKey("FK_PlanetId")]
        public required Planet planet { get; set; }
        [Required]
        public int FK_PlanetId { get; set; }
    }
}
