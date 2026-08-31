using Riok.Mapperly.Abstractions;
using BazzarOn.Application.User.Models;

namespace BazzarOn.Application.User;

[Mapper]
public partial class UserMapper
{
    [MapperIgnoreSource(nameof(Domain.Entities.User.Id))]
    [MapperIgnoreSource(nameof(Domain.Entities.User.Password))]
    public partial UserDto Map(Domain.Entities.User user);

    public partial List<UserDto> Map(List<Domain.Entities.User> user);
}