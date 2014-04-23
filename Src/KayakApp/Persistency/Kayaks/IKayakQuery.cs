using System.Collections.Generic;
using AppKickStart.Common.Providers.Persistency;
using IKayak.Schemas.Models;

namespace IKayak.Persistency.Kayaks
{
    public interface IKayakQuery : IQuery
    {
        IList<Kayak> GetAll();

        bool SaveAll(IList<Kayak> kayaks);
        Kayak GetKayakByName(string kayakName);
        Kayak GetKayakByKey(string kayakKey);
    }
}