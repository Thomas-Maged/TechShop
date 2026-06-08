using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce_entities.Migrations
{
    /// <inheritdoc />
    public partial class FinalizeSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "Admin1ID",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "7e028918-848d-4684-b39b-b845af9ebb06", "0decee54-464e-41fe-baf6-7684de7ebd97" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "Admin1ID",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3fde117d-1fda-40d0-b689-afb7cc0e018f", "bd676dce-86ea-4337-9300-90f869ea5b94" });
        }
    }
}
