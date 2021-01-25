using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace SideKick.Examination.Data.Model
{
    public class ClaimsPrincipalUser : ClaimsPrincipal
    {
        const string NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public ClaimsUser User { get; private set; }

        public ClaimsPrincipalUser(ClaimsPrincipal user) : base(user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var id = user.Claims.Where(w => w.Type == "sub").FirstOrDefault()?.Value;
            var name = user.Claims.Where(w => w.Type == "name").FirstOrDefault()?.Value;
            var userid = id ?? user.Claims.FirstOrDefault(c => c.Type == NameClaimType)?.Value;
            var username = name ?? user.Claims.FirstOrDefault(c => c.Type == "username")?.Value;

            User = new ClaimsUser
            {
                Id = Convert.ToInt64(userid),
                UserName = username,

            };

        }

        public ClaimsPrincipalUser(ClaimsUser user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public ClaimsPrincipalUser(IPrincipal principal, ClaimsUser user) : base(principal)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }

    public class ClaimsUser
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string AccessToken { get; set; }
    }
}
