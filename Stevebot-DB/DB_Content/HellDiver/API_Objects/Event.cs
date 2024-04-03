using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Stevebot_DB.Framework_ENT;

namespace Stevebot_DB.DB_Content.HellDiver.API_Objects
{
    public class Event
    {    //GlobalEventSchema
        [Key]
        public int id { get; set; }
        [NotMapped]
        public int Assignment_id_32 { get; set; }
        public required string[] effects { get; set; }
        [NotMapped]
        public int flag { get; set; }
        [NotMapped]
        public int id_32 { get; set; }
        [ForeignKey("FK_MessageId")]
        public required Messages Message { get; set; }
        /*
        {
            get => HellDiverLogic.GetMessage(msg_id)!;
            set
            {
                if (HellDiverLogic.GetMessage(msg_id) != null)
                {
                    msg_id = value.PK_Id;
                    return;
                }
                HellDiverLogic.AddMessage(value);
            }
        }*/
        [NotMapped]
        public int message_id_32;
        [NotMapped]
        public required Planet[] planets { get; set; }
        public string Planets_DB
        {
            get
            {
                if (planets is null)
                {
                    return "";
                }
                string val = "";
                for (int i = 0; i < planets.Length; i++)
                {
                    val += planets[i].index + ',';
                }
                return val.TrimEnd(',');
            }
            set
            {
                string[] vals = value.Split(',');
                int[] valint = new int[vals.Length];
                for (int i = 0; i < vals.Length; i++)
                {
                    valint[i] = int.Parse(vals[i]);
                }
                planets = HellDiverLogic.GetPlanets(valint);
            }
        }
        [NotMapped]
        public int portrait_id_32 { get; set; }
        public string race { get; set; } = "Human";
        public string? title { get; set; }
        [NotMapped]
        public int title_32 { get; set; }
    }
}
