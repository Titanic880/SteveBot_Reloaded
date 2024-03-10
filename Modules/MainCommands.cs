﻿using Discord.Commands;
using Discord;

using SB_Content;
using SB_Content.BlackJack;

namespace SteveBot_Rebuild.Modules {
    public partial class MainCommands : ModuleBase<SocketCommandContext> {
        public bool LongTask = false;
        private int timerSeconds = 0;
        #region Help Commands
        [Command("help")]
        public async Task Help() {
            EmbedBuilder EmbedBuilder = new EmbedBuilder()
                    .WithTitle($"Command prefix is '{BotProgram.PrefixChar}'")
                    .WithDescription("  help : displays this command" +
                        "\npd2rand : Randomizer for Payday 2" +
                        "\ncwzRand : Randomizer for Black ops Cold war Zombies" +
                        "\nslap : you what?" +
                        "\nroll : Rolls a specified dice; 4,6,8,10,20,100 (default:6)" +
                        "\nban  : ban <User> <Comment>" +
                        "\nunban: unban <User> <Comment>" +
                        "\nkick : kick <User> <Comment>" +
                        "\nblackjack Autoplay x: will play x games of blackjack" +
                        "\nmath : Accepts an equation and will calculate using PEDMAS" +
                        "\ncalculator : Calculator Help")
                    .WithCurrentTimestamp();
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }
        [Command("calculator")]
        public async Task Calc() {
            EmbedBuilder EmbedBuilder = new EmbedBuilder()
        .WithTitle($"Command prefix is '{BotProgram.PrefixChar}'")
        .WithDescription("  calculator : displays this command" +
            "\nadd : Basic Addition <Num1>  <Num2>" +
            "\nsub  : Basic Subtraction <Num2>  <Num1>" +
            "\nmult:  Multiplication <Num1>  <Num2>" +
            "\ndiv : Division <Num1>  <Num2>" +
            "\ndec2hex : Converts a decmial number to a hexidecimal number <Num1>" +
            "\nhex2dec : Converts a hexidecimal to a decimal number <Num1>" +
            "\nfact : Factorial of the number <Num1>")
        .WithCurrentTimestamp();
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }
        [Command("linking")]
        public async Task Linking() {
            EmbedBuilder EmbedBuilder = new EmbedBuilder()
        .WithTitle("Command prefix is '!'")
        .WithDescription("  linking : displays this command" +
            "\naddlink : Adds a link to the list" +
            "\nrandomlink : gets a random link out of the list" +
            "\ngetlink# : pulls a link from the specific index" +
            "\nLLL : returns length of the list" +
            "\ndellink : Removes link from the list")
        .WithCurrentTimestamp();
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }
        #endregion Help Commands
        #region Standard Commands
        [Command("ping")]
        public async Task Ping() {
            await ReplyAsync("Pong");
        }
        [Command("pong")]
        public async Task Pong() {
            await ReplyAsync("That's my line!");
        }
        [Command("kek")]
        public async Task Kek() {
            await ReplyAsync("LOL");
        }
        [Command("slap")]
        public async Task Slap(IGuildUser? user = null) {
            if (user == null)
                await ReplyAsync("You Slapped yourself!");
            else
                await ReplyAsync("You Slapped " + user.Mention);
        }
        [Command("dice")]
        public async Task Roll(int dice_size = 6) {
            await ReplyAsync($"You rolled a {CommandFunctions.DiceRoll(dice_size + 1)}");
        }
        [Command("k")]
        public async Task K() {
            await ReplyAsync("https://tenor.com/view/bet-gif-5301020");
        }


        #endregion Standard Commands
        
        #region TEST
        [Command("test")]
        public async Task Test() {
            await ReplyAsync("twas but a test!");
        }

        #endregion TEST
        #region Bans
        [Command("ban")]
        [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "YOU DONT GOT ENOUGH COFFEE FOR THIS!")]
        public async Task BanMember(IGuildUser? user = null, [Remainder] string reason = "Default Reason") {

            if (user == null) {
                await ReplyAsync("Please specify a user!");
                return;
            }
            await Context.Guild.AddBanAsync(user, 1, reason);

            EmbedBuilder EmbedBuilder = new EmbedBuilder()
                .WithDescription($"{user.Mention} was banned \n **Reason** {reason}")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("User Banned");
                });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }

        [Command("unban")]
        [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "You have no coffee for me, find some.")]
        public async Task UnBanMember(IGuildUser? user = null, [Remainder] string reason = "Default Reason") {
            if (user == null) {
                await ReplyAsync("Please specify a user!");
                return;
            }

            await Context.Guild.RemoveBanAsync(user);

            EmbedBuilder EmbedBuilder = new EmbedBuilder()
                .WithDescription($"{user.Mention} was unbanned \n **Reason** {reason}")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("User unBanned");
                });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }
        [Command("kick")]
        [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "I need some coffee before I kick someone.")]
        public async Task Kick(IGuildUser? user = null, [Remainder] string reason = "Default Reason") {
            if (user == null) {
                await ReplyAsync("Please specify a user!");
                return;
            }
            reason ??= "N/A";

            await Context.Guild.AddBanAsync(user, 0, reason);
            await Context.Guild.RemoveBanAsync(user);

            EmbedBuilder EmbedBuilder = new EmbedBuilder()
                .WithDescription($"{user.Mention} was kicked \n **Reason** {reason}")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("User was kicked");
                });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }
        #endregion Bans
        
        #region BlackJack
        [Command("blackjack Autoplay")]
        public async Task BJAutoPlay(int games = 1) {
            if (LongTask) {
                await ReplyAsync("There is already a designated long task running" +
                                 "\nPlease wait for it to finish");
            } else {

                if (games >= 100000) {
                    await ReplyAsync("Too many games selected, I don't wanna play that much!");
                    return;
                }
                if (games >= 10000)
                    LongTask = true;
                if (games == 0)
                    games = 1;
                string output = "";
                if (games == 1) {
                    Blackjack bj = new();
                    Player winner = bj.PlayGame();
                    output = bj.Win();
                } else {
                    System.Timers.Timer Time = new(1000);
                    Time.Elapsed += Time_Elapsed;
                    int gamesplayed = games;
                    int[] output1 = new int[] { 0, 0 };

                    Time.Start();
                    for (int i = 0; i < games; i++) {
                        Blackjack game = new();
                        Player winner = game.PlayGame();
                        Console.WriteLine(i + "/" + games);
                        if (winner.IsDealer)
                            output1[0]++;
                        else
                            output1[1]++;
                    }
                    Time.Stop();

                    if (timerSeconds == 0)
                        timerSeconds = 1;

                    LongTask = false;
                    timerSeconds = 0;

                    output = $"In {gamesplayed} games" +
                         $"\nThe dealer won {output1[0]} times " +
                         $"\nThe player won {output1[1]} times" +
                         $"\nand took: {timerSeconds} Seconds!";
                }

                EmbedBuilder EmbedBuilder = new EmbedBuilder()
                    .WithTitle("Black Jack")
                    .WithDescription(output)
                    .WithCurrentTimestamp();
                Embed embed = EmbedBuilder.Build();

                await ReplyAsync(embed: embed);
            }
        }

        private void Time_Elapsed(object? sender, System.Timers.ElapsedEventArgs? e) {
            timerSeconds++;
        }
        #endregion BlackJack
        #region Payday
        [Command("pd2Help")]
        public async Task PD2Help() {
            EmbedBuilder EmbedBuilder = new EmbedBuilder()
.WithTitle($"Command prefix is '{BotProgram.PrefixChar}' (Commands are not case sensitive)")
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
            IEnumerable<Emoji> Reaction = BotProgram.Emojis.Except(new Emoji[]{ new("\u0039\u20E3") });
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
            await item.AddReactionsAsync(BotProgram.Emojis);
        }
        #endregion Call of Duty
    }
}