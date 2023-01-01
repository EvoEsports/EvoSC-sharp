using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;

namespace EvoSC.Common.Database;

/// <summary>
/// To support several database types, the EvoSCMigration class will ensure that database tables will be created
/// according to the given database type.
/// </summary>
public abstract class EvoSCMigration : Migration
{
    public override void GetUpExpressions(IMigrationContext context)
    {
        base.GetUpExpressions(context);
        var dbType = context.QuerySchema.DatabaseType;
        switch (dbType)
        {
            case "Postgres":
                ConvertDbInfoToLowercase(context);
                break;
        }
    }

    /// <summary>
    /// Converts all table names and columns within the database migration to lowercase.
    /// </summary>
    /// <param name="context"></param>
    private void ConvertDbInfoToLowercase(IMigrationContext context)
    {
        foreach (var expr in context.Expressions)
        {
            if (expr is CreateTableExpression tablexpr)
            {
                foreach (var column in tablexpr.Columns)
                {
                    column.Name = column.Name.ToLower();
                    column.TableName = column.TableName.ToLower();
                }

                tablexpr.TableName = tablexpr.TableName.ToLower();
            }
            else if (expr is CreateIndexExpression indexExpr)
            {
                indexExpr.Index.TableName = indexExpr.Index.TableName.ToLower();
                foreach (var column in indexExpr.Index.Columns)
                {
                    column.Name = column.Name.ToLower();
                }
            }
            else if (expr is CreateForeignKeyExpression foreignKeyExpr)
            {
                var foreignKey = foreignKeyExpr.ForeignKey;
                foreignKey.PrimaryTable = foreignKey.PrimaryTable.ToLower();
                foreignKey.ForeignTable = foreignKey.ForeignTable.ToLower();
                foreignKey.PrimaryColumns =
                    foreignKey.PrimaryColumns.Select(e => e.ToLower()).ToList();
                foreignKey.ForeignColumns =
                    foreignKey.ForeignColumns.Select(e => e.ToLower()).ToList();
            }
            else if (expr is AlterTableExpression tableExpr)
            {
                tableExpr.TableName = tableExpr.TableName.ToLower();
            }
            else if (expr is AlterColumnExpression alterColumnExpr)
            {
                alterColumnExpr.TableName = alterColumnExpr.TableName.ToLower();
                alterColumnExpr.Column.TableName = alterColumnExpr.Column.TableName.ToLower();
                alterColumnExpr.Column.Name = alterColumnExpr.Column.Name.ToLower();
            }
            else if (expr is DeleteTableExpression deleteTableExpr)
            {
                deleteTableExpr.TableName = deleteTableExpr.TableName.ToLower();
            }
            else if (expr is DeleteColumnExpression deleteColumnExpr)
            {
                deleteColumnExpr.TableName = deleteColumnExpr.TableName.ToLower();
                deleteColumnExpr.ColumnNames = deleteColumnExpr.ColumnNames.Select(e => e.ToLower()).ToList();
            }
        }
    }
}
