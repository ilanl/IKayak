using System.Net;
using AppKickStart.Common.Providers.Algorithms;
using IKayak.Schemas.Models;

namespace IKayak.Algorithms.Login
{
    public interface ILoginAlgorithm:IAlgorithm
    {
        
        Cookie Login(string userName, string password, string deviceToken, out User user);
    }
}