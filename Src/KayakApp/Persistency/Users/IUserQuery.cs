using System.Collections.Generic;
using AppKickStart.Common.Providers.Persistency;
using IKayak.Schemas.Models;

namespace IKayak.Persistency.Users
{
    public interface IUserQuery : IQuery
    {
        User GetUser(string userName);
        
        User GetUserById(long userId);

        User Save(User user);

        IList<User> GetAll();
        void UpdateStatus(string userName, UserStatus status);
        void UpdateReminder(string userName, int reminder);
    }
}