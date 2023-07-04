using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Viho.web.Migrations
{
    /// <inheritdoc />
    public partial class CreateRemindersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_expenses",
                columns: table => new
                {
                    e_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    e_cat = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    e_date = table.Column<DateTime>(type: "date", nullable: false),
                    e_amount = table.Column<double>(type: "float", nullable: false),
                    e_detail = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_expenses", x => x.e_id);
                });

            migrationBuilder.CreateTable(
                name: "tb_location",
                columns: table => new
                {
                    l_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    l_address = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_location", x => x.l_id);
                });

            migrationBuilder.CreateTable(
                name: "tb_role",
                columns: table => new
                {
                    rl_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rl_desc = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_role", x => x.rl_id);
                });

            migrationBuilder.CreateTable(
                name: "tb_attendance",
                columns: table => new
                {
                    att_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    att_lid = table.Column<int>(type: "int", nullable: false),
                    att_date = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_attendance", x => x.att_id);
                    table.ForeignKey(
                        name: "FK_tb_attendance_tb_location",
                        column: x => x.att_lid,
                        principalTable: "tb_location",
                        principalColumn: "l_id");
                });

            migrationBuilder.CreateTable(
                name: "tb_inventory",
                columns: table => new
                {
                    i_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    i_locationid = table.Column<int>(type: "int", nullable: false),
                    i_item = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    i_quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_inventory", x => x.i_id);
                    table.ForeignKey(
                        name: "FK_tb_inventory_tb_location",
                        column: x => x.i_locationid,
                        principalTable: "tb_location",
                        principalColumn: "l_id");
                });

            migrationBuilder.CreateTable(
                name: "tb_room",
                columns: table => new
                {
                    r_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    r_no = table.Column<int>(type: "int", nullable: false),
                    r_locationid = table.Column<int>(type: "int", nullable: false),
                    r_type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    r_price = table.Column<double>(type: "float", nullable: false),
                    r_depositAmount = table.Column<double>(type: "float", nullable: false),
                    r_desc = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    r_status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    r_img1 = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    r_img2 = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    r_img3 = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    r_img4 = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    r_img5 = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_room", x => x.r_id);
                    table.ForeignKey(
                        name: "FK_tb_room_tb_location",
                        column: x => x.r_locationid,
                        principalTable: "tb_location",
                        principalColumn: "l_id");
                });

            migrationBuilder.CreateTable(
                name: "tb_user",
                columns: table => new
                {
                    u_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    u_username = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    u_pass = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    u_phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    u_email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    u_roleid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_user", x => x.u_id);
                    table.ForeignKey(
                        name: "FK_tb_user_tb_role",
                        column: x => x.u_roleid,
                        principalTable: "tb_role",
                        principalColumn: "rl_id");
                });

            migrationBuilder.CreateTable(
                name: "tb_tenants",
                columns: table => new
                {
                    t_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    t_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    t_ic = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    t_phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    t_address = table.Column<string>(type: "text", nullable: false),
                    t_uploadIC = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    t_emerContact = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    t_roomid = table.Column<int>(type: "int", nullable: false),
                    t_entrydate = table.Column<DateTime>(type: "datetime", nullable: false),
                    t_exitdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    t_cardCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    t_addOnDetail = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    t_paymentStatus = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    t_agreement = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    t_LastReminderDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tenants", x => x.t_id);
                    table.ForeignKey(
                        name: "FK_tb_tenants_tb_room",
                        column: x => x.t_roomid,
                        principalTable: "tb_room",
                        principalColumn: "r_id");
                });

            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ReminderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reminders_tb_tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "tb_tenants",
                        principalColumn: "t_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_payment",
                columns: table => new
                {
                    p_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    p_tenantid = table.Column<int>(type: "int", nullable: false),
                    p_date = table.Column<DateTime>(type: "date", nullable: false),
                    p_amount = table.Column<double>(type: "float", nullable: false),
                    p_type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_payment", x => x.p_id);
                    table.ForeignKey(
                        name: "FK_tb_payment_tb_tenants",
                        column: x => x.p_tenantid,
                        principalTable: "tb_tenants",
                        principalColumn: "t_id");
                });

            migrationBuilder.CreateTable(
                name: "tb_sales",
                columns: table => new
                {
                    s_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    s_paymentid = table.Column<int>(type: "int", nullable: false),
                    s_exid = table.Column<int>(type: "int", nullable: false),
                    s_profitamount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_sales", x => x.s_id);
                    table.ForeignKey(
                        name: "FK_tb_sales_tb_expenses",
                        column: x => x.s_exid,
                        principalTable: "tb_expenses",
                        principalColumn: "e_id");
                    table.ForeignKey(
                        name: "FK_tb_sales_tb_payment",
                        column: x => x.s_paymentid,
                        principalTable: "tb_payment",
                        principalColumn: "p_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_TenantId",
                table: "Reminders",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_attendance_att_lid",
                table: "tb_attendance",
                column: "att_lid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_inventory_i_locationid",
                table: "tb_inventory",
                column: "i_locationid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_payment_p_tenantid",
                table: "tb_payment",
                column: "p_tenantid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_room_r_locationid",
                table: "tb_room",
                column: "r_locationid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sales_s_exid",
                table: "tb_sales",
                column: "s_exid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_sales_s_paymentid",
                table: "tb_sales",
                column: "s_paymentid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tenants_t_roomid",
                table: "tb_tenants",
                column: "t_roomid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_user_u_roleid",
                table: "tb_user",
                column: "u_roleid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reminders");

            migrationBuilder.DropTable(
                name: "tb_attendance");

            migrationBuilder.DropTable(
                name: "tb_inventory");

            migrationBuilder.DropTable(
                name: "tb_sales");

            migrationBuilder.DropTable(
                name: "tb_user");

            migrationBuilder.DropTable(
                name: "tb_expenses");

            migrationBuilder.DropTable(
                name: "tb_payment");

            migrationBuilder.DropTable(
                name: "tb_role");

            migrationBuilder.DropTable(
                name: "tb_tenants");

            migrationBuilder.DropTable(
                name: "tb_room");

            migrationBuilder.DropTable(
                name: "tb_location");
        }
    }
}
