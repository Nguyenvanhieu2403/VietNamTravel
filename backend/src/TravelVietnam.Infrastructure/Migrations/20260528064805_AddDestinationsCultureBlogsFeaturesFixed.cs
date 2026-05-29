using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelVietnam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDestinationsCultureBlogsFeaturesFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Users_AuthorId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_Provinces_ProvinceId",
                table: "Destinations");

            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_Regions_RegionId",
                table: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_AuthorId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "EntryFee",
                table: "Destinations");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Destinations",
                newName: "ThumbnailUrl");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Blogs",
                newName: "IsFeatured");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Blogs",
                newName: "ViewCount");

            migrationBuilder.AddColumn<int>(
                name: "CultureId",
                table: "MediaFiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BannerUrl",
                table: "Destinations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BestTimeToVisit",
                table: "Destinations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Destinations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EstimatedBudget",
                table: "Destinations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Destinations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Destinations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Destinations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Destinations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Destinations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BannerUrl",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReadTime",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cultures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BannerUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionId = table.Column<int>(type: "int", nullable: true),
                    CultureType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FestivalSeason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cultures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cultures_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_CultureId",
                table: "MediaFiles",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_RegionId",
                table: "Destinations",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_Slug",
                table: "Destinations",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UserId",
                table: "Blogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cultures_RegionId",
                table: "Cultures",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Cultures_Slug",
                table: "Cultures",
                column: "Slug",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Users_UserId",
                table: "Blogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_Provinces_ProvinceId",
                table: "Destinations",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_Regions_RegionId",
                table: "Destinations",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_Cultures_CultureId",
                table: "MediaFiles",
                column: "CultureId",
                principalTable: "Cultures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_Regions_RegionId",
                table: "Provinces",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Users_UserId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_Provinces_ProvinceId",
                table: "Destinations");

            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_Regions_RegionId",
                table: "Destinations");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_Cultures_CultureId",
                table: "MediaFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_Regions_RegionId",
                table: "Provinces");

            migrationBuilder.DropTable(
                name: "Cultures");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_CultureId",
                table: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_Destinations_RegionId",
                table: "Destinations");

            migrationBuilder.DropIndex(
                name: "IX_Destinations_Slug",
                table: "Destinations");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_UserId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "CultureId",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "BannerUrl",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "BestTimeToVisit",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "EstimatedBudget",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "BannerUrl",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ReadTime",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "ThumbnailUrl",
                table: "Destinations",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "ViewCount",
                table: "Blogs",
                newName: "AuthorId");

            migrationBuilder.RenameColumn(
                name: "IsFeatured",
                table: "Blogs",
                newName: "IsPublished");

            migrationBuilder.AddColumn<decimal>(
                name: "EntryFee",
                table: "Destinations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_AuthorId",
                table: "Blogs",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Users_AuthorId",
                table: "Blogs",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_Provinces_ProvinceId",
                table: "Destinations",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_Regions_RegionId",
                table: "Provinces",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
