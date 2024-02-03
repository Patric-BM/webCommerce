
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Exceptions;

namespace Infrastructure.Repositories;

public interface IUserRepository
{
    User GetUserById(int id);
    User GetUserByUsername(string username);
    User CreateUser(User user);
    User UpdateUser(User user);
    void DeleteUser(int id);

}

public class UserRepository : IUserRepository
{
    private static List<User> _users = new List<User>
    {
        new User { Id = 1, Username = "admin", PasswordHash = "admin", IsAdmin = true },
        new User { Id = 2, Username = "user", PasswordHash = "user", IsAdmin = false }
    };

    public User GetUserById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id ) ?? throw new NotFoundException("Not Found");
    }

    public User GetUserByUsername(string username)
    {
        return _users.FirstOrDefault(u => u.Username == username) ?? throw new NotFoundException("Not Found");
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