using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SideKick.Examination.Data;
using SideKick.Examination.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SideKick.Examination.WS.Services
{
    public class BaseService<T>
    {
        protected ClaimsPrincipalUser CurrentUser { get; private set; }

        public ILogger Logger;
        public ClientDbContext ClientDbContext;


        public BaseService(ILogger<T> logger, ClientDbContext clientDbContext)
        {
            Logger = logger;
            ClientDbContext = clientDbContext;
        }

        protected void HandleException(Exception ex)
        {
            LogError(ex);
        }

        protected void LogError(Exception ex)
        {
            Logger.LogError(string.Format("Error! {0} {1}", ex.InnerException, ex.StackTrace));
        }
    }
}
