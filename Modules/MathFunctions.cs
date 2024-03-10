using Discord.Commands;
using Discord;
using SB_Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteveBot_Rebuild.Modules {
    internal class MathFunctions : ModuleBase<SocketCommandContext> {
        #region Calc    
        [Command("math")]
        public async Task Math(string userinput) {
            double output = Calculator.Complex_Equation(userinput);
            EmbedBuilder EmbedBuilder = new EmbedBuilder()
                .WithDescription($"Your final output is {output}")
                .WithCurrentTimestamp();
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }
        [Command("dec2hex")]
        public async Task Dec2Hex(int Num1) {
            string output = Calculator.Dec2hex(Num1);

            EmbedBuilder EmbedBuilder = new EmbedBuilder()
                 .WithDescription($"{Num1} = {output}")
                 .WithCurrentTimestamp();
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }
        [Command("hex2dec")]
        public async Task Hex2Dec(string Num1) {
            string output = Calculator.Hex2dec(Num1).ToString();

            EmbedBuilder EmbedBuilder = new EmbedBuilder()
                 .WithDescription($"{Num1} = {output}")
                 .WithCurrentTimestamp();
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }
        [Command("fact")]
        public async Task Factorial(int Num1) {
            string output = Calculator.Factorial(Num1).ToString();

            EmbedBuilder EmbedBuilder = new EmbedBuilder()
                 .WithDescription($"!{Num1} = {output}")
                 .WithCurrentTimestamp();
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }
        #endregion Calc
    }
}
