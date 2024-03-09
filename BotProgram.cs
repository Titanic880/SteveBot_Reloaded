using Discord.WebSocket;
using Discord.Commands;
using Discord;

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

using SteveBot_Rebuild.Modules;

namespace SteveBot_Rebuild {
    internal class BotProgram {
        public const char PrefixChar = '$';


        /// <summary>
        /// https://www.fileformat.info/info/emoji/list.htm
        /// </summary>
        public static Emoji[] emojis { get; } = new Emoji[] //using get for references
    {
            new("\u0031\u20E3"), //1
            new("\u0032\u20E3"), //2
            new("\u0033\u20E3"), //3
            new("\u0034\u20E3"), //4
            new("\u0035\u20E3"), //5
            new("\u0036\u20E3"), //6
            new("\u0037\u20E3"), //7
            new("\u0038\u20E3"), //8
            new("\u0039\u20E3"), //9
            new("\u2705")        //WhiteCheckMark
    };

        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public BotProgram() {
            //Allows bot to see messages https://discordnet.dev/guides/v2_v3_guide/v2_to_v3_guide.html
            var config = new DiscordSocketConfig()
            { GatewayIntents =
            GatewayIntents.Guilds |
            GatewayIntents.GuildMessageReactions |
            GatewayIntents.GuildMessages |
            GatewayIntents.GuildEmojis |
            GatewayIntents.MessageContent 
            };

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

        private async Task StartBot(string token) {
            await RegisterCommandsAsync();
            //logs the bot into discord
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            while (Tim_Elapsed()) {
                await Task.Delay(600000);
            }
        }

        private bool Tim_Elapsed() {
            if (_client.LoginState != LoginState.LoggedIn) {
                try {
                    _client.LogoutAsync();
                    _client.LoginAsync(TokenType.Bot, File.ReadAllText("Files/auth.json"));
                    Console.WriteLine($"Steve needed some coffee.");
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    CommandFunctions.ErrorMessages(ex.Message);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// To Fix hot unloading/loading while debugging
        /// </summary>
        ~BotProgram() {
            _client.LogoutAsync();
        }
        //outputs to Console
        private Task Client_Log(LogMessage arg) {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
        /// <summary>
        /// Adds commands to the bot
        /// </summary>
        /// <returns></returns>
        private async Task RegisterCommandsAsync() {
            //registers anything tagged as 'Task' to the set of commands that can be called
            _client.MessageReceived += HandleCommandAsync;
            _client.ReactionAdded += HandleReactionAdd;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }
        /// <summary>
        /// Primary Command Handler
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Task HandleCommandAsync(SocketMessage arg) {
            _ = Task.Run(async () => {
                int argPos = 0;
                //IGuildChannel bots;   //TODO: Implement channel restriction

                //Stores user message and initilizes message position
                if (arg is not SocketUserMessage message)
                    return;
                //checks to see if the user is a bot
                else
                    //Checks for prefix or specified passthrough commands
                    if (message.HasCharPrefix(PrefixChar, ref argPos)
                     || message.Content.ToLower() == "k") {
                    //Saves user Input to a debug file for later inspection
                    CommandFunctions.UserCommand(message);
                    if (message.Author.IsBot)
                        return;
                    //generates an object from the user message
                    SocketCommandContext context = new(_client, message);

                    //Attempts to run the command and outputs accordingly
                    IResult result = await _commands.ExecuteAsync(context, argPos, _services);
                    if (!result.IsSuccess) {
                        Console.WriteLine(result.ErrorReason);
                        await message.Channel.SendMessageAsync(result.ErrorReason);
                    }
                    if (result.Error.Equals(CommandError.UnmetPrecondition))
                        await message.Channel.SendMessageAsync(result.ErrorReason);
                }
            });
            return Task.CompletedTask;
        }

        /// <summary>
        /// Reaction Handler
        /// </summary>
        /// <param name="msgCache"></param>
        /// <param name="msgChannel"></param>
        /// <param name="reaction"></param>
        /// <returns></returns>
        private Task HandleReactionAdd(Cacheable<IUserMessage, ulong> msgCache, Cacheable<IMessageChannel, ulong> msgChannel, SocketReaction reaction) {
            return ReactionHandler.HandlerEntry(msgCache,reaction);
        }
    }
}