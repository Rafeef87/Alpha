using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_UserEntityId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Clients_ClientEntityId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Statuses_StatusEntityId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "MemberAddresses");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ClientEntityId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_StatusEntityId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_UserEntityId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Client",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ClientEntityId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "StatusEntityId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "User",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Projects");

            migrationBuilder.AlterColumn<string>(
                name: "StatusName",
                table: "Statuses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ClientName",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_StatusName",
                table: "Statuses",
                column: "StatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ClientId",
                table: "Projects",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StatusId",
                table: "Projects",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserId",
                table: "Projects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientName",
                table: "Clients",
                column: "ClientName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_UserId",
                table: "Projects",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Clients_ClientId",
                table: "Projects",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Statuses_StatusId",
                table: "Projects",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_UserId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Clients_ClientId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Statuses_StatusId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Statuses_StatusName",
                table: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ClientId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_StatusId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_UserId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Clients_ClientName",
                table: "Clients");

            migrationBuilder.AlterColumn<string>(
                name: "StatusName",
                table: "Statuses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Client",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientEntityId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StatusEntityId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserEntityId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientName",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberAddresses",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberAddresses", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_MemberAddresses_Members_UserId",
                        column: x => x.UserId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ClientEntityId",
                table: "Projects",
                column: "ClientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StatusEntityId",
                table: "Projects",
                column: "StatusEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserEntityId",
                table: "Projects",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_UserEntityId",
                table: "Projects",
                column: "UserEntityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Clients_ClientEntityId",
                table: "Projects",
                column: "ClientEntityId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Statuses_StatusEntityId",
                table: "Projects",
                column: "StatusEntityId",
                principalTable: "Statuses",
                principalColumn: "Id");
        }
    }
}
