using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppGestionGastos.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AuditoriaEntidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreadoEn",
                table: "TiposMovimiento",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)))
                .Annotation("Relational:ColumnOrder", 1000);

            migrationBuilder.AddColumn<Guid>(
                name: "CreadoPor",
                table: "TiposMovimiento",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1001);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModificadoEn",
                table: "TiposMovimiento",
                type: "datetimeoffset",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1002);

            migrationBuilder.AddColumn<Guid>(
                name: "ModificadoPor",
                table: "TiposMovimiento",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1003);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreadoEn",
                table: "TiposCuenta",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)))
                .Annotation("Relational:ColumnOrder", 1000);

            migrationBuilder.AddColumn<Guid>(
                name: "CreadoPor",
                table: "TiposCuenta",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1001);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModificadoEn",
                table: "TiposCuenta",
                type: "datetimeoffset",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1002);

            migrationBuilder.AddColumn<Guid>(
                name: "ModificadoPor",
                table: "TiposCuenta",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1003);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreadoEn",
                table: "Movimientos",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)))
                .Annotation("Relational:ColumnOrder", 1000);

            migrationBuilder.AddColumn<Guid>(
                name: "CreadoPor",
                table: "Movimientos",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1001);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModificadoEn",
                table: "Movimientos",
                type: "datetimeoffset",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1002);

            migrationBuilder.AddColumn<Guid>(
                name: "ModificadoPor",
                table: "Movimientos",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1003);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreadoEn",
                table: "Cuentas",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)))
                .Annotation("Relational:ColumnOrder", 1000);

            migrationBuilder.AddColumn<Guid>(
                name: "CreadoPor",
                table: "Cuentas",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1001);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModificadoEn",
                table: "Cuentas",
                type: "datetimeoffset",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1002);

            migrationBuilder.AddColumn<Guid>(
                name: "ModificadoPor",
                table: "Cuentas",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1003);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreadoEn",
                table: "Categorias",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)))
                .Annotation("Relational:ColumnOrder", 1000);

            migrationBuilder.AddColumn<Guid>(
                name: "CreadoPor",
                table: "Categorias",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1001);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModificadoEn",
                table: "Categorias",
                type: "datetimeoffset",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1002);

            migrationBuilder.AddColumn<Guid>(
                name: "ModificadoPor",
                table: "Categorias",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1003);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreadoEn",
                table: "Bancos",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)))
                .Annotation("Relational:ColumnOrder", 1000);

            migrationBuilder.AddColumn<Guid>(
                name: "CreadoPor",
                table: "Bancos",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1001);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModificadoEn",
                table: "Bancos",
                type: "datetimeoffset",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1002);

            migrationBuilder.AddColumn<Guid>(
                name: "ModificadoPor",
                table: "Bancos",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 1003);

            migrationBuilder.UpdateData(
                table: "TiposMovimiento",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-7000-8000-000000000001"),
                columns: new[] { "CreadoEn", "CreadoPor", "ModificadoEn", "ModificadoPor" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null });

            migrationBuilder.UpdateData(
                table: "TiposMovimiento",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-7000-8000-000000000002"),
                columns: new[] { "CreadoEn", "CreadoPor", "ModificadoEn", "ModificadoPor" },
                values: new object[] { new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "TiposMovimiento");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "TiposMovimiento");

            migrationBuilder.DropColumn(
                name: "ModificadoEn",
                table: "TiposMovimiento");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "TiposMovimiento");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "TiposCuenta");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "TiposCuenta");

            migrationBuilder.DropColumn(
                name: "ModificadoEn",
                table: "TiposCuenta");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "TiposCuenta");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Movimientos");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Movimientos");

            migrationBuilder.DropColumn(
                name: "ModificadoEn",
                table: "Movimientos");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "Movimientos");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Cuentas");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Cuentas");

            migrationBuilder.DropColumn(
                name: "ModificadoEn",
                table: "Cuentas");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "Cuentas");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "ModificadoEn",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "CreadoEn",
                table: "Bancos");

            migrationBuilder.DropColumn(
                name: "CreadoPor",
                table: "Bancos");

            migrationBuilder.DropColumn(
                name: "ModificadoEn",
                table: "Bancos");

            migrationBuilder.DropColumn(
                name: "ModificadoPor",
                table: "Bancos");
        }
    }
}
