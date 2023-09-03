using System;

namespace SB_Content.Runescape
{
    public enum RS3Rituals
    {
        LesNecro = 0,
        LesCommun = 1,
        LesEnsoul,
        LesEss,
        GreNecro,
        GreCommun,
        GreEnsoul,
        GreEss,
        PowNecro,
        PowCommun,
        PowEnsoul,
        PowEss
    }
    public enum RS3Glyphs
    {
        Commune1, Commune2, Commune3,
        Elemental1, Elemental2, Elemental3,
        Reagent1, Reagent2, Reagent3,
        Change1, Change2, Change3,
        Multiply1, Multiply2, Multiply3,
        Protection1, Protection2, Protection3,
        Speed1, Speed2, Speed3,
        Attraction1, Attraction2, Attraction3,
    }
    public enum RS3GlyphLifetime
    {
        Core1,
        Core2,
        Core3,
        Alt1,
        Alt2,
        Alt3,
        BasicCandle,
        WhiteCandle,
        RegularCandle,
        GreaterCandle,
        SkullCandle
    }
    public class RSJson
    {
        public int[] NecroplasmPrices = new int[3] { 0, 1, 2 }; //Basic ink not included as price is always 1 (Buying from shop)
        public int[] InkPrices = new int[3] { 0, 1, 2 };
        //private readonly int[] RitualDurations = { 30, 60, 84 }; //Amount of time stated in game for the ritual to take (Assumed to be seconds)
        //private readonly int[] RitualsPerHour = { 119, 59, 42 }; //Theoretical Max amount of rituals per hour (-1 for est inbetweens)
        public DateTime LastUpdated = DateTime.UtcNow;
        public bool UpdatedCall = false;
        public int AshPrice = 69;
        public int VialOfWater = 10; //Shop Value
        public int Ectoplasm = 80;
        /// <summary>
        /// outputs total setup cost (not running/1 ritual)
        /// </summary>
        /// <param name="ritual"></param>
        /// <returns></returns>
        public int RitualSetup_Cost(RS3Rituals ritual)
        {
            //Pure numbers being added is the price of the Basic Ink ex: LesNecro returns 7 always
            return ritual switch
            {
                RS3Rituals.LesNecro => 7,
                RS3Rituals.LesCommun => 11,
                RS3Rituals.LesEnsoul => 7 + InkPrices[0] * 2,
                RS3Rituals.LesEss => 8 + InkPrices[0] * 4,
                RS3Rituals.GreNecro => 13 + InkPrices[0] * 10,
                RS3Rituals.GreCommun => 8 + InkPrices[0] * 6 + InkPrices[1] * 8,
                RS3Rituals.GreEnsoul => 7 + InkPrices[0] * 8 + InkPrices[1] * 4,
                RS3Rituals.GreEss => 4 + InkPrices[0] * 9 + InkPrices[1] * 6,
                RS3Rituals.PowNecro => 4 + InkPrices[0] * 20 + InkPrices[1] * 16,
                RS3Rituals.PowCommun => InkPrices[0] * 8 + InkPrices[1] * 20 + InkPrices[2] * 8,
                RS3Rituals.PowEss => InkPrices[0] * 10 + InkPrices[1] * 18 + InkPrices[2] * 4,
                RS3Rituals.PowEnsoul => 6 + InkPrices[0] * 11 + InkPrices[1] * 4,
                _ => -1,
            };
        }
        /// <summary>
        /// Cost of a single Alteration Glyph
        /// </summary>
        /// <param name="Glyph"></param>
        /// <returns></returns>
        public int AlterationCost(RS3Glyphs Glyph)
        {
            //Pure numbers being added is the price of the Basic Ink
            return Glyph switch
            {
                RS3Glyphs.Multiply1 => Ectoplasm + InkPrices[0] * 2 + 1,
                RS3Glyphs.Multiply2 => Ectoplasm + InkPrices[0] * 2 + 2,
                RS3Glyphs.Multiply3 => Ectoplasm * 2 + InkPrices[1] * 2 + InkPrices[2] * 2,
                RS3Glyphs.Protection1 => Ectoplasm + InkPrices[0] * 2 + 1,
                RS3Glyphs.Protection2 => Ectoplasm + InkPrices[0] * 2 + 2,
                RS3Glyphs.Protection3 => Ectoplasm * 2 + InkPrices[1] * 2 + InkPrices[2] * 2,
                RS3Glyphs.Speed1 => Ectoplasm + InkPrices[0] * 2 + 1,
                RS3Glyphs.Speed2 => Ectoplasm + InkPrices[0] * 2 + InkPrices[1] * 2,
                RS3Glyphs.Speed3 => Ectoplasm * 2 + InkPrices[1] * 2 + InkPrices[2] * 2,
                RS3Glyphs.Attraction1 => Ectoplasm + InkPrices[0] * 2 + 2,
                RS3Glyphs.Attraction2 => Ectoplasm + InkPrices[1] + InkPrices[2] * 2,
                RS3Glyphs.Attraction3 => Ectoplasm * 2 + InkPrices[1] * 2 + InkPrices[2] * 2,
                _ => -1,
            };
        }
        public static int GlyphLifeSpan(RS3GlyphLifetime Lifetime)
        {
            return Lifetime switch
            {
                RS3GlyphLifetime.Core1 => 6,
                RS3GlyphLifetime.Core2 => 12,
                RS3GlyphLifetime.Core3 => 18,
                RS3GlyphLifetime.Alt1 => 3,
                RS3GlyphLifetime.Alt2 => 6,
                RS3GlyphLifetime.Alt3 => 9,
                RS3GlyphLifetime.BasicCandle => 6,
                RS3GlyphLifetime.WhiteCandle => 9,
                RS3GlyphLifetime.RegularCandle => 12,
                RS3GlyphLifetime.GreaterCandle => 18,
                RS3GlyphLifetime.SkullCandle => 36,
                _ => -1,
            };
        }
    }
}