using Discord;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace SB_Content.OilMan
{
    /// <summary>
    /// Highest level of Oilman Based Objects
    /// </summary>
    public static class GameHandler
    {
        private static readonly List<GameState> Games = new();

        #region Game Assets
        internal static readonly Emoji[] GameAssets = { 
            new(":x:"), new(":white_check_mark:"),        //Deny/Confirm items?
            new(":brown_circle:"),new(":brown_square:"), //Water : Ground
            new(":white_small_square:"),        //Shallow Drill
            new(":white_medium_small_square:"), //Medium Drill
            new(":white_medium_square:"),       //Depp Drill
            new(":black_large_square:"),new(":black_circle:") //Bidding
        };
        //Tuples of Ground/Water Player Color claims (How to indicate wells?)
        private static readonly Tuple<Emoji, Emoji>[] Player_Colors = { 
            Tuple.Create(new Emoji(":white_large_square:"), new Emoji(":white_circle:")),
            Tuple.Create(new Emoji(":yellow_circle:"),new Emoji(":yellow_square:")),
            Tuple.Create(new Emoji(":orange_circle:"),new Emoji(":orange_square:")),
            Tuple.Create(new Emoji(":red_circle:"),new Emoji(":red_square:")),
            Tuple.Create(new Emoji(":green_circle:"),new Emoji(":green_square:")),
            Tuple.Create(new Emoji(":purple_circle:"),new Emoji(":purple_square:")),
            Tuple.Create(new Emoji(":blue_circle:"),new Emoji(":blue_square:"))
        };
        #endregion Game Assets

        #region Game Command input
        #region Game State Actions
        public async static Task<Tuple<bool,string>> InitilizeNewGame(IUser Host)
        {
            return await Task.Run(() =>
            {
                if (Games.FirstOrDefault(x => x.GameHost == Host) != null)
                    return Tuple.Create(false, "You are already the host of a game!");
                GameState tmp = new(Host, Games.Count);
                Games.Add(tmp);
                return Tuple.Create(true, $"Game has been created with an ID of {tmp.GameID}");
            });
        }
        public static string CancelGame(IUser user)
        {
            foreach (GameState game in Games)
                if (game.GameHost == user)
                {
                    Games.Remove(game);
                    return "Game has been cancelled";
                }
            return "Game was not found";
        }
        public async static Task<Embed> StartGame(IUser Host)
        {
            return await Task.Run(async () =>
            {
                int gameID = -1;
                //Find Game
                foreach (GameState game in Games)
                {
                    //Find Host
                    if (game.GameHost == Host)
                    {
                        if (!game.StartGame())
                            return new EmbedBuilder().WithDescription($"Not enough players to start! (Minimum of: {GameState.MinimumPlayers})").Build();
                        gameID = game.GameID;
                        //Check that all players have selected a color
                        foreach (Oilman_Player player in game.Players)
                            if (player.Player_Color == null)
                                return new EmbedBuilder().WithDescription("Player(s) has yet to select a color!").Build();
                        break;
                    }
                }
                if (gameID == -1)
                    return new EmbedBuilder().WithDescription("User hosted game not found!").Build();
                return await MapBuilder(gameID);
            });
        }
        public static string LeaveGame(IUser user)
        {
            foreach (GameState game in Games)
                if (game.PlayerCheck(user) && game.GameHost != user)
                {
                    game.RemovePlayer(user);
                    return $"you have left {game.GameHost}'s game";
                }
                else if (game.GameHost == user)
                    return "Please use Cancel Game command as host";
            return "You were not found in a game";
        }
        /// <summary>
        /// Ends game and returns results dialog
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        internal static Embed EndGame(IUser Host, int Id)
        {
            throw new NotImplementedException();
        }
        #endregion Game State Actions
        #region Turn Based Actions
        public static async Task<Embed> StartTurn()
        {

            return new EmbedBuilder().WithDescription("Not implemented.").Build();
        }

        /// <summary>
        /// Triggered by user, Clean up turn and output info
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Embed EndTurn(IUser user)
        {
            //Find 
            throw new NotImplementedException();
        }
            #endregion Turn Based Actions
        #endregion Game Command input
        #region Game Reaction input
        /// <summary>
        /// Uses gameID as its passed via reaction 
        /// (Setup so that it passes Reaction IUser?)
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="gameID"></param>
        /// <returns></returns>
        public static async Task<bool> ReactionLayoutSelected(int layout, int gameID)
        {
            if (Games == null) return false;
            //Safe Guards
            if (layout > 3 || layout < 0) return false;
            if (gameID > Games.Count && Games.Count != 0) return false;
            return await Games[gameID-1].SetLayout(layout);
        }

        public static Tuple<bool,string> ReactionInteract()
        {
            throw new NotImplementedException();
        }
        #endregion Game Reaction input
       
        #region Game output
        /// <summary>
        /// Returns Map as Generic object
        /// </summary>
        /// <param name="GameID"></param>
        /// <returns></returns>
        public static async Task<Embed> MapBuilder(int GameID)
        {
            if(Games == null) return new EmbedBuilder().WithDescription("Game not created").Build();
            return await Task.Run(() =>
            {
                GameTile[][] Map = Games[GameID - 1].GetMap();
                string Rows = "";
                for (int y = 1; y <= Map.Length; y++)
                {
                    Rows += "\n";
                    //Removed due to description limit
                    //Rows += $"\n`{(y < 10 ? "0" :"") }{y}`"; 
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
        /// Returns map with Owner colors/unowned (no well depths)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task<Embed> MapBuilder(IUser user)
        {
            throw new NotImplementedException();
        }
        #region User Help
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
        /// <summary>
        /// Returns a Color pallete Embed of current colors
        /// </summary>
        /// <returns></returns>
        public static Embed BuildColorSelector(IUser User)
        {
            //Check if user is part of a game
            GameState? userGame = Games.Find(x => x.PlayerCheck(User));
            string embdesc = "";
            //Sets up default embed
            EmbedBuilder DefaultEmbed = new EmbedBuilder()
                .WithTitle("Oilman Game").WithCurrentTimestamp().WithFooter("Color Selector");
            if (userGame == null)
            {
                for (int i = 0; i < Player_Colors.Length; i++)
                    embdesc += $"{Player_Colors[i].Item1} ; {Player_Colors[i].Item1}\n";
                return DefaultEmbed.WithDescription(embdesc).Build();
            }

            DefaultEmbed = DefaultEmbed.WithTitle($"Oilman Game: {userGame.GameID}");
            embdesc = "";
            int len = 0;
            //Build Game specific Colors
            for (int i = 0; i < Player_Colors.Length; i++)
            {
                len = embdesc.Length;
               foreach(Oilman_Player pl in userGame.Players)
                    if (pl.Player_Color == Player_Colors[i])
                    {
                        embdesc += $"{pl.User}: {Player_Colors[i].Item1} ; {Player_Colors[i].Item1}\n";
                        break;
                    }
               if(len == embdesc.Length)
                    embdesc += $"unclaimed: {Player_Colors[i].Item1} ; {Player_Colors[i].Item1}\n";
            }
            return DefaultEmbed.WithDescription(embdesc).Build();
        }
        public static Emoji[] GetColorReaction()
        {
            Emoji[] items = new Emoji[Player_Colors.Length];
            for (int i = 0; i < Player_Colors.Length; i++)
                items[i] = Player_Colors[i].Item2;
            return items;
        }
        #endregion User Help
        #endregion Game output
    }
}
