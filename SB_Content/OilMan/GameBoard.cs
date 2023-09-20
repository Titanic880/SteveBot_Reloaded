using System.Threading.Tasks;
using System;
using Discord;
using System.Collections.Generic;

namespace SB_Content.OilMan
{
    //Drill Types
    internal enum Drilling_Types
    {
        Stepup = 0,
        FirstToThird = 1,
        FullDepth = 2
    }
    //Ground level Drilling Costs
    internal enum Ground_costs
    {
        Drill_1 = 5000,
        Drill_2 = 10000,
        Drill_3 = 15000,
        FirstToThird = 30000, //1 to 3
        FullDepth = 35000 //Empty to 3
    }
    //Water level Drilling Costs
    internal enum Water_costs
    {
        Drill_1 = 15000,
        Drill_2 = 30000,
        Drill_3 = 45000,
        FirstToThird = 90000,
        FullDepth = 115000
    }
    

    internal class GameBoard
    {
        private const int GameWidth = 15;
        private const int GameHeight = 12;
        private readonly Oilman_Player BuySave;
        public GameBoard(int GameID)
        {
            BuySave = new Oilman_Player(new Blank_User(), GameID);
        }
        public GameTile[][] GameMap { get; private set; } = Array.Empty<GameTile[]>();
        //             y:x
        internal async Task<bool> GenerateMap(int Type)
        {
            return await Task.Run(() =>
            {
                if(Type == 1) { }
                else if(Type == 2) { }
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
        internal void SetBuyState(Tuple<int,int>[] tiles)
        {
            foreach(Tuple<int,int> Tile in tiles)
                GameMap[Tile.Item1][Tile.Item2].SetOwner(BuySave);
        }
        internal bool ClaimLand(Oilman_Player _player)
        {
            foreach (GameTile tile in BuySave.OwnedTiles)
            {
                if (!tile.SetOwner(_player)) return false;
                _player.OwnedTiles.Add(tile);
            }
            return true;
        }
        internal Tuple<bool, string> DrillTile(Drilling_Types _Drill, Tuple<int, int> Tile)
            => GameMap[Tile.Item1][Tile.Item2].Drill(_Drill); 
    }
}
