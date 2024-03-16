namespace SteveBot_Rebuild.Modules
{
    static class CommandFunctions
    {
        public static bool DBLog = true;
        #region Links
        private static List<string> Links = [];
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
                Links ??= [];
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

        #region Logging_Entry
        public static void Log_UserCommand(Discord.WebSocket.SocketUserMessage userMessage) {
            if (DBLog) {
                DBLogUserCommand(userMessage);
            } else {
                FileUserCommand(userMessage);
            }
        }
        public static void Log_UserMessage(Discord.WebSocket.SocketUserMessage userMessage) {
            if (DBLog) {
                DBLogUserMessage(userMessage);
            } else {
                FileUserMessages(userMessage);
            }
        }
        public static void LogError(string Error) {
            if (DBLog) {
                DBLogError(Error);
            } else {
                FileErrorMessages(Error);
            }
        }
        public static void LogWarning(string warning) {
            if (DBLog) {
                DBLogWarn(warning);
            } 
        }
        #endregion Logging_Entry
        #region Logging_DB
        private static void DBLogUserCommand(Discord.WebSocket.SocketUserMessage userMessage) {
            Stevebot_DB.Framework_ENT.SB_DBLogic.Log(Stevebot_DB.DB_Content.LoggingType.Info, userMessage.Content);
        }
        private static void DBLogUserMessage(Discord.WebSocket.SocketUserMessage userMessage) {
            Stevebot_DB.Framework_ENT.SB_DBLogic.Log(Stevebot_DB.DB_Content.LoggingType.Info, userMessage.Content);
        }
        private static void DBLogWarn(string Warning) {
            Stevebot_DB.Framework_ENT.SB_DBLogic.Log(Stevebot_DB.DB_Content.LoggingType.Warning, "System", Warning);
        }
        private static void DBLogError(string Error) {
            Stevebot_DB.Framework_ENT.SB_DBLogic.Log(Stevebot_DB.DB_Content.LoggingType.Error,"System",Error);
        }
        #endregion
        #region Logging_File
        private static void FileUserCommand(Discord.WebSocket.SocketUserMessage message) { 
            //Writes to file
            StreamWriter sW = File.AppendText(usercommandsPath);
            sW.WriteLine($"{message.CreatedAt.DateTime}:{message.Author}: {message.Content}");
            sW.Close();
        }
        private static void FileUserMessages(Discord.WebSocket.SocketUserMessage message) {
            //Writes to file
            StreamWriter sW = File.AppendText(usermessagesPath);
            sW.WriteLine($"{message.CreatedAt.DateTime}:{message.Author}: {message.Content}");
            sW.Close();
        }
        private static void FileErrorMessages(string Error) {
            //Writes to file
            StreamWriter sW = File.AppendText(Error);
            sW.WriteLine($"{DateTime.UtcNow}: {Error}");
            sW.Close();
        }
        #endregion Logging_File
    }
}