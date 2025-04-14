using System.Linq.Expressions;

namespace Business.Helpers;

public static class IncludeExpressions
{
    public static Expression<Func<TEntity, object>>[] For<TEntity>(
        params Expression<Func<TEntity, object>>[] includes)
    {
        return includes;
    }
}
