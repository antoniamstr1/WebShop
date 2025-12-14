using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CartId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Shipped = table.Column<bool>(type: "boolean", nullable: false),
                    Arrived = table.Column<bool>(type: "boolean", nullable: false),
                    Paid = table.Column<bool>(type: "boolean", nullable: false),
                    AddressId = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AlterColumn<decimal>(
        name: "Price",
        table: "Carts",
        type: "decimal(18,2)",
        nullable: false,
        oldClrType: typeof(int),
        oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Carts",
                nullable: true,
                oldClrType: typeof(int));


            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CartId",
                table: "Orders",
                column: "CartId");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.AlterColumn<int>(
        name: "Price",
        table: "Carts",
        type: "int",
        nullable: false,
        oldClrType: typeof(decimal),
        oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Carts",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);


        }
    }
}
