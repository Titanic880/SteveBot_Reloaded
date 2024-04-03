using Stevebot_DB.DB_Content.HellDiver.API_Objects;
using Stevebot_DB.DB_Content.HellDiver;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Stevebot_DB.Framework_ENT
{
    public static class HellDiverLogic {
        #region Updates
        public static bool UpdateDatabase() {
            bool AllPass = true;
            //TODO:Adjust Order, Logging
            if (!UpdatePlanets()) {
                AllPass = false;
            }
            if (!UpdateWarStatus()) {
                AllPass = false;

            }
            return AllPass;
        }
        private static bool UpdatePlanets() {
            //TODO: Replace entries if newer planets have different info
            List<Planet> planets = [.. API.GetPlanets()];
            using SB_DBContext context = new();
            if(!context.Planets.Any()) {
                context.Planets.AddRange(planets);
                context.SaveChanges();
                return true;
            }
            //remove all exact matchs
            planets = planets.Except(context.Planets).ToList();
            //Loop through via index & hash to swap out new planets
            for (int i = 0; i < planets.Count; i++) {
                if (context.Planets.Where(x => x.index == planets[i].index).Any()) {
                    if (context.Planets.Where(x => x.index == planets[i].index).First().hash != planets[i].hash) {
                        context.Planets.Remove(context.Planets.Where(x => x.index == planets[i].index).First());
                        context.Planets.Add(planets[i]);
                    }
                }
            }
            context.SaveChanges();
            return true;
        }
        private static bool UpdateWarStatus() {
            WarStatus status = API.GetWarStatus();
            UpdateCampaign(status.campaigns);
            UpdateJointOps(status.joint_operations);
            UpdatePlanetAttacks(status.planet_attacks);
            UpdatePlanetStatus(status.planet_status);
            using SB_DBContext context = new();
           WarStatus_DB dbStatus = new(status);
            context.WarStatus.Add(dbStatus);
            context.SaveChanges();
            return true;
        }
        private static void UpdateCampaign(Campaign[] content) {
            using SB_DBContext context = new();
            for (int i = 0; i < content.Length; i++) {
                //Get All Versions of the PlanetStatus
                Campaign[] entries = [.. context.Campaigns.Where(x=>x.id==content[i].id)];
                if (entries != null) {
                    context.Campaigns.RemoveRange(entries);
                    context.SaveChanges();
                    context.Campaigns.Attach(content[i]);
                    context.Entry(content[i]).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    context.SaveChanges();
                }
            }
        }

        private static void UpdateJointOps(JointOperations[] content) {
            using SB_DBContext context = new();
            for (int i = 0; i < content.Length; i++) {
                //Get All Versions of the PlanetStatus
                JointOperations[] entries = [.. context.JointOperations.Where(x=>x.FK_PlanetId==content[i].planet.index)];
                if (entries != null) {
                    context.JointOperations.RemoveRange(entries);
                    context.SaveChanges();
                    context.JointOperations.Attach(content[i]);
                    context.Entry(content[i]).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    context.SaveChanges();
                }
            }
        }
        private static void UpdatePlanetAttacks(Planet_attack[] content) {
            using SB_DBContext context = new();
            for (int i = 0; i < content.Length; i++) {
                Planet_attack[] entries = [.. context.Planet_Attacks.Where(x=>x.PK_Id==content[i].PK_Id)];
                if (entries != null) {
                    context.Planet_Attacks.RemoveRange(entries);
                    context.SaveChanges();
                    context.Planet_Attacks.Attach(content[i]);
                    context.Entry(content[i]).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    context.SaveChanges();
                }
            }
        }
        private static void UpdatePlanetStatus(PlanetStatus[] content) {
            using SB_DBContext context = new();
            for (int i = 0; i < content.Length; i++) {
                //Get All Versions of the PlanetStatus
                PlanetStatus[] entries = [.. context.PlanetStatuses.Where(x=>x.FK_PlanetId==content[i].planet.index)];
                if (entries != null) {
                    context.PlanetStatuses.RemoveRange(entries);
                    context.SaveChanges();
                    context.PlanetStatuses.Attach(content[i]);
                    context.Entry(content[i]).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    context.SaveChanges();
                }
            }
        }
        #endregion Updates
        #region Public_GetVals
        public static Planet GetPlanet(int index) {
            using SB_DBContext context = new();
            return context.Planets.Where(x => x.index == index).First();
        }

        public static string GetConnectedPlanets(int[] indexes) {
            string ret = "";
            foreach(int a in indexes) {
                ret += GetPlanet(a).ToString() + Environment.NewLine;
            }
            return ret;
        }
        public static Planet GetPlanet(string FuzzyName) {
            using SB_DBContext context = new();
            return context.Planets.Where(x=>x.name.Contains(FuzzyName)).First();
        }
        public static Event[] GetEvents(int[] Input) {
            using SB_DBContext context = new();
            return [.. context.Events.Where(x => Input.Contains(x.id))];
        }
        public static Campaign[] GetCampaigns(int[] Input) {
            using SB_DBContext context = new();
            return [.. context.Campaigns.Where(x => Input.Contains(x.id))];
        }
        public static Planet_attack[] GetPlanet_Attacks(int[] Input) {
            using SB_DBContext context = new();
            return [.. context.Planet_Attacks.Where(x => Input.Contains(x.PK_Id))];
        }
        public static PlanetStatus[] GetPlanetStatuses(int[] Input) {
            using SB_DBContext context = new();
            return [.. context.PlanetStatuses.Where(x => Input.Contains(x.id))];
        }
        public static PlanetStatus[] GetAllPlanetStatus() {
            using SB_DBContext context = new();
            return [.. context.PlanetStatuses];
        }
        public static Planet[] GetPlanets(int[] Input) {
            using SB_DBContext context = new();
            return [.. context.Planets.Where(x => Input.Contains(x.index))];
        }
        #endregion Public_GetVals
        #region internal_GetVals
        internal static Campaign? GetCampaign(int index) {
            using SB_DBContext context = new();
            return context.Campaigns.Where(x => x.id == index).FirstOrDefault();
        }
        internal static void AddCampaign(Campaign camp) {
            using SB_DBContext context = new();
            if (!context.Campaigns.Where(x => x.id == camp.id).Any()) {
                context.Campaigns.Add(camp);
                context.SaveChanges();
            }
        }
        internal static Position? GetPosition(int index) {
            using SB_DBContext context = new();
            var tmp = context.Positions.Where(x => x.PK_Id == index).FirstOrDefault();
            return tmp;
        }
        internal static bool PositionCheck(Position toCheck) {
            using SB_DBContext context = new();
            if(context.Positions.Where(x=> x.y == toCheck.y && x.x == toCheck.x).Any()) {
                return true;
            }
            return false;
        }
        internal static void AddPosition(Position pos) {
            using SB_DBContext context = new();
            if (context.Positions.Where(x => x.PK_Id == pos.PK_Id).Any()) {
                return;
            }
            context.Positions.Add(pos);
            context.SaveChanges();
        }
        internal static Messages? GetMessage(int index) {
            using SB_DBContext context = new();
            return context.messages.Where(x => x.PK_Id == index).FirstOrDefault();
        }
        internal static void AddMessage(Messages msg) {
            using SB_DBContext context = new();
            if (!context.messages.Where(x => x.PK_Id == msg.PK_Id).Any()) {
                context.messages.Add(msg);
                context.SaveChanges();
            }
        }
        #endregion internal_GetVals
    }
}
