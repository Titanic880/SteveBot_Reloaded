using Microsoft.EntityFrameworkCore;
using Stevebot_DB.DB_Content.HellDiver.API_Objects;

namespace Stevebot_DB.Framework_ENT
{
    internal class SB_DBContext : DbContext
    {
        internal DbSet<SB_Content.Payday.PD2.ListObject> ListObjects { get; set; }
        internal DbSet<DB_Content.DataLogging> DataLogs { get; set; }

        internal DbSet<Position> Positions { get; set; }
        internal DbSet<Messages> messages { get; set; }
        internal DbSet<Planet> Planets { get; set; }
        internal DbSet<NewsFeedMessage> FeedMessages { get; set; }
        internal DbSet<PlanetStatus> PlanetStatuses { get; set; }
        internal DbSet<PlanetEvent> PlanetEvents { get; set; }
        internal DbSet<Event> Events { get; set; }
        internal DbSet<Campaign> Campaigns { get; set; }
        internal DbSet<JointOperations> JointOperations { get; set; }
        internal DbSet<Planet_attack> Planet_Attacks { get; set; }


        /// <summary>
        /// Expecting this to go Nuclear...
        /// </summary>
        internal DbSet<WarStatus_DB> WarStatus { get; set; }

        //internal DbSet<SB_Content.OilMan.GameState> Oilman_States { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //using SqlConnection conn = new("Server=127.0.0.1;Database=SteveBot_Main;User Id=sa;Password=SQLTestServer01!;");
#if DEBUG
            options.UseSqlServer("Server=172.17.0.3;Database=SteveBot_Main;User Id=sa;Password=SQLTestServer01!;TrustServerCertificate=true");
#else
            options.UseSqlServer("Server=127.0.0.1;Database=SteveBot_Main;User Id=sa;Password=SQLTestServer01!;TrustServerCertificate=true");
#endif
        }
    }
}
