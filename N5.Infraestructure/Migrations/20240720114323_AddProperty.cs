using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace N5.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "FechaPermiso",
                table: "Permisos",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "TiposPermisoId",
                table: "Permisos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_TiposPermisoId",
                table: "Permisos",
                column: "TiposPermisoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permisos_TiposPermisos_TiposPermisoId",
                table: "Permisos",
                column: "TiposPermisoId",
                principalTable: "TiposPermisos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permisos_TiposPermisos_TiposPermisoId",
                table: "Permisos");

            migrationBuilder.DropIndex(
                name: "IX_Permisos_TiposPermisoId",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "TiposPermisoId",
                table: "Permisos");

            migrationBuilder.AlterColumn<string>(
                name: "FechaPermiso",
                table: "Permisos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
