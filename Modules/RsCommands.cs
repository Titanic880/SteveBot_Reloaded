using Discord.Commands;
using System.Threading.Tasks;

namespace SteveBot_Rebuild.Modules
{
    internal class RsCommands : ModuleBase<SocketCommandContext>
    {
        [Command("ritual")]
        public async Task Ritual()
        {
            await ReplyAsync("Ritual Commands!");
        }
    }
}
