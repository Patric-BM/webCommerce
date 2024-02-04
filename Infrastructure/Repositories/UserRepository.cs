
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Exceptions;

namespace Infrastructure.Repositories;

public interface IUserRepository
{
    User GetUserById(int id);
    User GetUserByEmail(string userEmail);
    User CreateUser(User user);
    User UpdateUser(User user);
    void DeleteUser(int id);
    List<User> List();

}

public class UserRepository : IUserRepository
{
    private static List<User> _users = new List<User>
    {
        new User { Id = 1, Username = "User1", Email = "test@test.com", PasswordHash = "000000", Role = "Admin" },
        new User { Id = 2, Username = "Patric", Email = "pp@test.com",PasswordHash = "2975:/gO19xHqIZvuHyQbBPIBgpUgSIjz9GyC:HEkbfAkCy0GwxtLOE4l6NcJmW9o=",Role = "Admin"
}
    };

    public User GetUserById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public User GetUserByEmail(string userEmail)
    {
        return _users.FirstOrDefault(u => u.Email == userEmail);
    }

    public List<User> List()
    {
        return _users;
    }

    public User CreateUser(User user)
    {
        user.Id = _users.Max(u => u.Id) + 1;
        _users.Add(user);
        return user;
    }

    public User UpdateUser(User user)
    {
        var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new NotFoundException("Not Found");
        }

        _users[_users.IndexOf(existingUser)] = user;
        return user;
    }

    public void DeleteUser(int id)
    {
        var existingUser = _users.FirstOrDefault(u => u.Id == id);
        if (existingUser is null)
        {
            throw new NotFoundException("Not Found");
        }

        _users.Remove(existingUser);
    }
}