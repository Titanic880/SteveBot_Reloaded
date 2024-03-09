using System;
using System.Linq;
using static SB_Content.Payday.PD2.Generic_Data;

namespace SB_Content.Payday.PD2 {
    internal class NewRando : IRandomizer {
        private string Result = "";
        private readonly Random rand;

        public NewRando() {
            rand = new Random(DateTime.Now.Millisecond);
        }
        public string GetResult() {
            return Result;
        }
        public bool SetSafeGuards(bool[] Toggles) {
            if(Toggles.Length < 3) {
                return false;
            }
            HitmanSafeGuard = Toggles[0];
            GrinderSafeGuard = Toggles[1];
            Allow_OneDown = Toggles[2];
            return true;
        }
        /// <summary>
        /// bool[] length of 8 expected
        /// </summary>
        /// <param name="ToRandomize"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Randomize(bool[] ToRandomize) {
            string perkdeck = "";
            string ret = "";
            if (ToRandomize[0]) {
                perkdeck  = PerkDeck();
                ret += "Perk Deck: " + perkdeck;
            }
            if (ToRandomize[2]) {
                ret += "\nPrimary: " + Primary(perkdeck);
            }
            if (ToRandomize[1]) {
                ret += "\nThrowable: " + Throwable(perkdeck);
            }
            if (ToRandomize[3]) {
                ret += "\nSecondary: " + Secondary();
            }
            if (ToRandomize[4]) {
                ret += "\nMelee: " + Melee();
            }
            if (ToRandomize[5]) {
               ret +="\nDeployable: " + Deployable();
            }
            if (ToRandomize[6]) {
                ret += "\nArmor: " + Armor(perkdeck);
            }
            if (ToRandomize[7]) {
                ret += "\nDifficulty: " + Difficulty();
            }

            Result = ret;
            return;
        }
        private string PerkDeck() {
            return PerkDecks[rand.Next(PerkDecks.Length - 1)];
        }
        private string Throwable(string PerkDeck) {
            if (!DeckEquips.Contains(PerkDeck)) {
                return Throwables[rand.Next(Throwables.Length - 1)];
            }
            //Checks if the current perk deck has an equipable, if it does sets it
            for (int i = 0; i < DeckEquips.Length; i++)
                if (PerkDeck == DeckEquips[i]) {
                    return DeckThrowables[i];
                }
            //if no deck specific throwable is found, run it against full list
            return Throwables[rand.Next(Throwables.Length - 1)];
        }
        private string Primary(string PerkDeck) {
            if (PerkDeck == "Hitman" && HitmanSafeGuard) {
                return Primaries[rand.Next(3) + 5];
            }
            return Primaries[rand.Next(Primaries.Length - 1)];
        }
        private string Secondary() {
            return Secondaries[rand.Next(Secondaries.Length - 1)];
        }
        private string Melee() {
            return Melees[rand.Next(Melees.Length - 1)];
        }
        private string Deployable() {
            return Deployables[rand.Next(Deployables.Length - 1)];
        }
        private string Armor(string PerkDeck) {
            //Checks for the grinder deck and checks the safeguard
            if (PerkDeck != "Grinder" || !GrinderSafeGuard) {
                return Generic_Data.Armor[rand.Next(Generic_Data.Armor.Length - 1)];
            }
            if (rand.Next(2) == 1) {
                return "LightWeight Ballistic";
            } else {
                return "Two Piece Suit";
            }
        }
        private string Difficulty() {
            string ret = Difficulties[rand.Next(Difficulties.Length) + 1];
            //Check for One down mechanic addition
            if (Allow_OneDown && rand.Next(2) == 1)
                ret += ": One Down";
            return ret;
        }


        #region Options
        /// <summary>
        /// Determines if random primary will only roll Akimbos when hitman it rolled
        /// </summary>
        private bool HitmanSafeGuard { get; set; } = true;
        private bool GrinderSafeGuard { get; set; } = true;
        private bool Allow_OneDown { get; set; } = true;
        #endregion Options
    }
}
