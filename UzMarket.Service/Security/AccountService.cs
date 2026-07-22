using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace UzMarket.ServiceLayer.Security
{
    public class AccountService : IAccountService
    {
        private readonly IHttpContextAccessor _accessor;
        public AccountService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        //Bu property foydalanuvchi tizimga kirgan (authenticated) yoki yo'qligini tekshiradi
        public bool IsAuthenticated
            => _accessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        // ClaimTypes.NameIdentifier claim'dan keladi
        public long UserId
        {
            get
            {
                var claim = _accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return claim != null ? long.Parse(claim) : 0;
            }
        }

        // ClaimTypes.Name claim'dan keladi

        //public string UserName
        //=> _accessor.HttpContext?.User?.Identity?.Name ?? string.Empty;
        public string UserName
        {
            get
            {
                
                if (_accessor.HttpContext != null &&
                    _accessor.HttpContext.User != null &&
                    _accessor.HttpContext.User.Identity != null)
                {
                    return _accessor.HttpContext.User.Identity.Name;
                }

                return string.Empty;
            }
        }

        //public string FullName
        //=> _accessor.HttpContext?.User?.FindFirst("FullName")?.Value ?? string.Empty;
        public string FullName
        {
            get
            {
                if (_accessor.HttpContext == null)
                    return string.Empty;

                if (_accessor.HttpContext.User == null)
                    return string.Empty;

                var claim = _accessor.HttpContext.User.FindFirst("FullName");

                if (claim == null)
                    return string.Empty;

                return claim.Value;
            }
        }
    }
}
