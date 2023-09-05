using System.Threading.Tasks;
using System;

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
                catch (Exception ex)
                {
                    return false;
                }
            });
        }

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
