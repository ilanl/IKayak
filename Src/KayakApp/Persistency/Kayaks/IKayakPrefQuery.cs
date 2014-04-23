using System.Collections.Generic;
using AppKickStart.Common.Providers.Persistency;
using IKayak.Schemas.Models;

namespace IKayak.Persistency.Kayaks
{
    public interface IKayakPrefQuery : IQuery
    {
        List<KayakPref> GetAll();
        IList<KayakPref> GetByUser(User user);

        bool SaveUserKayaks(IList<LightKayakPref> kayaks, long userId);
        IList<KayakPref> GetByUserAndTime(User user, string dayOfWeek, Timing when);
    }
}