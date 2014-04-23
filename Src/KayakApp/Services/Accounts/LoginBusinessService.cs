using AppKickStart.Common.Providers;
using AppKickStart.Common.Providers.Services;
using IKayak.Algorithms.Login;
using IKayak.Schemas.Contracts.Accounts;
using IKayak.Schemas.Models;
using IKayak.Schemas.Models.Exceptions;

namespace IKayak.Services.Accounts
{
    public class LoginBusinessService :
        BusinessService<LoginRequest, string, LoginResponse>, ILoginBusinessService
    {
        private const string Ok = "Valid";

        public LoginBusinessService()
        {
        }

        public LoginBusinessService(IAppContext appContext)
            : base(appContext)
        {
        }

        #region ILoginBusinessService Members

        public override string ServiceName
        {
            get { return "login"; }
        }

        public override string Request
        {
            get { return "LoginRequest"; }
        }

        public override string Response
        {
            get { return "LoginResponse"; }
        }

        #endregion

        public override string Execute(LoginRequest args)
        {
            User user;
            
            var algorithm = AppContext.AlgorithmProvider.Get<ILoginAlgorithm>();
            
            if (algorithm.Login(args.UserName, args.Password, args.DeviceToken, out user) == null)
                throw new UserNotFoundBusinessException();

            return Ok;
        }

        public override LoginResponse BuildResponse(string results)
        {
            return new LoginResponse();
        }
    }
}