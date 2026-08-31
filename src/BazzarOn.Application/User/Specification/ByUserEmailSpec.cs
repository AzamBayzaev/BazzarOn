using Ardalis.Specification;
using BazzarOn.Domain.ValueObjects;

namespace BazzarOn.Application.User.Specification;

public class ByUserEmailSpec : Specification<BazzarOn.Domain.Entities.User>
{
    public ByUserEmailSpec(string email, bool asnotracking = false)
    {
        if (asnotracking)
            Query.AsNoTracking();
        
        var parsedemail = new Email(email);
        
        Query.Where(x => x.Email == parsedemail && !x.IsDeleted);
    }
}