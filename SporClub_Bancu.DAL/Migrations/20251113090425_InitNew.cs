using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporClub_Bancu.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InventoryDbId",
                table: "pictures_inventory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserDbId",
                table: "orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryDbId",
                table: "inventory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "InventoryDbOrdersDb",
                columns: table => new
                {
                    InventoryDbId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrdersDbId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryDbOrdersDb", x => new { x.InventoryDbId, x.OrdersDbId });
                    table.ForeignKey(
                        name: "FK_InventoryDbOrdersDb_inventory_InventoryDbId",
                        column: x => x.InventoryDbId,
                        principalTable: "inventory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryDbOrdersDb_orders_OrdersDbId",
                        column: x => x.OrdersDbId,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pictures_inventory_InventoryDbId",
                table: "pictures_inventory",
                column: "InventoryDbId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_UserDbId",
                table: "orders",
                column: "UserDbId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_CategoryDbId",
                table: "inventory",
                column: "CategoryDbId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDbOrdersDb_OrdersDbId",
                table: "InventoryDbOrdersDb",
                column: "OrdersDbId");

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_categories_CategoryDbId",
                table: "inventory",
                column: "CategoryDbId",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_user_UserDbId",
                table: "orders",
                column: "UserDbId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_pictures_inventory_inventory_InventoryDbId",
                table: "pictures_inventory",
                column: "InventoryDbId",
                principalTable: "inventory",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventory_categories_CategoryDbId",
                table: "inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_user_UserDbId",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_pictures_inventory_inventory_InventoryDbId",
                table: "pictures_inventory");

            migrationBuilder.DropTable(
                name: "InventoryDbOrdersDb");

            migrationBuilder.DropIndex(
                name: "IX_pictures_inventory_InventoryDbId",
                table: "pictures_inventory");

            migrationBuilder.DropIndex(
                name: "IX_orders_UserDbId",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_inventory_CategoryDbId",
                table: "inventory");

            migrationBuilder.DropColumn(
                name: "InventoryDbId",
                table: "pictures_inventory");

            migrationBuilder.DropColumn(
                name: "UserDbId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "CategoryDbId",
                table: "inventory");
        }
    }
}
