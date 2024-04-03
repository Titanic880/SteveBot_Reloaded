using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Stevebot_DB.DB_Content.HellDiver.API_Objects
{
    public class NewsFeedMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public required int id { get; set; }
        [ForeignKey("FK_MessageId")]
        public required Messages Message { get; set; }
        [Required]
        public int FK_MessageId { get; set; }
        public required string published { get; set; }
        [NotMapped]
        public int[]? tag_ids { get; set; }    //Unsure of use, unmapped till discovered
        public int type { get; set; }          //Type of message (unimportant until data can be gathered and compared)
    }
}
