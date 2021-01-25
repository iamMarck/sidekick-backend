using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SideKick.Examination.WS.Constants
{
    public class Cons
    {

        public const string LoginSalt = "loginSalt";
        public const string Login = "login";
        public const string Register = "register";

        public const string EmailVerification = "emailVerification";
        public const string CheckUsername = "checkUsername";
        public const string CheckEmail = "checkEmail";


        #region SALT
        public const string SaltKey = "superSecretKey";
        public const int SaltValidityINSeconds = 60 * 5; // 5 minutes

        #endregion
    }


}
