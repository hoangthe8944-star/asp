using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hoangngocthe_2123110488.Migrations
{
    /// <inheritdoc />
    public partial class updatecategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "StreamCategories",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveStreamsCount",
                table: "StreamCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundImageUrl",
                table: "StreamCategories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BannerImageUrl",
                table: "StreamCategories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "StreamCategories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StreamCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "StreamCategories",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentViewers",
                table: "StreamCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Developer",
                table: "StreamCategories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GameplayVideoUrl",
                table: "StreamCategories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GeforceNowId",
                table: "StreamCategories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Genres",
                table: "StreamCategories",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconUrl",
                table: "StreamCategories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IgdbId",
                table: "StreamCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "StreamCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFreeToPlay",
                table: "StreamCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MetacriticScore",
                table: "StreamCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficialWebsiteUrl",
                table: "StreamCategories",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Platforms",
                table: "StreamCategories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PopularityRank",
                table: "StreamCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "StreamCategories",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Publisher",
                table: "StreamCategories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RawgSlug",
                table: "StreamCategories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "StreamCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScreenshotsJson",
                table: "StreamCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "StreamCategories",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SteamAppId",
                table: "StreamCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SteamReviewCount",
                table: "StreamCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SteamReviewScore",
                table: "StreamCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SteamStoreUrl",
                table: "StreamCategories",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SteamTags",
                table: "StreamCategories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerVideoUrl",
                table: "StreamCategories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "StreamCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveStreamsCount",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "BackgroundImageUrl",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "BannerImageUrl",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "CurrentViewers",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "Developer",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "GameplayVideoUrl",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "GeforceNowId",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "Genres",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "IconUrl",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "IgdbId",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "IsFreeToPlay",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "MetacriticScore",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "OfficialWebsiteUrl",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "Platforms",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "PopularityRank",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "Publisher",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "RawgSlug",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "ScreenshotsJson",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "SteamAppId",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "SteamReviewCount",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "SteamReviewScore",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "SteamStoreUrl",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "SteamTags",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "TrailerVideoUrl",
                table: "StreamCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "StreamCategories");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "StreamCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
