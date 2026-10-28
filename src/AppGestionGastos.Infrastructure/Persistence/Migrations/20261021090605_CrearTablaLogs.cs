using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppGestionGastos.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
        IF OBJECT_ID(N'[dbo].[Logs]', N'U') IS NULL
        BEGIN
            CREATE TABLE [dbo].[Logs]
            (
                [Id]              BIGINT IDENTITY(1,1) NOT NULL,
                [TimeStamp]       DATETIME2(7)  NOT NULL,
                [Level]           NVARCHAR(16)  NOT NULL,
                [Message]         NVARCHAR(MAX) NULL,
                [MessageTemplate] NVARCHAR(MAX) NULL,
                [Exception]       NVARCHAR(MAX) NULL,
                [LogEvent]        NVARCHAR(MAX) NULL,
                [Application]     NVARCHAR(100) NULL,
                [TraceId]         NVARCHAR(64)  NULL,
                CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED ([Id])
            );

            CREATE INDEX [IX_Logs_TimeStamp]       ON [dbo].[Logs] ([TimeStamp]);
            CREATE INDEX [IX_Logs_Level_TimeStamp] ON [dbo].[Logs] ([Level], [TimeStamp]);
        END
        """);
        }

        /// <inheritdoc />

        protected override void Down(MigrationBuilder migrationBuilder) =>
            migrationBuilder.Sql("IF OBJECT_ID(N'[dbo].[Logs]', N'U') IS NOT NULL DROP TABLE [dbo].[Logs];");
    }
}
