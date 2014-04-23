using System.Collections.Generic;
using AppKickStart.Common.Providers.Persistency;
using IKayak.Schemas.Models;

namespace IKayak.Persistency.Preferences
{
    public interface IPreferenceQuery : IQuery
    {
        //IList<Preference> GetByUser(User user);

        IList<LightTimePref> GetUserPrefsById(long userId);
        bool SaveUserPrefs(IList<LightTimePref> preferences, User user);
    }
}