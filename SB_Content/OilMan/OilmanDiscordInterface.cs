using Discord;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace SB_Content.OilMan
{
    public static class OilmanDiscordInterface
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
        public static string CancelGame(IUser User)
        {
            foreach (GameState game in Games)
                if (game.GameHost == User)
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
                            return EmbedFactory($"Not enough players to start! (Minimum of: {GameState.MinimumPlayers})");
                        gameID = game.GameID;
                        //Check that all players have selected a color
                        foreach (Oilman_Player player in game.Players)
                            if (player.Player_Color == null)
                                return EmbedFactory("Player(s) has yet to select a color!");
                        break;
                    }
                }
                if (gameID == -1)
                    return EmbedFactory("User hosted game not found!");
                return await GameMapBuilder(Host);
            });
        }
        public static string LeaveGame(IUser User)
        {
            foreach (GameState game in Games)
                if (game.PlayerCheck(User) && game.GameHost != User)
                {
                    game.RemovePlayer(User);
                    return $"you have left {game.GameHost}'s game";
                }
                else if (game.GameHost == User)
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

            return EmbedFactory("Not implemented.");
        }

        /// <summary>
        /// Triggered by User, Clean up turn and output info
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public static Embed EndTurn(IUser User)
        {
            //Find 
            throw new NotImplementedException();
        }
        
        public static async Task<string> BuyLand(IUser User,string positional)
        {
            return await Task.Run(() =>
            {
                //make non case sensitive inputs
                positional = positional.ToLower();
                //Get gamestate
                GameState? game = null;
                foreach (GameState gs in Games)
                    if (gs.PlayerCheck(User))
                    {
                        if (gs.CurrentPlayer.User != User) return $"{User.Mention} It is not your turn!";
                        game = gs;
                        break;
                    }
                if (game == null) return "Active game was not found.";

                List<Tuple<int, int>> tilesToBuy = new();
                string[] QuardinentsToSolve = positional.Split(',');

                for (int i = 0; i < QuardinentsToSolve.Length; i++)
                {
                    if (QuardinentsToSolve[i].Contains(':'))
                    {
                        string[] RangeToSolve = QuardinentsToSolve[i].Split(':');
                        if (RangeToSolve.Length != 2) return "Invalid range has been provided.";

                        Tuple<int, int> StartPosition = QuardinentGenerator(RangeToSolve[0]);
                        Tuple<int, int> EndPosition = QuardinentGenerator(RangeToSolve[1]);
                        //Find all positions between the two
                        //Search across then down
                        for(int y = StartPosition.Item2; y < EndPosition.Item2; y++) 
                            for(int x = StartPosition.Item1; x < EndPosition.Item1; x++)
                                tilesToBuy.Add(Tuple.Create(x,y));  
                    }
                    else //Simple Position item
                    {
                        Tuple<int, int> Quardinent = QuardinentGenerator(QuardinentsToSolve[i]);
                        if (Quardinent.Item1 != -1)
                            tilesToBuy.Add(Quardinent);
                        //Return item TBD
                        else throw new NotImplementedException();
                    }
                }
                //check size of tilestoBuy (cannot exceed set limit)
                if(tilesToBuy.Count > GameState.MaximumTileBuy)
                {
                    return "Mis-matched Game Tiles.";
                }
                foreach(Tuple<int,int> tile in tilesToBuy)
                    if (game.CheckOwned(tile))
                    {
                        return "A tile was found to be already owned.";
                    }
                game.SetBuyTiles(tilesToBuy.ToArray());
                return $"You have set a bid for {positional}";
            });
            //Converts a-o<number> into positional data
            static Tuple<int,int> QuardinentGenerator(string quard)
            {
                if (!int.TryParse(quard.Skip(1).ToString(), out int y))
                    return Tuple.Create(-1,-1);
                return Tuple.Create(quard[0] switch
                {
                    'a' => 0,
                    'b' => 1,
                    'c' => 2,
                    'd' => 3,
                    'e' => 4,
                    'f' => 5,
                    'g' => 6,
                    'h' => 7,
                    'i' => 8,
                    'j' => 9,
                    'k' => 10,
                    'l' => 11,
                    'm' => 12,
                    'n' => 13,
                    'o' => 14,
                    _ => -1,
                }, y);
            }
        }
        public static Embed ClaimLand(IUser User)
        {
            GameState? CurrentGame = UserInGame(User);
            if (CurrentGame == null) return EmbedFactory("Game not found", "Claim Failed");
            if (CurrentGame.PlayerClaim(User))
            {
                if (CurrentGame.CurrentPlayer.User != User)
                    return EmbedFactory($"{User} has stolen the claim from {CurrentGame.CurrentPlayer.User}!","Claim Stolen!");
                return EmbedFactory($"{User} has claimed the active Territory!","Territory Claimed");
            }
            return EmbedFactory("Attempted to Double claim Territory.","Error Claiming");
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
        public static async Task<bool> ReactionLayoutSelected(IUser User, int layout)
        {
            if (Games.Count == 0 || layout > 3 || layout < 0) return false;

            GameState? CurrentGame = UserInGame(User);
            if (CurrentGame == null) return false;
            if(CurrentGame.GameHost == User) return await CurrentGame.SetLayout(layout);
            return false;
        }

        public static async Task<Tuple<bool, string>> PlayerColorReaction(IUser User, Emoji color)
        {
            return await Task.Run(() =>
            {
                GameState? CurrentGame = UserInGame(User);
                if (CurrentGame == null) return Tuple.Create(false, "Game/Player was not Found.");
                //Check that color is not already selected
                if (CurrentGame.Players.Where(x => x.Player_Color == Player_Colors.First(x => x.Item2 == color)).ToArray().Length != 0)
                    return Tuple.Create(false, "Color has already been selected");
                //Add color to user
                CurrentGame.Players.Where(x => x.User == User).First().SetColors(Player_Colors.First(x => x.Item2 == color));
                return Tuple.Create(true, $"{User.Username} color has been set");
            });
        }
        #endregion Game Reaction input   
        #region Game output
        /// <summary>
        /// Returns Map as Generic object
        /// </summary>
        /// <param name="GameID"></param>
        /// <returns></returns>
        public static async Task<Embed> GameMapBuilder(IUser User)
        {
            GameState? CurrentGame = UserInGame(User);
            if(CurrentGame == null) return EmbedFactory("Game not created");
            return await Task.Run(() =>
            {
                string Rows = "";
                for (int y = 1; y <= CurrentGame.GameMap.Length; y++)
                {
                    Rows += "\n";
                    //Removed due to description limit
                    //Rows += $"\n`{(y < 10 ? "0" :"") }{y}`"; 
                    for(int x = 0; x < CurrentGame.GameMap[y-1].Length; x++)
                        Rows += CurrentGame.GameMap[y-1][x].ToString();
                }
                return EmbedFactory(Rows,Title: $"Oilman Game #{CurrentGame.GameID}");
            });
        }
        /// <summary>
        /// Returns map with Owner colors/unowned (no well depths)
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public static async Task<Embed> OwnedTilesMapBuilder()
        {
            throw new NotImplementedException();
        }
        #region User Interaction


        public static Embed UserBid(IUser User, int currentbid)
        {
            string EmbedDesc = "Game Not Found.";
            GameState? CurrentGame = UserInGame(User);
            if (CurrentGame != null)
                EmbedDesc = GameMapFactory(CurrentGame);
            return EmbedFactory(EmbedDesc);
        }
        #endregion User Interaction
        #endregion Game output

        #region Factories
        internal static Embed EmbedFactory(string Description,string Title = "" ,string Footer = "")
        {
            if(Footer == "" && Title == "")
                return new EmbedBuilder().WithCurrentTimestamp().WithDescription(Description).Build();
            if(Title == "" && Footer != "")
                return new EmbedBuilder().WithFooter(Footer).WithDescription(Description).WithCurrentTimestamp().Build();
            if(Title != "" && Footer == "")
                return new EmbedBuilder().WithTitle(Title).WithDescription(Description).WithCurrentTimestamp().Build();
            return new EmbedBuilder().WithTitle(Title).WithDescription(Description).WithFooter(Footer).WithCurrentTimestamp().Build();
        }
        internal static string GameMapFactory(GameState CurrentGame)
        {
            GameTile[][] GameBoard = CurrentGame.GameMap;
            string Rows = "";
            for (int y = 1; y <= GameBoard.Length; y++)
            {
                Rows += "\n";
                for (int x = 0; x < GameBoard[y - 1].Length; x++)
                {
                    Rows += GameBoard[y - 1][x].ToString();
                }
            }

            return Rows;
        }
        private static GameState? UserInGame(IUser User)
        {
            foreach (GameState game in Games)
                if (game.PlayerCheck(User))
                    return game;
            return null;
        }
        public static Embed BuildColorSelector(IUser User)
        {
            GameState? userGame = UserInGame(User);
            string embedDescription = "";
            //Sets up default embed
            if (userGame == null)
            {
                for (int i = 0; i < Player_Colors.Length; i++)
                    embedDescription += $"{Player_Colors[i].Item1} ; {Player_Colors[i].Item1}\n";
                return EmbedFactory(embedDescription, Footer: "Color Selection");
            }
            //Build Game specific Colors
            for (int i = 0; i < Player_Colors.Length; i++)
            {
                bool ColorIsClaimed = false;
                foreach (Oilman_Player pl in userGame.Players)
                    if (pl.Player_Color == Player_Colors[i])
                    {
                        embedDescription += $"{pl.User}: {Player_Colors[i].Item1} ; {Player_Colors[i].Item1}\n";
                        ColorIsClaimed = true;
                        continue;
                    }
                if (!ColorIsClaimed)
                    embedDescription += $"unclaimed: {Player_Colors[i].Item1} ; {Player_Colors[i].Item1}\n";
            }
            return EmbedFactory(embedDescription, $"Oilman Game: {userGame.GameID}", "Color Selector");
        }
        public static Emoji[] GetColorReaction()
        {
            Emoji[] items = new Emoji[Player_Colors.Length];
            for (int i = 0; i < Player_Colors.Length; i++)
                items[i] = Player_Colors[i].Item2;
            return items;
        }

        /// <summary>
        /// Outputs the Emoji Legend of what tiles are
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static Embed BuildTileLegend()
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
        #endregion Factories
    }
}
