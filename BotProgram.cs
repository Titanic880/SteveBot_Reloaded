using Discord.WebSocket;
using Discord.Commands;
using Discord;

using Microsoft.Extensions.DependencyInjection;
using CommandModule;
using SteveBot_Rebuild.Modules;
using System.Reflection;

namespace SteveBot_Rebuild {
    internal class BotProgram {
       // public const char PrefixChar = '$';


        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public BotProgram() {
            //Allows bot to see messages https://discordnet.dev/guides/v2_v3_guide/v2_to_v3_guide.html
            DiscordSocketConfig config = new()
            {
                GatewayIntents =
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
            await Task.Delay(-1);
        }
        /*
        private bool Tim_Elapsed() {
            if (_client.LoginState != LoginState.LoggedIn) {
                try {
                    _client.LogoutAsync();
                    _client.LoginAsync(TokenType.Bot, File.ReadAllText("Files/auth.json"));
                    Console.WriteLine($"Steve needed some coffee.");
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    CommandFunctions.LogError(ex.Message);
                    return false;
                }
            }
            return true;
        }*/
        private async Task Tim_Elapsed(Exception exception) {
            if (_client.LoginState != LoginState.LoggedIn) {
                try {
                    await _client.LogoutAsync();
                    await _client.LoginAsync(TokenType.Bot, File.ReadAllText("Files/auth.json"));
                    Console.WriteLine($"Steve needed some coffee.");
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    CommandFunctions.LogError(ex.Message);
                }
            }
            return;
        }

        ~BotProgram() {
            _client.LogoutAsync();
        }
        //outputs to Console
        private Task Client_Log(LogMessage arg) {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
        private async Task RegisterCommandsAsync() {
            //registers anything tagged as 'Task' to the set of commands that can be called
            _client.MessageReceived += HandleCommandAsync;
            _client.ReactionAdded += HandleReactionAdd;
            _client.Disconnected += Tim_Elapsed;

            await _commands.AddModulesAsync(Assembly.LoadFile("/app/bin/Debug/net8.0/CommandModule.dll"),_services);
        }

        private Task HandleCommandAsync(SocketMessage arg) {
            _ = Task.Run(async () => {
                int argPos = 0;
                //IGuildChannel bots;   //TODO: Implement channel restriction
                if (arg is not SocketUserMessage message || message.Author.IsBot) {
                    return;
                }
                if (!message.HasCharPrefix(Command_Identifiers.Command_Prefix, ref argPos)) {
                    return;
                }
                CommandFunctions.Log_UserCommand(message);
                SocketCommandContext context = new(_client, message);
                IResult result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) {
                    CommandFunctions.LogError(result.ErrorReason);
                    await message.Channel.SendMessageAsync(result.ErrorReason);
                }
            });
            return Task.CompletedTask;
        }
        private Task HandleReactionAdd(Cacheable<IUserMessage, ulong> msgCache, Cacheable<IMessageChannel, ulong> msgChannel, SocketReaction reaction) {
            return ReactionHandler.HandlerEntry(msgCache,reaction);
        }
    }
}