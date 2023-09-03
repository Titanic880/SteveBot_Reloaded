using System.Collections.Generic;
using Discord;
using System.Linq;
using System;

namespace SB_Content.OilMan
{
    internal class GameState
    {
        public readonly int GameID;
        #region Constant Information
        private const int MinimumPlayers = 2;
        private const int MaximumPlayers = 7;
        private const int MaxTurns = 500;
        #endregion Constant Information
        #region Runtime Information

        private bool GameActive = false;
        private readonly GameBoard Board;
        #endregion Runtime Information
        internal IGuildUser GameHost;
        internal List<Oilman_Player> Players { get; set; } = new();
        internal int CurrentTurn { get; private set; }


        public GameState(IGuildUser Host,int GameID)
        {
            GameHost = Host;
            this.GameID = GameID;
            Board = new();
        }
        internal void StartGame()
        {
            GameActive = true;
            CurrentTurn = 0;
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
    }
}
