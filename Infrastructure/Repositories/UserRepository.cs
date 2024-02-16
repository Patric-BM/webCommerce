
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Exceptions;

namespace Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User> GetUserById(int id);
    Task<User> GetUserByEmail(string userEmail);
    Task<User> CreateUser(User user);
    Task<User> UpdateUser(User user);
    Task DeleteUser(int id);
    Task<List<User>> List();

}

public class UserRepository : IUserRepository
{


    private static Task<List<User>> _users = Task.Run(() => new List<User>
{
    new User { Id = 2, Username = "Patric", Email = "pp@test.com",PasswordHash = "2975:/gO19xHqIZvuHyQbBPIBgpUgSIjz9GyC:HEkbfAkCy0GwxtLOE4l6NcJmW9o=",Role = "Admin"}
});



    public async Task<User> GetUserById(int id)
    {
      List<User>? result = await _users;
        return result.FirstOrDefault(u => u.Id == id);
    }

    public async Task<User>  GetUserByEmail(string userEmail)
    {
        var result = await _users;
        Console.WriteLine(result);
        return result.FirstOrDefault(u => u.Email == userEmail);
     
    }

    public async Task<List<User>> List()
    {
        return await _users;
    }

    public async Task<User> CreateUser(User user)
    {
        user.Id = (await _users).Max(u => u.Id) + 1;
        (await _users).Add(user);
        return user;
    }

    public async Task<User> UpdateUser(User user)
    {
        var existingUser = (await _users).FirstOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new NotFoundException("Not Found");
        }

        (await _users)[(await _users).IndexOf(existingUser)] = user;
        return user;
    }

    public async Task DeleteUser(int id)
    {
        var existingUser = (await _users).FirstOrDefault(u => u.Id == id);
        if (existingUser is null)
        {
            throw new NotFoundException("Not Found");
        }
        (await _users).Remove(existingUser);
    }
}