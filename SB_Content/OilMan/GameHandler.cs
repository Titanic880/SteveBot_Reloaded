using Discord.WebSocket;
using Discord;

using System.Collections.Generic;
using System.Linq;
using System;

namespace SB_Content.OilMan
{
    /// <summary>
    /// Highest level of Oilman Based Objects
    /// </summary>
    public static class GameHandler
    {
        private static List<GameState> Games = new();
        private static readonly Emoji[] GameAssets = { };

        /// <summary>
        /// Takes 1:1 Throw from Reaction Added (TO BE TRIMMED)
        /// </summary>
        /// <param name="msgCache"></param>
        /// <param name="msgChannel"></param>
        /// <param name="reaction"></param>
        public static Tuple<bool, string> ReactionThrow(Cacheable<IUserMessage, ulong> msgCache, Cacheable<IMessageChannel, ulong> msgChannel, SocketReaction reaction)
        {
            throw new NotImplementedException();
        }



        public static Tuple<bool,string> InitilizeNewGame(IGuildUser Host)
        {

            if (Games.FirstOrDefault(x => x.GameHost == Host) != null)
                return Tuple.Create(false,"You are already the host of a game!");


            Games.Add(new GameState(Host,Games.Count+1));
            return Tuple.Create(true, $"Game has been created with an ID of {Games.Count}");
        }

        public static Tuple<bool,string> ReactionInteract()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Ends game and returns results dialog
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        internal static Embed EndGame(IGuildUser Host, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Outputs the Emoji Legend of what tiles are
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        internal static Embed BuildLegend()
        {
            throw new NotImplementedException();
        }
    }
}
