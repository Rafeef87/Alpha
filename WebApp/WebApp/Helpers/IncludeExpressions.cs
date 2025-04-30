using System.Linq.Expressions;

namespace WebApp.Helpers;

public static class IncludeExpressions
{
    public static Expression<Func<TEntity, object>>[] For<TEntity>(
        params Expression<Func<TEntity, object>>[] includes)
    {
        return includes;
    }
}
