using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Utilities.Extensions;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> JoinWithOr<T>(this IReadOnlyList<Expression<Func<T, bool>>> expressions)
    {
        if (expressions is null) throw new ArgumentNullException(nameof(expressions));

        var leftExpression = expressions[0];
        foreach (var rightExpression in expressions.Skip(1)) leftExpression = leftExpression.OrElse(rightExpression);

        return leftExpression;
    }

    internal static Expression<Func<T, bool>> True<T>()
    {
        return Predicate<T>.True;
    }

    internal static Expression<Func<T, bool>> False<T>()
    {
        return Predicate<T>.False;
    }

    internal static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        if (Equals(left, right)) return left;

        var body = Expression.OrElse(left.Body, right.Body.Replace(right.Parameters[0], left.Parameters[0]));
        return Expression.Lambda<Func<T, bool>>(body, left.Parameters);
    }

    private static Expression Replace(this Expression expression, Expression source, Expression target)
    {
        return new ExpressionReplacer(source, target).Visit(expression);
    }

    internal class ExpressionReplacer : ExpressionVisitor
    {
        public ExpressionReplacer(Expression source, Expression target)
        {
            Source = source;
            Target = target;
        }

        public Expression Source { get; }

        public Expression Target { get; }

        [return: NotNullIfNotNull("node")]
        public override Expression? Visit(Expression? node)
        {
            if (node is null) return null;

            return node == Source ? Target : base.Visit(node);
        }
    }

    private static class Predicate<T>
    {
        public static readonly Expression<Func<T, bool>> True = item => true;
        public static readonly Expression<Func<T, bool>> False = item => false;
    }
}
