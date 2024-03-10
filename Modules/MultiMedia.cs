using Discord.Commands;

namespace SteveBot_Rebuild.Modules {
    internal class MultiMedia : ModuleBase<SocketCommandContext> {
        #region Media
        [Command("addlink")]
        public async Task AddLink(string link) {
            string linktest = link[..5];
            if (linktest.ToLower() != "https") {
                await ReplyAsync("Please provide a link with https at the beginning");
            }
            int tmp = CommandFunctions.AddLink(link);
            await ReplyAsync($"Link added successfully and is #{tmp}");
        }

        [Command("randomlink")]
        public async Task RandomLink() {
            string link = CommandFunctions.LinksPub[new Random().Next(CommandFunctions.LinksPub.Count)];
            await ReplyAsync(link);
        }
        [Command("getlink")]
        public async Task GetLinknum(int input) {
            if (input == 0)
                input = 1;
            if (input > CommandFunctions.LinksPub.Count) {
                await ReplyAsync($"Your index exceeds the size of the list, the list is currently: {input} Links long");
                return;
            }
            await ReplyAsync(CommandFunctions.LinksPub[input - 1]);
        }
        [Command("LLL")]
        public async Task LinkListLength() {
            await ReplyAsync($"The list is: {CommandFunctions.LinksPub.Count} links long!!!");
        }
        [Command("dellink")]
        public async Task DeleteLink(int input) {
            if (input == 0)
                input = 1;
            if (input > CommandFunctions.LinksPub.Count) {
                await ReplyAsync($"Your index exceeds the size of the list, the list is currently: {input}" +
                                  "\nLinks long");
                return;
            }

            CommandFunctions.RemoveLink(input - 1);
            await ReplyAsync("Link removed from list!");
        }
        #endregion Media
    }
}
