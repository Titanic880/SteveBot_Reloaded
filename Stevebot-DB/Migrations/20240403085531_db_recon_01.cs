using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stevebot_DB.Migrations
{
    /// <inheritdoc />
    public partial class db_recon_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasOptional = table.Column<bool>(type: "bit", nullable: false),
                    OptionalData = table.Column<bool>(type: "bit", nullable: false),
                    ExtendedInfo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListObjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    PK_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    de = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    en = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    es = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    it = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    zh = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.PK_Id);
                });

            migrationBuilder.CreateTable(
                name: "Planet_Attacks",
                columns: table => new
                {
                    PK_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<int>(type: "int", nullable: false),
                    Target = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planet_Attacks", x => x.PK_Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    PK_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    x = table.Column<float>(type: "real", nullable: false),
                    y = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.PK_Id);
                });

            migrationBuilder.CreateTable(
                name: "WarStatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Campaigns_DB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Global_Events_DB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Planet_Attacks_DB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    impact_multiplier = table.Column<float>(type: "real", nullable: false),
                    snapshot_at = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    started_at = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    war_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarStatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    effects = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FK_MessageId = table.Column<int>(type: "int", nullable: false),
                    Planets_DB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    race = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.id);
                    table.ForeignKey(
                        name: "FK_Events_messages_FK_MessageId",
                        column: x => x.FK_MessageId,
                        principalTable: "messages",
                        principalColumn: "PK_Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FeedMessages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    FK_MessageId = table.Column<int>(type: "int", nullable: false),
                    published = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedMessages", x => x.id);
                    table.ForeignKey(
                        name: "FK_FeedMessages_messages_FK_MessageId",
                        column: x => x.FK_MessageId,
                        principalTable: "messages",
                        principalColumn: "PK_Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    index = table.Column<int>(type: "int", nullable: false),
                    disabled = table.Column<bool>(type: "bit", nullable: false),
                    hash = table.Column<float>(type: "real", nullable: false),
                    initial_owner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    max_health = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FK_Position = table.Column<int>(type: "int", nullable: false),
                    sector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    waypoints = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.index);
                    table.ForeignKey(
                        name: "FK_Planets_Positions_FK_Position",
                        column: x => x.FK_Position,
                        principalTable: "Positions",
                        principalColumn: "PK_Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    FK_PlanetId = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.id);
                    table.ForeignKey(
                        name: "FK_Campaigns_Planets_FK_PlanetId",
                        column: x => x.FK_PlanetId,
                        principalTable: "Planets",
                        principalColumn: "index",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "JointOperations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    hq_node_index = table.Column<int>(type: "int", nullable: false),
                    FK_PlanetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JointOperations", x => x.id);
                    table.ForeignKey(
                        name: "FK_JointOperations_Planets_FK_PlanetId",
                        column: x => x.FK_PlanetId,
                        principalTable: "Planets",
                        principalColumn: "index",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PlanetStatuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    health = table.Column<float>(type: "real", nullable: false),
                    liberation = table.Column<float>(type: "real", nullable: false),
                    owner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FK_PlanetId = table.Column<int>(type: "int", nullable: false),
                    players = table.Column<float>(type: "real", nullable: false),
                    regen_per_second = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanetStatuses", x => x.id);
                    table.ForeignKey(
                        name: "FK_PlanetStatuses_Planets_FK_PlanetId",
                        column: x => x.FK_PlanetId,
                        principalTable: "Planets",
                        principalColumn: "index",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PlanetEvents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_CampaignId = table.Column<int>(type: "int", nullable: false),
                    event_type = table.Column<int>(type: "int", nullable: false),
                    expire_time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    health = table.Column<int>(type: "int", nullable: false),
                    max_health = table.Column<int>(type: "int", nullable: false),
                    FK_PlanetId = table.Column<int>(type: "int", nullable: false),
                    race = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    start_time = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanetEvents", x => x.id);
                    table.ForeignKey(
                        name: "FK_PlanetEvents_Campaigns_FK_CampaignId",
                        column: x => x.FK_CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PlanetEvents_Planets_FK_PlanetId",
                        column: x => x.FK_PlanetId,
                        principalTable: "Planets",
                        principalColumn: "index",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_FK_PlanetId",
                table: "Campaigns",
                column: "FK_PlanetId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_FK_MessageId",
                table: "Events",
                column: "FK_MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedMessages_FK_MessageId",
                table: "FeedMessages",
                column: "FK_MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_JointOperations_FK_PlanetId",
                table: "JointOperations",
                column: "FK_PlanetId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanetEvents_FK_CampaignId",
                table: "PlanetEvents",
                column: "FK_CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanetEvents_FK_PlanetId",
                table: "PlanetEvents",
                column: "FK_PlanetId");

            migrationBuilder.CreateIndex(
                name: "IX_Planets_FK_Position",
                table: "Planets",
                column: "FK_Position");

            migrationBuilder.CreateIndex(
                name: "IX_PlanetStatuses_FK_PlanetId",
                table: "PlanetStatuses",
                column: "FK_PlanetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataLogs");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "FeedMessages");

            migrationBuilder.DropTable(
                name: "JointOperations");

            migrationBuilder.DropTable(
                name: "ListObjects");

            migrationBuilder.DropTable(
                name: "Planet_Attacks");

            migrationBuilder.DropTable(
                name: "PlanetEvents");

            migrationBuilder.DropTable(
                name: "PlanetStatuses");

            migrationBuilder.DropTable(
                name: "WarStatus");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "Planets");

            migrationBuilder.DropTable(
                name: "Positions");
        }
    }
}
