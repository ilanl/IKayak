using AppKickStart.Common.Providers.Algorithms;

namespace IKayak.Algorithms.Login
{
    public interface IWeatherAlgorithm:IAlgorithm
    {
        void RetrieveLastForecasts(string key, double latitude, double longitude);
    }
}