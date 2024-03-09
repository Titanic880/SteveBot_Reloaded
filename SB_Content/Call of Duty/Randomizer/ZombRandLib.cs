using System.Linq;
using System;

namespace SB_Content.Call_of_Duty.Randomizer {
    public class ZombRandLib : IRandomizer
    {
        readonly Random rand = new();
        readonly ZmPerks perks = new();

        #region Info
        string explName = "Explosives";

        public string WeaponRand { get; private set; } = "";
        public string FieldRand { get; private set; } = "";
        public string OrderedPerks { get; private set; } = "";
        public string SupportRand { get; private set; } = "";
        public string TacticalRand { get; private set; } = "";
        public string LethalRand { get; private set; } = "";

        /// <summary>
        /// Field Upgrade
        /// </summary>
        public bool FR{ get; private set; } = false;
        /// <summary>
        /// Support
        /// </summary>
        public bool SR { get; private set; } = false;
        /// <summary>
        /// Tactical
        /// </summary>
        public bool TR { get; private set; } = false; 
        /// <summary>
        /// Lethal
        /// </summary>
        public bool LR { get; private set; } = false;
        /// <summary>
        /// Weapon
        /// </summary>
        public bool Wep { get; private set; } = false;

        #region SafeGuards
        /// <summary>
        /// Changes the name Explosive to "Content Cannons"
        /// </summary>
        public bool Content { get; private set; } = false;
        /// <summary>
        /// Flag that says to allow the DLC weapons 
        /// </summary>
        public bool DLC_Enabled { get; private set; } = false;
        /// <summary>
        /// bool to determine if out of category weapons are good from box
        /// </summary>
        public bool Box_OutofCat { get; private set; } = false;
        /// <summary>
        /// bool to determine if out of category weapons are good from wall
        /// </summary>
        public bool Wall_OutofCat { get; private set; } = false;

        public bool PerkOrd { get; private set; } = false;
        #endregion SafeGuards
        #endregion Info

        #region SetInfo
        public void Randomize(bool[] ToRandomize) {
            Wep = ToRandomize[0];
            SetDLC(ToRandomize[1]);
            Box_OutofCat = ToRandomize[2];
            Wall_OutofCat = ToRandomize[3];
            FR = ToRandomize[4];
            PerkOrd = ToRandomize[5];
            SR = ToRandomize[6];
            TR = ToRandomize[7];
            LR = ToRandomize[8];

            RandEquipment();
            if (PerkOrd) {
                PerkOrder();
            }
            if (Wep) {
                RandomWeapon();
            }
            SetOutofCategory();

        }

        public void ApplyOptions(bool[] Sets)
        {
            Wep = Sets[0];
            SetDLC(Sets[1]);
            Box_OutofCat = Sets[2];
            Wall_OutofCat = Sets[3];
            FR = Sets[4];
            PerkOrd = Sets[5];
            SR = Sets[6];
            TR = Sets[7];
            LR = Sets[8];
        }
        public void TrueRandomize()
        {
            SetContent(Convert.ToBoolean(rand.Next(2)));
            SetDLC(Convert.ToBoolean(rand.Next(2)));
            SetOutofCategory();

            bool[] rnd = { Convert.ToBoolean(rand.Next(2)), Convert.ToBoolean(rand.Next(2)), Convert.ToBoolean(rand.Next(2)), Convert.ToBoolean(rand.Next(2)) };
            FR = rnd[0];
            LR = rnd[1];
            SR = rnd[2];
            TR = rnd[3];
            RandEquipment();
            PerkOrder();
            if (Convert.ToBoolean(rand.Next(2)))
            {
                RandomWeapon();
                Wep = true;
            }
        }
        public void Randomize()
        {
            RandEquipment();
            if(PerkOrd) PerkOrder();
            if (Wep) RandomWeapon();
            SetOutofCategory();
        }

        public string GetResult() => 
            $"Weapon: {WeaponRand}" +
            $"\nDLC: {(DLC_Enabled ? "Enabled" : "Disabled")}" +
            $"\n{(Box_OutofCat ? "Box has been banned" : "Box is allowed")}" +
            $"\n{(Wall_OutofCat ? "Wall weapons have been banned" : "Wall weapons are allowed")}" +
            $"\nField Upgrade: {FieldRand}" +
            $"\nPerk order: {OrderedPerks}" +
            $"\nSupport item: {SupportRand}" +
            $"\nTactical item: {TacticalRand}" +
            $"\nLethal item: {LethalRand}";

        /// <summary>
        /// Sets the value of content
        /// </summary>
        /// <param name="cont"></param>
        private string SetContent(bool cont)
        {
            if (cont)
                explName = "Content Cannons";
            else
                explName = "Explosives";

            return explName;
        }
        private void SetDLC(bool cont) => DLC_Enabled = cont;
        
        #region OutofCategory
        /// <summary>
        /// Sets the out of category of box weapons
        /// </summary>
        /// <param name="chk"></param>
        public void SetOutofCategory_Box(bool chk) => Box_OutofCat = chk;       
        /// <summary>
        /// Sets the out of category of wall weapons
        /// </summary>
        /// <param name="chk"></param>
        public void SetOutofCategory_Wall(bool chk) => Wall_OutofCat = chk;
        /// <summary>
        /// Sets random info for out of categories
        /// </summary>
        public void SetOutofCategory()
        {
            if(Box_OutofCat) SetOutofCategory_Box(Convert.ToBoolean(rand.Next(2)));
            if(Wall_OutofCat) SetOutofCategory_Wall(Convert.ToBoolean(rand.Next(2)));
        }
        #endregion OutofCategory
        #endregion SetInfo
        #region Randomize
        /// <summary>
        /// Returns category and Weapon (Comma seperated)
        /// </summary>
        /// <returns></returns>
        private void RandomWeapon()
        {
            WeaponNames names = new();
            int Category = rand.Next(names.WeaponCategories.Length - 1);
            string ret = names.WeaponCategories[Category] + ",";
            ///Checks to see if the category is DLC, and if it is enabled
            if (Category == 9 && DLC_Enabled)
                ret += names.DLC[rand.Next(names.DLC.Length)];
            ///Checks for the disabled DLC, and if the Category is selected
            else if (Category == 9 && !DLC_Enabled)
                //Recursion at its finest...
                RandomWeapon();
            else
            {
                ///Find a better method ?
                switch (Category)
                {
                    case 0:
                        ret += names.SMG[rand.Next(names.SMG.Length - 1)];
                        break;
                    case 1:
                        ret += names.Shotgun[rand.Next(names.Shotgun.Length - 1)];
                        break;
                    case 2:
                        ret += names.Pistol[rand.Next(names.Pistol.Length - 1)];
                        break;
                    case 3:
                        ret += names.TacR[rand.Next(names.TacR.Length - 1)];
                        break;
                    case 4:
                        ret += names.Sniper[rand.Next(names.Sniper.Length - 1)];
                        break;
                    case 5:
                        ret += names.LMG[rand.Next(names.LMG.Length - 1)];
                        break;
                    case 6:
                        ret += names.AR[rand.Next(names.AR.Length - 1)];
                        break;
                    case 7:
                        ret += names.Melee[0];            //HARDCODED 0
                        break;
                    case 8:
                        ret = explName;
                        ret += ": "+names.Expl[rand.Next(names.Expl.Length)];
                        break;
                }
            }
            WeaponRand = ret;
        }

        /// <summary>
        /// Options :: Banned Specific, All but Specified banned, None
        /// </summary>
        /// <returns></returns>
        private void RandEquipment()
        {
            if(FR) FieldRand = perks.FieldUpgrades[rand.Next(perks.FieldUpgrades.Length - 1)];
            if(SR) SupportRand = perks.Support[rand.Next(perks.Support.Length - 1)];
            if(TR) TacticalRand = perks.Tactical[rand.Next(perks.Tactical.Length - 1)];
            if(LR) LethalRand = perks.Lethal[rand.Next(perks.Lethal.Length - 1)];
        }
        /// <summary>
        /// Randomize the order that you would obtain perks (comma seperated)
        /// </summary>
        /// <returns></returns>
        private void PerkOrder()
        {
            string[] random = perks.Perks.OrderBy(x => rand.Next(perks.Perks.Length-1)).ToArray();
            OrderedPerks = "";
            for(int i = 0; i < random.Length; i++)
                OrderedPerks += (i+1)+": "+random[i] + '\n';
            OrderedPerks = OrderedPerks.Trim('\n');
        }

        public bool SetSafeGuards(bool[] Toggles) {
            //None Used For This System
            throw new NotImplementedException();
        }
        #endregion Randomize
    }
}