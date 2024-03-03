using Microsoft.EntityFrameworkCore;
using System.Data.SQLite;
using System.IO;

namespace Stevebot_DB.Framework_ENT
{
    internal class SB_DBContext : DbContext
    {
        internal DbSet<SB_Content.Runescape.RSJson> Rs_Data { get; set; }
        internal DbSet<SB_Content.Payday.PD2.Perk_Deck> PD2_PerkDecks { get; set; }
        internal DbSet<SB_Content.Payday.PD2.Equipment> PD2_Equipment { get; set; }
        internal DbSet<SB_Content.Payday.PD2.Difficulty> PD2_Difficulty { get; set; }


        //internal DbSet<SB_Content.OilMan.GameState> Oilman_States { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            using SQLiteConnection conn = new($"Data Source=Files/SBDB.db;Version=3");
            if (!File.Exists("Files/SBDB.db"))
            {
                SQLiteConnection.CreateFile("Files/SBDB.db");
            }
            options.UseSqlite("Data Source=Files/SBDB.db;Version=3;");
        }
    }
}
