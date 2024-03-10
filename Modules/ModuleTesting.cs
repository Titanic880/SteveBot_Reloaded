using Discord.Commands;


namespace SteveBot_Rebuild.Modules {
    internal class ModuleTesting : ModuleBase<SocketCommandContext> {
        [Command("filetest")]
        public async Task TesterINIT() {
            await ReplyAsync("Test Success!");
        }

    }
}
