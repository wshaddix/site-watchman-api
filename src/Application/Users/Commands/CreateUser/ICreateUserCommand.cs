using System.Threading.Tasks;
using SiteWatchman.Application.Users.Models;

namespace SiteWatchman.Application.Users.Commands.CreateUser
{
    public interface ICreateUserCommand
    {
        Task<UserModel> ExecuteAsync(string username, string password);
    }
}