using Discord.Commands;
using Discord;

namespace CommandModule.Modules {
    internal class Randomizer_Functions : ModuleBase<SocketCommandContext>  {
        #region Payday
        [Command("pd2Help")]
        public async Task PD2Help() {
            EmbedBuilder EmbedBuilder = new EmbedBuilder()
.WithTitle($"Command prefix is '{Command_Identifiers.Command_Prefix}' (Commands are not case sensitive)")
.WithDescription("pd2rando <setting> (only supports all currently)"
)
.WithCurrentTimestamp();
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }
        [Command("PD2Rand")]
        public async Task PD2Rand() {
            EmbedBuilder EmbedBuilder = new EmbedBuilder()
                .WithTitle($"Payday 2 Randomizer")
                .WithDescription("React with the provided to change settings:" +
                "\n1️⃣ Randomize Perk Deck" +
                "\n2️⃣ Randomize Throwable" +
                "\n3️⃣ Randomize Primary" +
                "\n4️⃣Randomize Secondary" +
                "\n5️⃣ Randomize Melee" +
                "\n6️⃣ Randomize Deployable" +
                "\n7️⃣ Randomize Armor" +
                "\n8️⃣ Randomize Difficulty" +
                "\n✅ Randomize!"
               ).WithCurrentTimestamp();
            Embed embed = EmbedBuilder.Build();
            var item = await ReplyAsync(embed: embed);
            IEnumerable<Emoji> Reaction = Command_Identifiers.Emojis.Except([new("\u0039\u20E3")]);
            await item.AddReactionsAsync(Reaction);
        }
        #endregion Payday

        #region Call of Duty
        [Command("cwzRandfast")]
        public async Task CODCWRandomizer() {
            SB_Content.Call_of_Duty.Randomizer.ZombRandLib randlib = new();

            randlib.TrueRandomize();
            EmbedBuilder EmbedBuilder = new EmbedBuilder()
                .WithTitle($"Payday 2 Randomizer")
                .WithDescription(randlib.GetResult())
                .WithCurrentTimestamp();
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }
        [Command("cwzRand")]
        public async Task CODCWReatRandomzier() {
            EmbedBuilder EmbedBuilder = new EmbedBuilder()
    .WithTitle($"COD Cold War Randomizer")
    .WithDescription("React with the provided to change settings:" +
    $"\n1️⃣ Randomize Weapon" +
    $"\n2️⃣ DLC Enabled" +
    $"\n3️⃣ Randomize Box Ban" +
    $"\n4️⃣ Randomize Wall Ban" +
    $"\n5️⃣ Randomize Field Upgrade" +
    $"\n6️⃣ Randomize Perk Order" +
    $"\n7️⃣ Randomize Support Item" +
    $"\n8️⃣ Randomize Tactical Item" +
    $"\n9️⃣ Randomize Lethal Item" +
    $"\n✅  Randomize!"
   ).WithCurrentTimestamp();
            Embed embed = EmbedBuilder.Build();
            var item = await ReplyAsync(embed: embed);
            await item.AddReactionsAsync(Command_Identifiers.Emojis);
        }
        #endregion Call of Duty
    }
}
