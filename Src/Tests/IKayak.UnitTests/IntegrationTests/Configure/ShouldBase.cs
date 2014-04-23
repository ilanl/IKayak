using System.Collections.Generic;
using System.Threading;
using AppKickStart.Client;
using AppKickStart.Schemas.Contracts.Enums;
using AppKickStart.Schemas.Contracts.SetUp;
using IKayak.Schemas.Contracts.Accounts;
using IKayak.Schemas.Contracts.Bookings;
using IKayak.Schemas.Contracts.Forecasts;
using IKayak.Schemas.Contracts.Preferences;
using IKayak.Schemas.Models;
using NUnit.Framework;

namespace IKayak.Tests.IntegrationTests.Configure
{
    public class ShouldBase
    {
        protected ServiceInvoker Si;

        public ShouldBase()
        {
            Si = new ServiceInvoker();
        }

        protected void Wait(int i)
        {
            Thread.Sleep(i*1000);
        }

        protected void WakeUpServer()
        {
            SetUpResponse response = Si.Invoke<SetUpRequest, SetUpResponse>("setup",
                                                                            new SetUpRequest
                                                                                {
                                                                                    LogLevel = "DEBUG",
                                                                                    Action = Action.Save
                                                                                });

            Assert.IsNotNull(response);
            Assert.IsNull(response.Error);
        }

        protected void SavePreferences(string userName, string password, string deviceToken, IList<LightTimePref> timingPrefs,
                                       IList<LightKayakPref> kayakPrefs)
        {
            var preferenceRequest = new PreferenceRequest
                                        {
                                            Action = Action.Save,
                                            UserName = userName,
                                            Password = password,
                                            DeviceToken = deviceToken,
                                            Set = new Set {TimePrefs = timingPrefs, KayakPrefs = kayakPrefs}
                                        };

            PreferenceResponse response = Si.Invoke<PreferenceRequest, PreferenceResponse>("preferences",
                                                                                           preferenceRequest);

            Assert.IsNotNull(response);
        }

        protected PreferenceResponse GetPreferences(string userName, string password, string deviceToken)
        {
            var preferenceRequest = new PreferenceRequest
                                        {
                                            Action = Action.Lookup,
                                            UserName = userName,
                                            Password = password,
                                            DeviceToken = deviceToken,
                                            Set = new Set()
                                        };

            PreferenceResponse response = Si.Invoke<PreferenceRequest, PreferenceResponse>("preferences",
                                                                                           preferenceRequest);
            Assert.IsNotNull(response);

            Assert.IsNotNull(response.Set);

            return response;
        }


        protected BookingResponse GetBookings(string userName, string password, string deviceToken)
        {
            var request = new BookingRequest()
            {
                Action = Action.Lookup,
                UserName = userName,
                Password = password,
                DeviceToken = deviceToken
            };

            var response = Si.Invoke<BookingRequest, BookingResponse>("bookings", request);
            Assert.IsNotNull(response);
            Assert.AreEqual("Success", response.Status);

            return response;
        }

        protected ForecastResponse GetForecasts()
        {
            var request = new ForecastRequest();

            var response = Si.Invoke<ForecastRequest, ForecastResponse>("forecasts", request);
            Assert.IsNotNull(response);
            Assert.AreEqual("Success", response.Status);

            return response;
        }

        protected void Login(string user, string pwd, string deviceToken)
        {
            LoginResponse response = Si.Invoke<LoginRequest, LoginResponse>("login",
                                                                            new LoginRequest
                                                                                {UserName = user, Password = pwd, DeviceToken = deviceToken});
            Assert.IsNotNull(response);
            Assert.AreEqual("Success", response.Status);
        }

        protected void UpdateUserStateAndReminder(string userName, string password, string deviceToken, string state, int reminder)
        {
            var request = new PreferenceRequest()
            {
                UserName = userName,
                Password = password,
                Action = Action.Save,
                Reminder = reminder,
                IsFrozen = state,
                DeviceToken = deviceToken
            };

            var response = Si.Invoke<PreferenceRequest, PreferenceResponse>("preferences", request);

            Assert.IsNotNull(response);
            Assert.AreEqual(state == "1",response.IsFrozen);
            Assert.AreEqual(response.Reminder, reminder);
            Assert.AreEqual("Success",response.Status);
        }
    }
}