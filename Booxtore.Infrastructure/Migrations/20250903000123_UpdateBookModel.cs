using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booxtore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    author_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    biography = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Authors__86516BCFD54FF63F", x => x.author_id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__D54EE9B4B8732378", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__B9BE370F7EAC11EC", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    book_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    author_id = table.Column<int>(type: "int", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    isbn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: true, defaultValue: 0.00m),
                    is_free = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    is_available_for_borrow = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    is_available_for_purchase = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    total_copies = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    available_copies = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    cover_image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pdf_file_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pages = table.Column<int>(type: "int", nullable: true),
                    publication_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "active"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Books__490D1AE12ED3F536", x => x.book_id);
                    table.ForeignKey(
                        name: "FK__Books__author_id__45F365D3",
                        column: x => x.author_id,
                        principalTable: "Authors",
                        principalColumn: "author_id");
                    table.ForeignKey(
                        name: "FK__Books__category___46E78A0C",
                        column: x => x.category_id,
                        principalTable: "Categories",
                        principalColumn: "category_id");
                });

            migrationBuilder.CreateTable(
                name: "Admin_Actions",
                columns: table => new
                {
                    action_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    admin_id = table.Column<int>(type: "int", nullable: true),
                    action_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    target_table = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    target_id = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Admin_Ac__74EFC21708F2B4D2", x => x.action_id);
                    table.ForeignKey(
                        name: "FK__Admin_Act__admin__7B5B524B",
                        column: x => x.admin_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Purchase_Orders",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    total_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true, defaultValue: 0.00m),
                    payment_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "pending"),
                    payment_method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    order_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    completed_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__465962295194A6D3", x => x.order_id);
                    table.ForeignKey(
                        name: "FK__Purchase___user___628FA481",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Borrowing_Records",
                columns: table => new
                {
                    borrow_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    book_id = table.Column<int>(type: "int", nullable: true),
                    borrow_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    due_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    return_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "active"),
                    fine_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true, defaultValue: 0.00m),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Borrowin__262B57A0468725A1", x => x.borrow_id);
                    table.ForeignKey(
                        name: "FK__Borrowing__book___59FA5E80",
                        column: x => x.book_id,
                        principalTable: "Books",
                        principalColumn: "book_id");
                    table.ForeignKey(
                        name: "FK__Borrowing__user___59063A47",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Reading_Sessions",
                columns: table => new
                {
                    session_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    book_id = table.Column<int>(type: "int", nullable: true),
                    start_time = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    end_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    pages_read = table.Column<int>(type: "int", nullable: true),
                    last_page = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reading___69B13FDC7C10D54A", x => x.session_id);
                    table.ForeignKey(
                        name: "FK__Reading_S__book___6E01572D",
                        column: x => x.book_id,
                        principalTable: "Books",
                        principalColumn: "book_id");
                    table.ForeignKey(
                        name: "FK__Reading_S__user___6D0D32F4",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    review_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    book_id = table.Column<int>(type: "int", nullable: true),
                    rating = table.Column<int>(type: "int", nullable: true),
                    review_text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "active"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reviews__60883D90D7885EF3", x => x.review_id);
                    table.ForeignKey(
                        name: "FK__Reviews__book_id__73BA3083",
                        column: x => x.book_id,
                        principalTable: "Books",
                        principalColumn: "book_id");
                    table.ForeignKey(
                        name: "FK__Reviews__user_id__72C60C4A",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "User_Libraries",
                columns: table => new
                {
                    library_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    book_id = table.Column<int>(type: "int", nullable: true),
                    access_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    purchase_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User_Lib__7A2F73CAFAFDDE1A", x => x.library_id);
                    table.ForeignKey(
                        name: "FK__User_Libr__book___5441852A",
                        column: x => x.book_id,
                        principalTable: "Books",
                        principalColumn: "book_id");
                    table.ForeignKey(
                        name: "FK__User_Libr__user___534D60F1",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Order_Items",
                columns: table => new
                {
                    item_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<int>(type: "int", nullable: true),
                    book_id = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    total_price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order_It__52020FDD188DDE3A", x => x.item_id);
                    table.ForeignKey(
                        name: "FK__Order_Ite__book___6A30C649",
                        column: x => x.book_id,
                        principalTable: "Books",
                        principalColumn: "book_id");
                    table.ForeignKey(
                        name: "FK__Order_Ite__order__693CA210",
                        column: x => x.order_id,
                        principalTable: "Purchase_Orders",
                        principalColumn: "order_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admin_Actions_admin_id",
                table: "Admin_Actions",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_Books_author_id",
                table: "Books",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_Books_category_id",
                table: "Books",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Books__99F9D0A4E5DE0733",
                table: "Books",
                column: "isbn",
                unique: true,
                filter: "[isbn] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowing_Records_book_id",
                table: "Borrowing_Records",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowing_Records_user_id",
                table: "Borrowing_Records",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Categori__72E12F1BFCF86850",
                table: "Categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_Items_book_id",
                table: "Order_Items",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Items_order_id",
                table: "Order_Items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_Orders_user_id",
                table: "Purchase_Orders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reading_Sessions_book_id",
                table: "Reading_Sessions",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reading_Sessions_user_id",
                table: "Reading_Sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_book_id",
                table: "Reviews",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_user_id",
                table: "Reviews",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Libraries_book_id",
                table: "User_Libraries",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Libraries_user_id",
                table: "User_Libraries",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__AB6E61640F074CEA",
                table: "Users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin_Actions");

            migrationBuilder.DropTable(
                name: "Borrowing_Records");

            migrationBuilder.DropTable(
                name: "Order_Items");

            migrationBuilder.DropTable(
                name: "Reading_Sessions");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "User_Libraries");

            migrationBuilder.DropTable(
                name: "Purchase_Orders");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
