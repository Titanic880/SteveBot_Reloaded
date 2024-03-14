using Discord.WebSocket;
using Discord;
using CommandModule;

namespace SteveBot_Rebuild {
    internal class ReactionHandler {

        public static async Task HandlerEntry(Cacheable<IUserMessage, ulong> msgCache, SocketReaction reaction) {
            if (reaction.User.Value.IsBot) {
                return;
            }
            IUserMessage? message = await msgCache.GetOrDownloadAsync();
            if(message is null) {
                return;
            }
            //Data Validation
            IEmote checkmarkemote = message.Reactions.Keys.FirstOrDefault(x => x == new Emoji("\u2705"))!;
            ReactionMetadata data = message.Reactions[checkmarkemote];
            if (!data.IsMe || data.ReactionCount < 1) {
                return;
            }

            Embed? result;
            switch (message.Embeds.FirstOrDefault()!.Title.Split(':')[0]) {
            case "Payday 2 Randomizer":
                result = ReactionHandler.RandomizerHandling(
                    message,7,new SB_Content.Payday.PD2.Randomizer());
                break;
            case "COD Cold War Randomizer":
                result = ReactionHandler.RandomizerHandling(
                    message,8,new SB_Content.Call_of_Duty.Randomizer.ZombRandLib());
                break;
            /*case "Oilman Game":
                if (message.Embeds.FirstOrDefault()!.Footer.ToString() == "Start Info")
                {
                    //Emoji[] reac = new Emoji[] { ":one:", ":two:", ":three:" };
                    int throwup = 0;
                    //Find a reaction with a count not equal to bot add
                    if (message.Reactions[emojis[0]].ReactionCount != 1)
                        throwup = 1;
                    else if (message.Reactions[emojis[1]].ReactionCount != 1)
                        throwup = 2;
                    else if (message.Reactions[emojis[2]].ReactionCount != 1)
                        throwup = 3;
                    //Pass layout to GameHandler to find the game and generate the layout
                    if (await SB_Content.OilMan.OilmanDiscordInterface.ReactionLayoutSelected(reaction.User.Value, throwup))
                        await message.AddReactionAsync(new Emoji("✅"));
                    else await message.AddReactionAsync(new Emoji(":x:"));
                }
                else if(message.Embeds.First()!.Footer.ToString() == "Color Selector")
                {
                    Tuple<bool,string> result = await SB_Content.OilMan.OilmanDiscordInterface.PlayerColorReaction(reaction.User.Value, new Emoji(reaction.Emote.Name));
                    await reaction.Channel.SendMessageAsync($"{reaction.User.Value.Mention} {result.Item2}");
                }
                break;*/
            default:
                return;
            }

            if (result is null) {
                await reaction.Channel.SendMessageAsync($"<@{reaction.UserId}> I am sorry, a problem has occoured while trying to act on your input!");
                return;
            }
            await reaction.Channel.SendMessageAsync($"<@{reaction.UserId}> ", embed: result);
        }
        private static Embed? RandomizerHandling(IUserMessage message, int DataSize, SB_Content.IRandomizer RandomizerLibrary) {
            Emoji[] emoj = new Emoji[DataSize];
            Array.Copy(Command_Identifiers.Emojis, emoj, DataSize);
            bool[] rand = new bool[DataSize];
            //loop through the reactions and check if its been added (in this case its setting toggles)
            for (int i = 0; i < DataSize; i++) {
                if (message.Reactions[emoj[i]].ReactionCount != 1) {
                    rand[i] = true;
                }
            }
            RandomizerLibrary.Randomize(rand);
            EmbedBuilder builder = new EmbedBuilder()
                            .WithTitle("Randomizer Results")
                            .WithTimestamp(DateTime.UtcNow)
                            .WithDescription(RandomizerLibrary.GetResult());
            return builder.Build();
        }
    }

}
