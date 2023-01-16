using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using RepoDb;
using RepoDb.Attributes;
using RepoDb.Attributes.Parameter;
using RepoDb.Enumerations;
using RepoDb.Extensions;
using RepoDb.Interfaces;

namespace EvoSC.Common.Database.Extensions;

public static class QueryBuilderExtensions
{
    /// <summary>
    /// Select everything from a table. Uses * instead of column names.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="dbSetting"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static QueryBuilder SelectAllFrom<TEntity>(this QueryBuilder builder, IDbSetting dbSetting)
        where TEntity : class
        => builder
            .Select()
            .WriteText("*")
            .From()
            .TableNameFrom<TEntity>(dbSetting);

    /// <summary>
    /// Shortcut for selecting all fields from a table.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="dbSetting"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static QueryBuilder SelectFieldsFrom<TEntity>(this QueryBuilder builder, IDbSetting dbSetting)
        where TEntity : class
        => builder
            .Select()
            .FieldsFrom(FieldCache.Get<TEntity>(), dbSetting)
            .From()
            .TableNameFrom<TEntity>(dbSetting);

    /// <summary>
    /// Select a single field from a table.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="propertyExpr"></param>
    /// <param name="dbSetting"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static QueryBuilder SelectFieldFrom<TEntity>(this QueryBuilder builder,
        Expression<Func<TEntity, object>> propertyExpr,
        IDbSetting dbSetting) where TEntity : class
    {
        var property = (MemberExpression)((UnaryExpression)propertyExpr.Body).Operand;
        var field = new Field(GetMemberName(property.Member), property.Member.DeclaringType);

        return builder
            .Select()
            .FieldFrom(field, dbSetting)
            .From()
            .TableNameFrom<TEntity>(dbSetting);
    }
    
    public static QueryBuilder FieldFrom<TEntity>(this QueryBuilder builder,
        Expression<Func<TEntity, object>> propertyExpr,
        IDbSetting dbSetting) where TEntity : class
    {
        var property = (MemberExpression)((UnaryExpression)propertyExpr.Body).Operand;
        var field = new Field(GetMemberName(property.Member), property.Member.DeclaringType);

        return builder.FieldFrom(field, dbSetting);
    }

    /// <summary>
    /// Add a single where clause to the query.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="propertyExpression"></param>
    /// <param name="dbSetting"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static QueryBuilder Where<TEntity>(this QueryBuilder builder,
        Expression<Func<TEntity, bool>> propertyExpression, IDbSetting dbSetting) where TEntity : class
    {
        var body = (BinaryExpression)propertyExpression.Body;
        var left = (MemberExpression)body.Left;
        var field = new Field(GetMemberName(left.Member), left.Member.DeclaringType);
        var queryField = new QueryField(field.Name, ToDbOperation(body.NodeType), body.Right.GetValue());
        return builder.WhereFrom(new QueryGroup(queryField), dbSetting);
    }

    /// <summary>
    /// Add a WHERE IN part to the query.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="propertyExpr"></param>
    /// <param name="dbSetting"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static QueryBuilder WhereIn<TEntity>(this QueryBuilder builder,
        Expression<Func<TEntity, object>> propertyExpr,
        IDbSetting dbSetting) where TEntity : class
    {
        var property = (MemberExpression)((UnaryExpression)propertyExpr.Body).Operand;
        var field = new Field(GetMemberName(property.Member), property.Member.DeclaringType);
        return builder
            .Where()
            .FieldFrom(field, dbSetting)
            .In();
    }

    /// <summary>
    /// Add a delete from table to the query.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="dbSetting"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static QueryBuilder DeleteFrom<TEntity>(this QueryBuilder builder, IDbSetting dbSetting)
        where TEntity : class
        => builder
            .Delete()
            .From()
            .TableNameFrom<TEntity>(dbSetting);

    /// <summary>
    /// Adds "INNER JOIN" to the query.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static QueryBuilder InnerJoin(this QueryBuilder builder) => builder.WriteText("INNER").Join();
    
    /// <summary>
    /// Adds "LEFT JOIN" to the query.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static QueryBuilder LeftJoin(this QueryBuilder builder) => builder.WriteText("LEFT").Join();
    
    /// <summary>
    /// Adds "RIGHT JOIN" to the query
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static QueryBuilder RightJoin(this QueryBuilder builder) => builder.WriteText("RIGHT").Join();
    
    /// <summary>
    /// Adds "OUTER JOIN" to the query
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static QueryBuilder OuterJoin(this QueryBuilder builder) => builder.WriteText("OUTER").Join();

    public static string QuoteIdentifier(string name, IDbSetting dbSetting) =>
        $"{dbSetting.OpeningQuote}{name}{dbSetting.ClosingQuote}";

    private static string GetMemberName(this MemberInfo member) =>
        member.GetCustomAttribute<MapAttribute>()?.Name
        ?? member.GetCustomAttribute<NameAttribute>()?.Name
        ?? member.GetCustomAttribute<ColumnAttribute>()?.Name
        ?? member.Name;

    private static Operation ToDbOperation(ExpressionType expressionType) => expressionType
        switch
        {
            ExpressionType.Equal => Operation.Equal,
            ExpressionType.NotEqual => Operation.NotEqual,
            ExpressionType.LessThan => Operation.LessThan,
            ExpressionType.GreaterThan => Operation.GreaterThan,
            ExpressionType.LessThanOrEqual => Operation.LessThanOrEqual,
            ExpressionType.GreaterThanOrEqual => Operation.GreaterThanOrEqual,
            _ => Operation.Equal
        };
}
