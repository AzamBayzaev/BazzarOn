using Ardalis.Specification;

namespace BazzarOn.Mediator.Helper.Persistence;

public class ReadOnlySpecification<T> : Specification<T> where T : class
{
    public ReadOnlySpecification() => Query.AsNoTracking();
}

public class ReadOnlySpecification<T, TResult> : Specification<T, TResult> where T : class
{
    public ReadOnlySpecification() => Query.AsNoTracking();
}   