using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Asakabank.UserApi.Migrations
{
    public partial class AddedUserSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Firstname", "IsDeleted", "IsIdentified", "Lastname", "Middlename", "OTP", "OTPSentTime", "Passport", "Password", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { new Guid("0ebae847-3b98-4372-93a8-1499f21f3ae8"), new DateTime(2021, 5, 25, 15, 4, 59, 934, DateTimeKind.Local).AddTicks(978), null, false, false, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "$2a$11$TNpoNe8DmA3jvddpKTE/7O/NpbusaIzaM10KZ3RFxBrIivviAHQHG", null, "test1" },
                    { new Guid("d4dd5488-22b3-42cc-8119-e5329aeb7770"), new DateTime(2021, 5, 25, 15, 5, 0, 104, DateTimeKind.Local).AddTicks(7091), null, false, false, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "$2a$11$kAJo7B8Ob4sYTjdhkY/Re.TVc/9mwYhzlywpMBaGj.e3qnEOOxFde", null, "test2" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0ebae847-3b98-4372-93a8-1499f21f3ae8"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4dd5488-22b3-42cc-8119-e5329aeb7770"));
        }
    }
}
