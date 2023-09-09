using System;

namespace SB_Content.OilMan
{
    internal class GameTile
    {
        private static Random rand = new Random();
        //Income Calculation items
        private const int baseincome = 2000;
        private const double WtrMultiplier = 2.5;
        /// <summary>
        /// True = Water, False = Ground
        /// </summary>
        public readonly bool LandState;
        public readonly int TileID;
        public Oilman_Player? Owner { get; private set; }
        public int DrillDepth { get; private set; } = -1;
        /// <summary>
        /// Level where oil is; 4 is empty
        /// </summary>
        private readonly int WellDepth;
        /// <summary>
        /// Indicates if the State of the Tile is allowed to be known
        /// </summary>
        public bool VisibleDepth { get => DrillDepth >= WellDepth; }

        public GameTile(int tileID, bool State)
        {
            LandState = State;
            WellDepth = rand.Next(1, 4);    //To be replaced with heat map Random
            TileID = tileID;
        }
        public GameTile(int tileID)
        {
            LandState = Convert.ToBoolean(rand.Next(0,100)%2);
            WellDepth = rand.Next(1, 4);    //To be replaced with heat map Random
            TileID = tileID;
        }
        public bool SetOwner(Oilman_Player user)
        {
            if (Owner != null) return false;
            DrillDepth = 0;
            Owner = user;
            return true;
        }
        public int GetWellDepth()
        {
            if (VisibleDepth) return WellDepth;
            return -1;
        }
        public int GetTileIncome()
        {
            if (!VisibleDepth) return 0;
            return (LandState ? (int)(baseincome * WtrMultiplier) : baseincome) * WellDepth;
        }
        public Tuple<bool,string> Drill(Drilling_Types _Drill)
        {
            if (Owner == null) return Tuple.Create(false, "Attempted to dig at unassigned Tile...");
            if (DrillDepth >= 3) return Tuple.Create(false,"Cannot drill further!");

            switch (_Drill)
            {
                //0-3 dig
                case Drilling_Types.FullDepth:
                    if (DrillDepth != 0)
                        return Tuple.Create(false, "Cannot Full depth on drilled tile");
                    if (!Owner.Pay(LandState ? (int)Water_costs.FullDepth : (int)Ground_costs.FullDepth))
                        return Tuple.Create(false, $"{Owner} Cannot afford Full Depth!");
                    if (WellDepth != 4)
                        DrillDepth = 3;
                    else DrillDepth = WellDepth;                        
                    break;
                //Goes from 1 -> 3 digs (1 turn save)
                case Drilling_Types.FirstToThird:
                    if (DrillDepth != 1)
                        return Tuple.Create(false, "Cannot First to third off of first drill");
                    if (!Owner.Pay(LandState ? (int)Water_costs.FirstToThird : (int)Ground_costs.FirstToThird))
                        return Tuple.Create(false, $"{Owner} Cannot afford First to Third!");
                    if (WellDepth != 4)
                        DrillDepth = 3;
                    else DrillDepth = WellDepth;
                    break;
                //Primary type of drilling
                case Drilling_Types.Stepup:
                    int cost = 0;
                    if(DrillDepth == 0)
                        cost = LandState ? (int)Water_costs.Drill_1 : (int)Ground_costs.Drill_1;
                    else if(DrillDepth == 1)
                        cost = LandState ? (int)Water_costs.Drill_2 : (int)Ground_costs.Drill_2;
                    else if(DrillDepth == 2)
                        cost = LandState ? (int)Water_costs.Drill_3 : (int)Ground_costs.Drill_3;
                    if(!Owner.Pay(cost))
                        return Tuple.Create(false, $"{Owner} Cannot afford to dig");
                    DrillDepth += 1;
                    break;
            }
            string output;

            //Setup output text
            if (VisibleDepth)
                output = $"While Digging you struck oil! it is outputting: ${GetTileIncome()}";
            else if (DrillDepth == 3)
                output = "Unfortunately no oil has been found at this site!";
            else
                output = "While digging you have yet to find oil";

            return Tuple.Create(true, output);
        }

        public override string ToString()
        {
            //Returns Blank Tile (owner color if owned)
            if (!VisibleDepth)
            {
                if (Owner == null || Owner.Player_Color == null)
                    return LandState ? GameHandler.GameAssets[2].Name : GameHandler.GameAssets[3].Name;
                return LandState ? Owner.Player_Color.Item1.Name : Owner.Player_Color.Item2.Name;
            }

            //Shallow Well
            if (WellDepth == 1) return GameHandler.GameAssets[4].Name;
            //Medium Well
            if (WellDepth == 2) return GameHandler.GameAssets[5].Name;
            //Deepest Well
            if (WellDepth == 3) return GameHandler.GameAssets[6].Name;
            //Empty Well
            if (WellDepth == 4) return GameHandler.GameAssets[0].Name;

            return GameHandler.GameAssets[1].Name;
        }
    }
}
