using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Stevebot_DB.DB_Content.HellDiver.API_Objects
{
    public class WarStatus
    {
        [Key]
        public int id { get; set; }
        [NotMapped]
        public int[]? active_election_policy_effects { get; set; }    //Unsure of use
        [NotMapped]
        public Campaign[] campaigns { get; set; }

        [NotMapped]
        public int[]? community_targets { get; set; }                 //Unsure of use
        [NotMapped]
        public Event[]? global_events { get; set; }
        public float impact_multiplier { get; set; }
        [NotMapped]
        public JointOperations[] joint_operations { get; set; }
        [NotMapped]
        public int[]? planet_active_effects { get; set; }             //Unsure of use
        [NotMapped]
        public Planet_attack[] planet_attacks { get; set; }
        [NotMapped]
        public PlanetStatus[] planet_status { get; set; }
        public string snapshot_at { get; set; }
        public string started_at { get; set; }
        public int war_id { get; set; }

        public WarStatus()
        {
            campaigns = [];
            joint_operations = [];
            planet_attacks = [];
            planet_status = [];
            snapshot_at = "";
            started_at = "";
        }
        public WarStatus(WarStatus status)
        {
            id = status.id;
            active_election_policy_effects = status.active_election_policy_effects;
            campaigns = status.campaigns;
            community_targets = status.community_targets;
            global_events = status.global_events;
            impact_multiplier = status.impact_multiplier;
            joint_operations = status.joint_operations;
            planet_active_effects = status.planet_active_effects;
            planet_attacks = status.planet_attacks;
            planet_status = status.planet_status;
            snapshot_at = status.snapshot_at;
            started_at = status.started_at;
            war_id = status.war_id;
        }
    }
    #region DB_types
    public class WarStatus_DB : WarStatus
    {
        public WarStatus_DB()
        {

        }
        public WarStatus_DB(WarStatus status) : base(status)
        {
            //This is very broken but my tired brain says this is the best way (extract all to a method with proper inputs)
            Campaigns_DB = MakeCamp_DB();
            Global_Events_DB = MakeEvent_DB();
            Planet_Attacks_DB = MakePlanetAttack_DB();
        }
        private string MakeCamp_DB()
        {
            if (campaigns is null)
            {
                return "";
            }
            string val = "";
            for (int i = 0; i < campaigns.Length; i++)
            {
                val += campaigns[i].id + ",";
            }
            return val.TrimEnd(',');
        }
        private void SetCamp_DB(string input)
        {
            string[] vals = [input];
            if (input == "")
            {
                return;
            }
            else if (input.Contains(','))
            {
                vals = input.Split(',');
            }
            int[] valint = new int[vals.Length];
            for (int i = 0; i < vals.Length; i++)
            {
                valint[i] = int.Parse(vals[i]);
            }
            campaigns = Framework_ENT.HellDiverLogic.GetCampaigns(valint);
            campDB = input;
        }
        public string Campaigns_DB
        {
            get => campDB;
            set => SetCamp_DB(value);
        }
        private string campDB = "";

        private string MakeEvent_DB()
        {
            if (global_events is null)
            {
                return "";
            }
            string val = "";
            for (int i = 0; i < global_events.Length; i++)
            {
                val += global_events[i].id + ",";
            }
            return val.TrimEnd(',');
        }
        private void SetEvent_DB(string input)
        {
            string[] vals = [input];
            if (input == "")
            {
                return;
            }
            else if (input.Contains(','))
            {
                vals = input.Split(',');
            }
            int[] valint = new int[vals.Length];
            for (int i = 0; i < vals.Length; i++)
            {
                valint[i] = int.Parse(vals[i]);
            }
            global_events = Framework_ENT.HellDiverLogic.GetEvents(valint);
        }
        public string Global_Events_DB
        {
            get => events_db;
            set => SetEvent_DB(value);
        }
        private readonly string events_db = "";

        private string MakePlanetAttack_DB()
        {
            if (planet_attacks is null)
            {
                return "";
            }
            string val = "";
            for (int i = 0; i < planet_attacks.Length; i++)
            {
                val += planet_attacks[i].PK_Id + ",";
            }
            return val.TrimEnd(',');
        }
        private void SetPlanetAttack_DB(string input)
        {
            string[] vals = [input];
            if (input == "")
            {
                return;
            }
            else if (input.Contains(','))
            {
                vals = input.Split(',');
            }
            int[] valint = new int[vals.Length];
            for (int i = 0; i < vals.Length; i++)
            {
                valint[i] = int.Parse(vals[i]);
            }
            planet_attacks = Framework_ENT.HellDiverLogic.GetPlanet_Attacks(valint);
        }
        public string Planet_Attacks_DB
        {
            get => planetattack_DB;
            set => SetPlanetAttack_DB(value);
        }
        private readonly string planetattack_DB = "";
    }
    #endregion DB_types
}