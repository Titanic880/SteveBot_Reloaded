namespace SB_Content.Call_of_Duty
{
    /// <summary>
    /// Perks related to multiplayer
    /// </summary>
    public class MPPerks : TacticalEquipment
    {
        public MPPerks()
        {
            Tactical[4] = Tactical[4].Split('/')[0];
        }
        //Will contain Wildcards
        public string[] Field { get; } =
        {
            "Proximity Mine",
            "Field Mic",
            "Trophy System",
            "Assault Pack",
            "SAM Turret",
            "Jammer",
            "Gas Mine"
        };
        public string[] WildCard { get; } =
        {
            "Danger Close", //2x Tact/Lethal + more ammo
            "Law Breaker", //Find use
            "GunFighter", //Weapon for any category for both slots
            "Perk Greed" //2x more perks (Find other conditions)
        };
    }
    /// <summary>
    /// Perks related to Zombies
    /// </summary>
    public class ZmPerks : TacticalEquipment
    {
        public ZmPerks()
        {
            Tactical[4] = Tactical[4].Split('/')[1];
        }
        public string[] Perks { get; } =
        {
            "Jugger-Nog",
            "Speed Cola",
            "Quick Revive",
            "Stamin-Up",
            "Elemental Pop",
            "Deadshot Daiquiri",
            "Tombstone"
        };
        public string[] FieldUpgrades { get; } =
{
            "Frost Blast",
            "Energy Mine",
            "Healing Aura",
            "Aether Shroud",
            "Ring of Fire"
        };
        public string[] Support { get; } =
        {
            "Combat Bow",
            "Sentry Turret",
            "War Machine",
            "Cruise Missle",
            "Chopper Gunner",
            "Self Revive",
            "Napalm",
            "Artillery"
        };
    }
    /// <summary>
    /// Full of the tactical items shared between the two modes : already implemented onto Perks
    /// </summary>
    public abstract class TacticalEquipment
    {
        public string[] Tactical { get; } =
        {
            "Stun Grenade",
            "StimShot",
            "Flashbang",
            "Decoy",
            "Smoke Grenade/Monkey Bomb"     //Split on the '/' depending on the mode
        };
        public string[] Lethal { get; } =
        {
            "Frag",
            "C4",
            "Semtex",
            "Molotov",
            "Tomahawk"
        };
    }
}
