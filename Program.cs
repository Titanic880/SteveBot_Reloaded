using SteveBot_Rebuild.Content.Runescape;
using SteveBot_Rebuild.Modules;

namespace SteveBot_Rebuild
{
    internal class Program
    {
        static void Main()
        {
            //Checks to see if all nessecary directories exist, if not it generates them
            if (File_Check())
            {
                _ = new BotProgram();
            }
            return;
        }

        //Checks all Files on runtime
        private static bool File_Check()
        {
            bool result = true;
            if (!Directory.Exists("Files/"))
                Directory.CreateDirectory("Files/");

            if (!File.Exists("Files/auth.json"))
            {
                File.Create("Files/auth.json").Close();
                Console.WriteLine("Please add Token to Files/auth.json to and restart to continue...");
            }
            if (File.ReadAllText("Files/auth.json") == "")
            {
                Console.WriteLine("Auth Token not found, please add it to auth.json in the files folder.");
                Console.ReadLine();
                result = false;
            }

            if (!File.Exists("Files/Runescape.json"))
            {
                File.Create(CommandFunctions.linkPath).Close();
                try
                {
                    using StreamWriter sw = new("Files/Runescape.json");
                    sw.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new RSJson(), Newtonsoft.Json.Formatting.Indented));
                }
                catch (Exception e)
                {
                    CommandFunctions.ErrorMessages(e.Message);
                }
            }
            if (!File.Exists(CommandFunctions.linkPath))
                File.Create(CommandFunctions.linkPath).Close();
            else
                CommandFunctions.UpdateLinks();

            if (!File.Exists(CommandFunctions.ErrorPath))
                File.Create(CommandFunctions.ErrorPath).Close();
            if (!File.Exists(CommandFunctions.usercommandsPath))
                File.Create(CommandFunctions.usercommandsPath).Close();

            if (!File.Exists(CommandFunctions.usermessagesPath))
                File.Create(CommandFunctions.usermessagesPath).Close();

            return result;
        }
    }
}