using System;

namespace Stevebot_DB.Framework_ENT
{
    public class SB_DBLogic
    {
        private static readonly DB_Content.LoggingType level = DB_Content.LoggingType.Warning;
        public static void Log(DB_Content.LoggingType LogLevel, string message, string error = "") {
            if(LogLevel < level) {
                return;
            }
            SB_DBContext dbc = new();
            DB_Content.DataLogging log = new() {
                Message = message,
                Time = DateTime.Now,
                Type = LogLevel,
                AdditionalInfo = error
            };
            dbc.DataLogs.Add(log);
            dbc.SaveChanges();
        }
    }
}
