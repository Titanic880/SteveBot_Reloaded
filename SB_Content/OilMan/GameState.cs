using System.Collections.Generic;
using Discord;
using System.Linq;
using System;
using System.Transactions;
using System.Threading.Tasks;

namespace SB_Content.OilMan
{
    internal class GameState
    {
        public readonly int GameID;
        #region Constant Information
        internal const int MinimumPlayers = 2;
        internal const int MaximumPlayers = 7;
        internal const int MaxTurns = 500;
        #endregion Constant Information
        #region Runtime Information

        public bool GameActive { get; private set; } = false;
        private readonly GameBoard Board;
        #endregion Runtime Information
        internal IUser GameHost;

        internal List<Oilman_Player> Players { get; set; } = new();
        internal Oilman_Player CurrentPlayer { get => Players[CP]; }
        internal int CurrentTurn { get; private set; }
        private int CP;

        public GameState(IUser Host,int GameID)
        {
            this.GameID = GameID;
            GameHost = Host;
            Board = new();
        }
        internal bool StartGame()
        {
            if(Players.Count < MinimumPlayers) return false;
            GameActive = true;
            CurrentTurn = 0;
            CP = 0;
            return true;
        }
        /// <summary>
        /// Updates Main Interactions
        /// </summary>
        private void Update()
        {

        }

        /// <summary>
        /// Updates start of each turn
        /// </summary>
        private void UpdatePlayer()
        {
            Players[CP].StartTurnUpdate();
            
            
        }

        /// <summary>
        /// Updates end of each turn
        /// </summary>
        private void EndPlayerTurn()
        {
            Players[CP].EndTurnUpdate();
            if (Players[CP] == Players[^1])
                CP = 0;
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
        internal async Task<bool> SetLayout(int layout)
            => await Board.GenerateMap(layout);
        internal GameTile[][] GetMap() 
            => Board.GameMap;
        
    }
}
