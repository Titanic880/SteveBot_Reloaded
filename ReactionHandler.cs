using Discord.WebSocket;
using Discord;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SteveBot_Rebuild.Modules;

namespace SteveBot_Rebuild {
    internal class ReactionHandler {

        public static async Task HandlerEntry(Cacheable<IUserMessage, ulong> msgCache, Cacheable<IMessageChannel, ulong> msgChannel, SocketReaction reaction) {
            if (reaction.User.Value.IsBot) {
                return;
            }
            IUserMessage? message = await msgCache.GetOrDownloadAsync();
            if(message is null) {
                return;
            }
            //Data Validation??
            IEmote checkmarkemote = message.Reactions.Keys.FirstOrDefault(x => x.Name == "✅")!;
            ReactionMetadata data = message.Reactions[checkmarkemote];

            //Attempting to use early return to increase readability (unsure of how it will work in prod)
            //if (checkmarkemote.Name == "✅" && data.IsMe && data.ReactionCount > 1) {
            if (checkmarkemote.Name != "✅" || !data.IsMe || data.ReactionCount < 1) {
                return;
            }

            //In switch items
            Embed? result;

            switch (message.Embeds.FirstOrDefault()!.Title.Split(':')[0]) {
            case "Payday 2 Randomizer":
                result = ReactionHandler.RandomizerHandling(message,8,new SB_Content.Payday.PD2.Randomizer());
                break;
            case "COD Cold War Randomizer":
                result = ReactionHandler.RandomizerHandling(message,9,new SB_Content.Call_of_Duty.Randomizer.ZombRandLib());
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
                await reaction.Channel.SendMessageAsync($"<@{reaction.UserId}> I am sorry, a problem has occoured while trying to randomize your data!");
                return;
            }
            await reaction.Channel.SendMessageAsync($"<@{reaction.UserId}> ", embed: result);
        }

        private static Embed? Payday2Handling(IUserMessage message, SocketReaction reaction) {
            try {
                //In switch items
                Emoji[] emoj = new Emoji[8];
                Array.Copy(emojis, emoj, 8);
                bool[] rand = new bool[8];

                //Data Validation??
                IEmote checkmarkemote = message.Reactions.Keys.FirstOrDefault(x => x.Name == "✅")!;
                ReactionMetadata data = message.Reactions[checkmarkemote];
                //Attempting to use early return to increase readability (unsure of how it will work in prod)
                //if (checkmarkemote.Name == "✅" && data.IsMe && data.ReactionCount > 1) {
                if (checkmarkemote.Name != "✅" || !data.IsMe || data.ReactionCount < 1) {
                    return null;
                }

                //loop through the reactions and check if its been added (in this case its setting toggles)
                for (int i = 0; i < emoj.Length; i++) {
                    if (message.Reactions[emoj[i]].ReactionCount != 1) {
                        rand[i] = true;
                    }
                }

                //Run the Randomizer
                SB_Content.Payday.PD2.Randomizer pd2data = new();
                pd2data.Randomize(rand);

                //Build User visual
                EmbedBuilder builder = new EmbedBuilder()
                            .WithTitle("Payday 2 Randomizer Results")
                            .WithTimestamp(DateTime.UtcNow)
                            .WithDescription(pd2data.GetResult());

                return builder.Build();
            } catch (Exception e) {
                CommandFunctions.ErrorMessages(e.Message);
                return null;
            }
        }
        private static Embed? CODColdWarHandling(IUserMessage message, SocketReaction reaction) {
            //In switch items
            Emoji[] emoj = new Emoji[9];
            Array.Copy(emojis, emoj, 9);
            bool[] rand = new bool[9];
            //Data Validation??
            IEmote checkmarkemote = message.Reactions.Keys.FirstOrDefault(x => x.Name == "✅")!;
            ReactionMetadata data = message.Reactions[checkmarkemote];
            
            //Attempting to use early return to increase readability (unsure of how it will work in prod)
            //if (checkmarkemote.Name == "✅" && data.IsMe && data.ReactionCount > 1) {
            if (checkmarkemote.Name != "✅" || !data.IsMe || data.ReactionCount < 1) {
                return null;
            }

            //loop through the reactions and check if its been added (in this case its setting toggles)
            for (int i = 0; i < emoj.Length; i++) {
                if (message.Reactions[emoj[i]].ReactionCount != 1) {
                    rand[i] = true;
                }
            }
            SB_Content.Call_of_Duty.Randomizer.ZombRandLib randlib = new();
            randlib.ApplyOptions(rand);
            randlib.Randomize();

            EmbedBuilder builder = new EmbedBuilder()
                            .WithTitle("Cold War Randomizer Results")
                            .WithTimestamp(DateTime.UtcNow)
                            .WithDescription(randlib.GetResult());
            return builder.Build();
        }

        private static Embed? RandomizerHandling(IUserMessage message, int DataSize, SB_Content.IRandomizer RandomizerLibrary) {
            //In switch items
            Emoji[] emoj = new Emoji[DataSize];
            Array.Copy(emojis, emoj, DataSize);
            bool[] rand = new bool[DataSize];
            //loop through the reactions and check if its been added (in this case its setting toggles)
            for (int i = 0; i < emoj.Length; i++) {
                if (message.Reactions[emoj[i]].ReactionCount != 1) {
                    rand[i] = true;
                }
            }
            RandomizerLibrary.Randomize(rand);
            EmbedBuilder builder = new EmbedBuilder()
                            .WithTitle("Cold War Randomizer Results")
                            .WithTimestamp(DateTime.UtcNow)
                            .WithDescription(RandomizerLibrary.GetResult());
            return builder.Build();
        }


    /// <summary>
        /// TODO: REPLACE WHAT EVER THE LIVING FUCK THIS SHIT IS
        /// </summary>
    public static readonly Emoji[] emojis = new Emoji[]
    {
            "1️⃣" ,
            "2️⃣" ,
            "3️⃣" ,
            "4️⃣" ,
            "5️⃣" ,
            "6️⃣" ,
            "7️⃣" ,
            "8️⃣" ,
            "9️⃣" ,
            "✅" ,
    };
    }

}
