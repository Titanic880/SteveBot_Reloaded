using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Stevebot_DB.DB_Content.HellDiver.API_Objects
{
    public class PlanetEvent
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("FK_CampaignId")]
        public required Campaign campaign { get; set; }
        [Required]
        public int FK_CampaignId { get; set; }
        public int event_type { get; set; }
        public string? expire_time { get; set; }
        public int health { get; set; }
        public int max_health { get; set; }
        [ForeignKey("FK_PlanetId")]
        public required Planet planet { get; set; }
        [Required]
        public int FK_PlanetId { get; set; }
        public string? race { get; set; }
        public string? start_time { get; set; }
    }
}
