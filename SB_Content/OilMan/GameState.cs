using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Discord;
using System;

namespace SB_Content.OilMan
{
    public class GameState
    {
        #region Constant Information
        internal const int MinimumPlayers = 2;
        internal const int MaximumPlayers = 7;
        internal const int MaximumTurns = 500;
        internal const int MaximumTileBuy = 9;
        private const int GameWidth = 15;
        private const int GameHeight = 12;
        #endregion Constant Information
        #region Runtime Information
        public readonly int GameID;
        public bool GameActive { get; private set; } = false;
        private int CP = -1;

        internal Oilman_Player GameHost { get; private set; }
        internal Oilman_Player CurrentPlayer { get => Players[CP]; }
        internal List<Oilman_Player> Players { get; set; } = new();
        public bool IsBuyCycle { get => BuySave.OwnedTiles.Count > 0; }
        public int CurrentTurn { get; private set; }
        private readonly Oilman_Player BuySave;
        internal GameTile[][] GameMap { get; private set; } = Array.Empty<GameTile[]>();
        //               y:x
        #endregion Runtime Information
       
        #region GameMap Handling
        private async Task<bool> GenerateMap(int Type)
        {
            return await Task.Run(() =>
            {
                if (Type == 1) { }
                else if (Type == 2) { }
                try
                {
                    GameMap = new GameTile[GameHeight][];
                    //RANDOM GENERATION
                    int tileID = 0;
                    for (int y = 0; y < GameHeight; y++)
                    {
                        GameMap[y] = new GameTile[GameWidth];
                        for (int x = 0; x < GameWidth; x++)
                            GameMap[y][x] = new GameTile(tileID++);
                    }
                    return true;
                }
                catch //(Exception ex)
                {
                    return false;
                }
            });
        }
        private Tuple<bool, string> DrillTile(Drilling_Types _Drill, Tuple<int, int> Tile)
            => GameMap[Tile.Item1][Tile.Item2].Drill(_Drill);
        #endregion GameMap Handling
        #region Land Buying
        private void SetBuyState(Tuple<int, int>[] tiles)
        {
            foreach (Tuple<int, int> Tile in tiles)
                GameMap[Tile.Item1][Tile.Item2].SetOwner(BuySave);
        }
        private bool ClaimLand(Oilman_Player _player)
        {
            foreach (GameTile tile in BuySave.OwnedTiles)
            {
                if (!tile.SetOwner(_player)) return false;
                _player.OwnedTiles.Add(tile);
            }
            return true;
        }
        /// <summary>
        /// returns if a specified tile is owned
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        internal bool CheckOwned(Tuple<int, int> tile)
        {
            return GameMap[tile.Item1][tile.Item2].Owner is not null;
        }
        internal void SetBuyTiles(Tuple<int, int>[] tiles)
        {
            SetBuyState(tiles);
        }
        /// <summary>
        /// Claims the currently contested region
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal bool PlayerClaim(IUser user)
        {
            return ClaimLand(Players.Where(x => x.User == user).First());
        }
        internal Tuple<string, int>[] PlayerBid(int Amount)
        {

            throw new NotImplementedException();
        }
        #endregion Land Buying
        public GameState(IUser Host,int GameID)
        {
            this.GameID = GameID;
            GameHost = new Oilman_Player(Host,GameID);
            BuySave = new(new Blank_User(), GameID);
            Players.Add(GameHost);
        }
        internal bool StartGame()
        {
            if(Players.Count < MinimumPlayers) 
                return false;

            GameActive = true;
            CurrentTurn = 0;
            CP = 0;
            GameUpdate();
            return true;
        }
        private void GameUpdate()
        {
            Players[CP].GameUpdate();
        }
        public void EndTurn()
        {
            GameUpdate();
            if (Players[CP] == Players[^1])
                CP = 0;
            else
                CP++;
        }

        /// <summary>
        /// Checks if specific user is in the game
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal bool PlayerCheck(IUser user)
        {
            foreach (Oilman_Player player in Players)
                if (player.User == user) return true;
            return false;
        }
        internal bool AddPlayer(IUser User)
        {
            if(User.IsBot || Players.Contains(Players.First(x=>x.User == User)) || Players.Count >= MaximumPlayers)
                return false;
            Players.Add(new Oilman_Player(User, GameID));
            return true;
        }
        internal bool RemovePlayer(IUser User)
        {
            if (GameActive) return false;
            Players.Remove(Players.First(x=> x.User == User));
            return true;
        }
        internal async Task<bool> SetLayout(int layout) => await GenerateMap(layout);
    }
}
