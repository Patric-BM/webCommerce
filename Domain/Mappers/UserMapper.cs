
using Domain.Entities;
using Domain.Responses;

namespace Domain.Mappers;

public static class UserMapper
{
    public static UserResponse Map(User user)
    {
        return new UserResponse
        {
            Id = user.Id.ToString(),
            Email = user.Email,
            Username = user.Username,
            Role = user.Role
        };
    }
}
