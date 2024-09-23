using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPeliculas.Migrations
{
    /// <inheritdoc />
    public partial class TablaUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pelicula_Categorias_CategoriaId",
                table: "Pelicula");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "Pelicula",
                newName: "categoriaId");

            migrationBuilder.RenameIndex(
                name: "IX_Pelicula_CategoriaId",
                table: "Pelicula",
                newName: "IX_Pelicula_categoriaId");

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Pelicula_Categorias_categoriaId",
                table: "Pelicula",
                column: "categoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pelicula_Categorias_categoriaId",
                table: "Pelicula");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.RenameColumn(
                name: "categoriaId",
                table: "Pelicula",
                newName: "CategoriaId");

            migrationBuilder.RenameIndex(
                name: "IX_Pelicula_categoriaId",
                table: "Pelicula",
                newName: "IX_Pelicula_CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pelicula_Categorias_CategoriaId",
                table: "Pelicula",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
