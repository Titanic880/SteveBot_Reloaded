namespace SB_Content {
    public interface IRandomizer {
        public void Randomize(bool[] ToRandomize);
        public string GetResult();

        public bool SetSafeGuards(bool[] Toggles);
    }
}
