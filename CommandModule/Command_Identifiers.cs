using Discord;

namespace CommandModule {
    public static class Command_Identifiers {
        public const char Command_Prefix = '$';
        public const int VERSION = 1001;

        /// <summary>
        /// https://www.fileformat.info/info/emoji/list.htm
        /// </summary>
        public static Emoji[] Emojis { get; } = //using get for references
[
            new("\u0031\u20E3"), //1
            new("\u0032\u20E3"), //2
            new("\u0033\u20E3"), //3
            new("\u0034\u20E3"), //4
            new("\u0035\u20E3"), //5
            new("\u0036\u20E3"), //6
            new("\u0037\u20E3"), //7
            new("\u0038\u20E3"), //8
            new("\u0039\u20E3"), //9
            new("\u2705")        //WhiteCheckMark
];
    }
}