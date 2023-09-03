using Discord.WebSocket;
using Discord.Commands;
using Discord;

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Timers;

using SteveBot_Rebuild.Modules;

namespace SteveBot_Rebuild
{
    internal class BotProgram
    {
        public const char PrefixChar = '$';
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

        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public BotProgram()
        {
            //Allows bot to see messages https://discordnet.dev/guides/v2_v3_guide/v2_to_v3_guide.html
            var config = new DiscordSocketConfig()
            { GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent };

            _client = new DiscordSocketClient(config);
            _commands = new CommandService();


            //Not 100% sure what this does in its entirity; .AddSingleton == static
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            _client.Log += Client_Log;
            StartBot(File.ReadAllText("Files/auth.json")).GetAwaiter().GetResult();
        }

        private async Task StartBot(string token)
        {
            await RegisterCommandsAsync();
            //logs the bot into discord
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            System.Timers.Timer tim = new();
            tim.Elapsed += Tim_Elapsed;
            tim.Interval = 300000;
            tim.Start();
            await Task.Delay(Timeout.Infinite);
            Console.WriteLine("zzz");
        }

        private void Tim_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (_client.LoginState != LoginState.LoggedIn)
            {
                try
                {
                    _client.LogoutAsync();
                    _client.LoginAsync(TokenType.Bot, File.ReadAllText("Files/auth.json"));
                    Console.WriteLine($"Steve needed some coffee.");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    CommandFunctions.ErrorMessages(ex.Message);
                }
            }
            //else Console.Write(".");
        }

        /// <summary>
        /// To Fix hot unloading/loading while debugging
        /// </summary>
        ~BotProgram()
        {
            _client.LogoutAsync();
        }
        //outputs to Console
        private Task Client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
        //Adds commands to the bot
        public async Task RegisterCommandsAsync()
        {
            //registers anything tagged as 'Task' to the set of commands that can be called
            _client.MessageReceived += HandleCommandAsync;
            _client.ReactionAdded += HandleReactionAdd;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private Task HandleReactionAdd(Cacheable<IUserMessage, ulong> msgCache, Cacheable<IMessageChannel, ulong> msgChannel, SocketReaction reaction)
        {
            _ = Task.Run(async () =>
            {
                if (reaction.User.Value.IsBot) return;
                IUserMessage? message = await msgCache.GetOrDownloadAsync();
                //var debug = message.Reactions;
                
                //In switch items
                Emoji[] emoj;
                IEmote checkmarkemote;
                ReactionMetadata data;
                bool[] rand;

                switch (message.Embeds.FirstOrDefault()!.Title.Split(':')[0])
                {
                    case "Payday 2 Randomizer":
                        emoj = new Emoji[8];
                        Array.Copy(emojis, emoj, 8);
                        rand = new bool[8];
                        checkmarkemote = message.Reactions.Keys.FirstOrDefault(x => x.Name == "✅")!;
                        data = message.Reactions[checkmarkemote];
                        if (checkmarkemote.Name == "✅" && data.IsMe && data.ReactionCount > 1)
                        {
                            for (int i = 0; i < emoj.Length; i++)
                                if (message.Reactions[emoj[i]].ReactionCount > 1)
                                    rand[i] = true;
                            SB_Content.Payday.Randomizer.PD2DataFile pd2data = new();
                            pd2data.Randomize(rand);
                            EmbedBuilder builder = new EmbedBuilder()
                            .WithTitle("Payday 2 Randomizer Results")
                            .WithTimestamp(DateTime.UtcNow)
                            .WithDescription(pd2data.GetResult());
                            await reaction.Channel.SendMessageAsync($"<@{reaction.UserId}> ", embed: builder.Build());
                        }
                        break;
                    case "COD Cold War Randomizer":
                        emoj = new Emoji[9];
                        Array.Copy(emojis, emoj, 9);
                        rand = new bool[9];
                        checkmarkemote = message.Reactions.Keys.FirstOrDefault(x => x.Name == "✅")!;
                        data = message.Reactions[checkmarkemote];
                        if (checkmarkemote.Name == "✅" && data.IsMe && data.ReactionCount > 1)
                        {
                            for (int i = 0; i < emoj.Length; i++)
                                if (message.Reactions[emoj[i]].ReactionCount > 1)
                                    rand[i] = true;
                            SB_Content.Call_of_Duty.Randomizer.ZombRandLib randlib = new();
                            randlib.ApplyOptions(rand);
                            randlib.Randomize();

                            EmbedBuilder builder = new EmbedBuilder()
                            .WithTitle("Cold War Randomizer Results")
                            .WithTimestamp(DateTime.UtcNow)
                            .WithDescription(randlib.GetResult());
                            await reaction.Channel.SendMessageAsync($"<@{reaction.UserId}> ", embed: builder.Build());
                        }
                        break;
                    case "Oilman Game":
                        if (message.Embeds.FirstOrDefault()!.Footer.ToString() == "Start Info")
                        {
                            Emoji[] reac = new Emoji[] { ":one:", ":two:", ":three:" };
                            int throwup = 0;
                            if (message.Reactions[reac[0]].ReactionCount != 1)
                                throwup = 1;
                            else if (message.Reactions[reac[1]].ReactionCount != 1)
                                throwup = 2;
                            else if (message.Reactions[reac[2]].ReactionCount != 1)
                                throwup = 3;
                            if (await SB_Content.OilMan.GameHandler.ReactionLayoutSelected(throwup, Convert.ToInt32(message.Embeds.FirstOrDefault()!.Title.Split(':')[1])))
                                await message.AddReactionAsync(new Emoji("✅"));
                            else await message.AddReactionAsync(new Emoji(":x:"));
                        }
                        break;
                    default:
                        break;
                }
            });
            return Task.CompletedTask;
        }

        //Command Handler
        private Task HandleCommandAsync(SocketMessage arg)
        {
            _ = Task.Run(async () =>
            {
                int argPos = 0;
                //IGuildChannel bots;   //TODO: Implement channel restriction

                //Stores user message and initilizes message position
                if (arg is not SocketUserMessage message)
                    return;
                //checks to see if the user is a bot
                else
                    //Checks for prefix or specified passthrough commands
                    if (message.HasCharPrefix(PrefixChar, ref argPos)
                     || message.Content.ToLower() == "k")
                {
                    //Saves user Input to a debug file for later inspection
                    CommandFunctions.UserCommand(message);
                    if (message.Author.IsBot)
                        return;
                    //generates an object from the user message
                    SocketCommandContext context = new(_client, message);
                    
                    //Attempts to run the command and outputs accordingly
                    IResult result = await _commands.ExecuteAsync(context, argPos, _services);
                    if (!result.IsSuccess)
                    {
                        Console.WriteLine(result.ErrorReason);
                        await message.Channel.SendMessageAsync(result.ErrorReason);
                    }
                    if (result.Error.Equals(CommandError.UnmetPrecondition))
                        await message.Channel.SendMessageAsync(result.ErrorReason);
                }
            });
            return Task.CompletedTask;
        }
    }
}
