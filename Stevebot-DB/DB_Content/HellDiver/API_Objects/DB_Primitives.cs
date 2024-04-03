using Stevebot_DB.Framework_ENT;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stevebot_DB.DB_Content.HellDiver.API_Objects
{
    public enum Races
    {
        Humans,
        Automatons,
        Terminids
    }
    public class Position
    {
        [Key]
        public int PK_Id { get; set; }
        public float x { get; set; }
        public float y { get; set; }
    }
    public class Messages
    {
        [Key]
        public int PK_Id { get; set; }
        public string? de { get; set; }
        public string? en { get; set; }
        public string? es { get; set; }
        public string? fr { get; set; }
        public string? it { get; set; }
        public string? pl { get; set; }
        public string? ru { get; set; }
        public string? zh { get; set; }
    }
    public class WarID
    { // /api return object
        public int current { get; set; }
        public int[]? seasons { get; set; }
    }
    public class Planet_attack
    {
        [Key]
        public int PK_Id { get; set; }
        [NotMapped]
        public Planet source
        {
            get => src ??= HellDiverLogic.GetPlanet(Source);
            set
            {
                Source = value.index;
                src = value;
            }
        }
        [NotMapped]
        public Planet target
        {
            get => trg ??= HellDiverLogic.GetPlanet(Target);
            set
            {
                Target = value.index;
                trg = value;
            }
        }
        private Planet? src;
        private Planet? trg;
        public int Source { get; set; }
        public int Target { get; set; }
    }
}
