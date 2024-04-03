using Stevebot_DB.Framework_ENT;
using Discord.Commands;
using Discord;
using Stevebot_DB.DB_Content.HellDiver.API_Objects;
namespace CommandModule.Modules
{
    public class HellDivers : ModuleBase<SocketCommandContext> {

        [Command("FUHDL")]
        public async Task UpdateLiberations() {
            PlanetStatus[] status = HellDiverLogic.GetAllPlanetStatus();
            // || x.players != 0 || x.regen_per_second != 1388.88892
            if (status.Length == 0) {
                Stevebot_DB.Framework_ENT.HellDiverLogic.UpdateDatabase();
                status = HellDiverLogic.GetAllPlanetStatus();
            }
            EmbedBuilder builder = new();

            for (int i = 0; i < status.Length; i++) {
                if (status[i].planet == null) {
                    status[i].planet = HellDiverLogic.GetPlanet(status[i].FK_PlanetId);
                }
                if (status[i].health == status[i].planet.max_health) {
                    continue;
                }

                string ConnectedPlanets = HellDiverLogic.GetConnectedPlanets(status[i].planet.waypoints);
                Embed embed = builder.WithTitle(status[i].planet.name)
                    .WithDescription(
                    $"Planet Health: {status[i].health}/{status[i].planet.max_health}" +
                    $"\nLiberation Status: {status[i].liberation}%" +
                    $"\nActive Players: {status[i].players}" +
                    $"\nSupply Routes: {ConnectedPlanets}" +
                    $"Current Regen/s: {status[i].regen_per_second}"

                    ).WithCurrentTimestamp().WithColor(Color.Orange)
                    .WithFooter(status[i].planet.sector).Build();
                await ReplyAsync(embed: embed);
            }
        }
    }
}
