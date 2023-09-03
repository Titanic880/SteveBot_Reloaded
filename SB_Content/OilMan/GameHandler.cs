using Discord.WebSocket;
using Discord;

using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using SB_Content.Runescape;

namespace SB_Content.OilMan
{
    /// <summary>
    /// Highest level of Oilman Based Objects
    /// </summary>
    public static class GameHandler
    {
        private static readonly List<GameState> Games = new();
        internal static readonly Emoji[] GameAssets = { 
            new(":x:"), new(":white_check_mark:"),        //Deny/Confirm items?
            new(":white_large_square:"), new(":white_circle:"), //Water : Ground
            new(":white_small_square:"),        //Shallow Drill
            new(":white_medium_small_square:"), //Medium Drill
            new(":white_medium_square:"),       //Depp Drill
            new(":black_large_square:"),new(":black_circle:") //Bidding
        };
        //Tuples of Ground/Water Player Color claims (How to indicate wells?)
        private static Tuple<Emoji, Emoji>[] Player_Colors = { 
            Tuple.Create(new Emoji(":brown_circle:"),new Emoji(":brown_square:")),
            Tuple.Create(new Emoji(":yellow_circle:"),new Emoji(":yellow_square:")),
            Tuple.Create(new Emoji(":orange_circle:"),new Emoji(":orange_square:")),
            Tuple.Create(new Emoji(":red_circle:"),new Emoji(":red_square:")),
            Tuple.Create(new Emoji(":green_circle:"),new Emoji(":green_square:")),
            Tuple.Create(new Emoji(":purple_circle:"),new Emoji(":purple_square:")),
            Tuple.Create(new Emoji(":blue_circle:"),new Emoji(":blue_square:"))
        };

        /// <summary>
        /// Takes 1:1 Throw from Reaction Added (TO BE TRIMMED)
        /// </summary>
        /// <param name="msgCache"></param>
        /// <param name="msgChannel"></param>
        /// <param name="reaction"></param>
        public static Tuple<bool, string> ReactionThrow(Cacheable<IUserMessage, ulong> msgCache, Cacheable<IMessageChannel, ulong> msgChannel, SocketReaction reaction)
        {
            throw new NotImplementedException();
        }

        public async static Task<Tuple<bool,string>> InitilizeNewGame(IGuildUser Host)
        {
            return await Task.Run(() =>
            {
                if (Games.FirstOrDefault(x => x.GameHost == Host) != null)
                    return Tuple.Create(false, "You are already the host of a game!");
                Games.Add(new GameState(Host, Games.Count + 1));
                return Tuple.Create(true, $"Game has been created with an ID of {Games.Count}");
            });
        }
        public static string CancelGame(IGuildUser user)
        {
            foreach (GameState game in Games)
                if (game.GameHost == user)
                {
                    if (game.GameActive)
                        return "Unable to cancel a started game!";
                    Games.Remove(game);
                    return "Game has been cancelled";
                }
            return "Game was not found";
        }
        public async static Task<Embed> StartGame(IGuildUser Host)
        {
            return await Task.Run(async () =>
            {
                int gameID = -1;
                //Find Game
                foreach(GameState game in Games)
                    if(game.GameHost == Host)
                    {
                        if (!game.StartGame()) 
                            return new EmbedBuilder().WithDescription($"Not enough players to start! (Minimum of: {GameState.MinimumPlayers})").Build();
                        gameID = game.GameID;
                        break;
                    }
                if (gameID == -1)
                    return new EmbedBuilder().WithDescription("User hosted game not found!").Build();

                
                return await MapBuilder(gameID);
            });
        }
        public static async Task<bool> ReactionLayoutSelected(int layout, int gameID)
        {
            //Safe Guards
            if (layout > 3 || layout < 0) return false;
            if (gameID > Games.Count && Games.Count != 0) return false;
            return await Games[gameID - 1].SetLayout(layout);
        }

        public static Tuple<bool,string> ReactionInteract()
        {
            throw new NotImplementedException();
        }

        public static void StartTurn()
        {

        }

        /// <summary>
        /// Triggered by user, Clean up turn and output info
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Embed EndTurn(IUser user)
        {
            //Find Game user is in
            foreach(GameState gs in Games)
            {
                if (gs.PlayerCheck(user))
                {
                    //gs.CurrentPlayer.User == user;
                }
            }
            throw new NotImplementedException();
        }
        /// <summary>
        /// Ends game and returns results dialog
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        internal static Embed EndGame(IGuildUser Host, int Id)
        {
            throw new NotImplementedException();
        }
        
        public static async Task<Embed> MapBuilder(int GameID)
        {
            return await Task.Run(() =>
            {
                GameTile[][] Map = Games[GameID - 1].GetMap();
                string Rows = "";
                Rows = "`-` a:";
                Rows = Rows.Replace(" ", " :regional_indicator_");
                Rows = Rows.Replace("-"," ");
                for (int y = 1; y <= Map.Length; y++)
                {
                    Rows += $"\n`{(y < 10 ? "0" :"") }{y}`";
                    for(int x = 0; x < Map[y-1].Length; x++)
                        Rows += Map[y-1][x].ToString();
                }

                EmbedBuilder emb = new EmbedBuilder()
                .WithTitle($"Oilman Game #{GameID}")
                .WithDescription(Rows)
                .WithCurrentTimestamp();
                return emb.Build();
            });
        }
        /// <summary>
        /// Outputs the Emoji Legend of what tiles are
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static Embed BuildLegend()
        {
            //Build Map
            EmbedBuilder builder = new EmbedBuilder()
            .WithTitle("Oilman Icon Legend")
            .WithDescription($"{GameAssets[2]}: Land Tile"
                + $"\n{GameAssets[3]} Water Tile"
                + $"\n{GameAssets[7]} Land Tile for purchase"
                + $"\n{GameAssets[8]} Water Tile for purchase"
                + $"\n{GameAssets[0]} Zero oil Found Tile"
                + $"\n{GameAssets[4]} Shallow Well"
                + $"\n{GameAssets[5]} Medium Depth Well"
                + $"\n {GameAssets[6]} Deep Well"
                ).WithCurrentTimestamp();
            Embed emb = builder.Build();
            return emb;
        }
    }
}
