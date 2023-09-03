using Discord;
using System;

namespace SB_Content.OilMan
{
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
        public readonly GameTile[][] GameMap;

        public GameBoard() { }
        public GameBoard(int BoardType) { }


        internal Tuple<bool, string> BuyLand(Tuple<int, int>[] Tiles, Oilman_Player _player)
        {
            Tuple<bool, string> retitem = Tuple.Create(true, $"Land has been claimed by {_player}");

            foreach (Tuple<int, int> tup in Tiles)
            {
                if (!GameMap[tup.Item1][tup.Item2].SetOwner(_player))
                {
                    retitem = Tuple.Create(false, $"Tile Already owned: {tup.Item1}:{tup.Item2}");
                    break;
                }
                _player.OwnedTiles.Add(GameMap[tup.Item1][tup.Item2]);
            }
            return retitem;
        }
        internal Tuple<bool, string> DrillTile(Tuple<int, int> Tile, Drilling_Types _Drill)
            => GameMap[Tile.Item1][Tile.Item2].Drill(_Drill);
        
    }
}
