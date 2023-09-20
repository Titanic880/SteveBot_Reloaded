﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Discord;
using System;

namespace SB_Content.OilMan
{
    public class GameState
    {
        public readonly int GameID;
        #region Constant Information
        internal const int MinimumPlayers = 2;
        internal const int MaximumPlayers = 7;
        internal const int MaximumTurns = 500;
        internal const int MaximumTileBuy = 9;
        #endregion Constant Information
        #region Runtime Information
        public bool GameActive { get; private set; } = false;
        private readonly GameBoard Board;
        #endregion Runtime Information
        internal IUser GameHost { get; private set; }

        internal List<Oilman_Player> Players { get; set; } = new();
        internal Oilman_Player CurrentPlayer { get => Players[CP]; }
        public int CurrentTurn { get; private set; }
        private int CP = -1;
        public GameState(IUser Host,int GameID)
        {
            this.GameID = GameID;
            GameHost = Host;
            Board = new(GameID);
        }
        internal bool StartGame()
        {
            if(Players.Count < MinimumPlayers) 
                return false;

            GameActive = true;
            CurrentTurn = 0;
            CP = 0;
            return true;
        }
        private void EndCurrentPlayerTurn()
        {
            Players[CP].EndTurnUpdate();
            if (Players[CP] == Players[^1])
                CP = 0;
            else 
                CP++;
        }
        /// <summary>
        /// returns if a specified tile is owned
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        internal bool CheckOwned(Tuple<int,int> tile)
        {
            return Board.GameMap[tile.Item1][tile.Item2].Owner is not null;
        }
        internal void SetBuyTiles(Tuple<int, int>[] tiles)
        {
            Board.SetBuyState(tiles);
        }
        /// <summary>
        /// Claims the currently contested region
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal bool PlayerClaim(IUser user)
        {
            return Board.ClaimLand(Players.Where(x => x.User == user).First());
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
