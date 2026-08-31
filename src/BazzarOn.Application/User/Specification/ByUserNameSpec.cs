using Ardalis.Specification;
using BazzarOn.Domain.ValueObjects;

namespace BazzarOn.Application.User.Specification;

public class ByUserNameSpec : Specification<BazzarOn.Domain.Entities.User>
{
    public ByUserNameSpec(string userName, bool asnotracking = false)
    {
        if (asnotracking)
            Query.AsNoTracking();

        var parsedUsername = new Username(userName);

        Query.Where(x => x.Username == parsedUsername && !x.IsDeleted);
    }
}