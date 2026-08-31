using Ardalis.Specification;

namespace BazzarOn.Mediator.Helper.Persistence;

public class DbSpecification<T> : Specification<T> where T : class;

public class DbSpecification<T, TResult> : Specification<T, TResult> where T : class;