using Discord.Commands;

namespace SteveBot_Rebuild.Modules {
    internal class Oilman_API : ModuleBase<SocketCommandContext> {
        #region Oilman
        [Command("om init")]
        private async Task InitOilman()
        {
            Tuple<bool, string> result = await OilmanDiscordInterface.InitilizeNewGame(Context.Message.Author);
            await ReplyAsync(result.Item2);
            if (result.Item1)
            {
                var item = await ReplyAsync(embed:
                    new EmbedBuilder()
                    .WithTitle("Oilman Game:1")
                    .WithDescription("To Choose which layout use the provided reactions"
                    + "\n`Layout 1: Original Games layout`"
                    + "\n`Layout 2: Original Game Alberta layout`"
                    + "\n`Layout 3: 100% random Generated tiles (not balanced)`"
                    + "\nTo add users to the game, use !Add @User"
                    + "\nTo remove users, use !remove @User"
                    + "\nTo Start the game, use !oilman go"
                    + "\n(please wait for Confirm check(or x) after reacting to this command)")
                    .WithFooter("Start Info")
                    .Build());
                await item.AddReactionsAsync(new Emoji[] { ":one:", ":two:", ":three:" });
            }
        }
        [Command("om cancel")]
        private async Task OilmanCancel()
        {
            string result = OilmanDiscordInterface.CancelGame(Context.Message.Author);
            await ReplyAsync(result);
        }
        [Command("om start")]
        private async Task StartOilmanGame()
        {
            Embed result = await OilmanDiscordInterface.StartGame(Context.Message.Author);
            await ReplyAsync(embed: result);
            await OilmanDiscordInterface.StartTurn();
        }
        [Command("om Legend")]
        private async Task OilmanLegend()
        {
            _ = await ReplyAsync(embed: OilmanDiscordInterface.BuildTileLegend());
            _ = await ReplyAsync(embed: new EmbedBuilder()
                .WithTitle("Oilman Game")
                .WithDescription(
                "Game board is A-O across;\n 1-12 down: Top left tile would be a1, bottom right would be o12"
                + $"\nom buy <price> <region>: <region> can be a range or individual tiles EX:\n`A1:B4` or `A1,A2,A3,A4,A5,B1,B2,B3,B4,B5` or `A1:B4,B5`"
                + $"\nom Bid x : puts in a bid of x for the currently selected tiles"
                + $"\nom Claim : claims currently selected tiles"
                + $"\n"
                ).WithCurrentTimestamp()
                .WithFooter("Game Commands")
                .Build());
        }
        [Command("om Buy")]
        private async Task OilmanBuy(int Price, string Positionals)
        {
            string result = await OilmanDiscordInterface.BuyLand(Context.Message.Author, Positionals);
            await ReplyAsync(result);
            await OilmanBid(Price);
        }
        [Command("om bid")]
        private async Task OilmanBid(int amount)
        {
            Embed result = OilmanDiscordInterface.UserBid(Context.Message.Author, amount);
            _ = await ReplyAsync(embed: result);
        }
        [Command("om Claim")]
        private async Task OilmanClaim()
        {
            await ReplyAsync(embed: OilmanDiscordInterface.ClaimLand(Context.Message.Author));
        }
        [Command("om Colors")]
        private async Task OilmanColors()
        {
            var result = await ReplyAsync(embed: OilmanDiscordInterface.BuildColorSelector(Context.Message.Author));
            await result.AddReactionsAsync(OilmanDiscordInterface.GetColorReaction());
        }
        #endregion Oilman
    }
}
