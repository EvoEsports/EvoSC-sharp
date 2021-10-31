using System;
using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Infrastructure;

namespace EvoSC.Migrations
{
    public static class MigratorExtensions
    {
        public static IFluentSyntax CreateTableIfNotExists(this MigrationBase self, string tableName,
            Func<ICreateTableWithColumnOrSchemaOrDescriptionSyntax, IFluentSyntax> constructTableFunction, string schemaName = "dbo")
        {
            if (!self.Schema.Schema(schemaName).Table(tableName).Exists())
            {
                return constructTableFunction(self.Create.Table(tableName));
            }
            else
            {
                return null;
            }
        }
    }
}
