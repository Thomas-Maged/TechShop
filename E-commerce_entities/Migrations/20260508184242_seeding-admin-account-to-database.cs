using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce_entities.Migrations
{
    /// <inheritdoc />
    public partial class seedingadminaccounttodatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "Admin1ID", 0, "3fde117d-1fda-40d0-b689-afb7cc0e018f", "admin@techshop.com", true, "Admin", false, null, "ADMIN@TECHSHOP.COM", "ADMIN@TECHSHOP.COM", "AQAAAAIAAYagAAAAEM08SPSwbToCty+F3Zb5Nk6zZlEDJpaeefrV/uOBgMC+7EVmp3N+l8KUCm2Vw+gj3A==", null, false, "bd676dce-86ea-4337-9300-90f869ea5b94", false, "admin@techshop.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "Admin1ID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "Admin1ID" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "Admin1ID");
        }
    }
}
