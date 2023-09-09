using System.Collections.Generic;
using System;

using Discord;

namespace SB_Content.OilMan
{
    internal class Oilman_Player
    {
        public readonly IUser User;
        public readonly int GameID;
        public int Money { get; private set; }
        private int OwedMoney = 0;
        public int Wealth { get => Money - OwedMoney; }
        public int Income { get
            {
                int ttl = 0;
                foreach (GameTile tile in OwnedTiles)
                    ttl += tile.GetTileIncome();
                return ttl;
            }
            }
        public readonly List<GameTile> OwnedTiles = new();
        public Tuple<Emoji, Emoji>? Player_Color { get; private set; }

        //public Tuple<Emoji, Emoji> PlayerColors;
        public Oilman_Player(IUser User, int gameID)
        {
            this.User = User;
            Money = 100000;
            GameID = gameID;
        }
        internal Oilman_Player(string ov)
        {
            if (ov != "OVERRIDE_STATE")
                throw new AccessViolationException();
            User = null;
            Money = 0;
            GameID = -1;
            Player_Color = Tuple.Create(GameHandler.GameAssets[7], GameHandler.GameAssets[8]);
        }
        public void SetColors(Tuple<Emoji, Emoji> Colors)
        {
            Player_Color = Colors;
        }

        public bool Pay(int amount)
        {
            if (Money < amount) return false;
            Money -= amount;
            return true;
        }
        public void StartTurnUpdate()
        {
            Money += Income;
        }
        public void EndTurnUpdate()
        {
            
        }
        public override string ToString()
            => User.Username;
    }
}
