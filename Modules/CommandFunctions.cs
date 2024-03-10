﻿using Discord;

namespace SteveBot_Rebuild.Modules
{
    static class CommandFunctions
    {
        #region Links
        private static List<string> Links = new();
        public static List<string> LinksPub { get { return Links; } }
        public const string linkPath         = "Files/Links.txt";
        public const string usercommandsPath = "Files/UserCommands.txt";
        public const string usermessagesPath = "Files/UserMessages.txt";
        public const string ErrorPath        = "Files/Errors.txt";    

        public static void UpdateLinks(List<string>? strlst = null)
        {
            strlst ??= new List<string>(File.ReadAllLines(linkPath));
            //Checks to see if list is empty, if so output console command
            if (strlst.Count == 0)
            {
                Console.WriteLine("Links list was empty, Please check Links File");
                Links ??= new List<string>();
                return;
            }
            Links = strlst;
        }
        public static int AddLink(string link)
        {
            Links.Add(link);
            if(File.ReadAllBytes(linkPath) != null) 
                File.AppendAllText(linkPath, "\n" + link);
            else
                File.AppendAllText(linkPath, link);
            return Links.Count;
        }
        public static void RemoveLink(int input)
        {
            Links.RemoveAt(input);
            //TODO: DB INTERGRATION
            foreach (string tmp in Links)
                File.WriteAllText(linkPath, "\n" + tmp);
        }
        #endregion Links
        #region Logging_DB
        public static void LogUserCommand(Discord.WebSocket.SocketUserMessage userMessage) {
            Stevebot_DB.Framework_ENT.SB_DBLogic.Log(Stevebot_DB.DB_Content.LoggingType.Info, userMessage.Content);
        }
        public static void LogUserMessage(Discord.WebSocket.SocketUserMessage userMessage) {
            Stevebot_DB.Framework_ENT.SB_DBLogic.Log(Stevebot_DB.DB_Content.LoggingType.Info, userMessage.Content);
        }
        public static void LogWarn(string Warning) {
            Stevebot_DB.Framework_ENT.SB_DBLogic.Log(Stevebot_DB.DB_Content.LoggingType.Warning, "System", Warning);
        }
        public static void LogError(string Error) {
            Stevebot_DB.Framework_ENT.SB_DBLogic.Log(Stevebot_DB.DB_Content.LoggingType.Error,"System",Error);
        }
        #endregion
        #region Logging_File
        public static void UserCommand(Discord.WebSocket.SocketUserMessage message)
        {
            //Writes to file
            StreamWriter sW = File.AppendText(usercommandsPath);
            sW.WriteLine($"{message.CreatedAt.DateTime}:{message.Author}: {message.Content}");
            sW.Close();
        }
        public static void UserMessages(Discord.WebSocket.SocketUserMessage message)
        {
            //Writes to file
            StreamWriter sW = File.AppendText(usermessagesPath);
            sW.WriteLine($"{message.CreatedAt.DateTime}:{message.Author}: {message.Content}");
            sW.Close();
        }
        public static void ErrorMessages(string Error)
        {
            LogError(Error);
            /*
            //Writes to file
            StreamWriter sW = File.AppendText(Error);
            sW.WriteLine($"{DateTime.UtcNow}: {Error}");
            sW.Close();*/
        }
        #endregion Logging_File
        #region DiceGames
        public static string DiceRoll(int dice_size)
        {
            Random rand = new();
            return rand.Next(dice_size).ToString();
        }
        #endregion DiceGames
    }
}