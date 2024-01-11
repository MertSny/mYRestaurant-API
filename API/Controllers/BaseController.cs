using Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BaseController : ControllerBase
    {
        internal UserContext GetCurrentUser()
        {
            var userContext = new UserContext
            {
                SystemUserId = Convert.ToInt32(User.Claims.FirstOrDefault(f => f.Type == "systemUserId").Value),
                CurrentUserId = User?.Claims?.FirstOrDefault(f => f.Type == "currentUserId")?.Value,
                FullName = User.Claims.FirstOrDefault(f => f.Type == "fullName").Value,
                Email = User.Claims.FirstOrDefault(f => f.Type == "systemUserEmail").Value,
                UserName = User.Claims.FirstOrDefault(f => f.Type == "registryNo").Value,
                ProxyUserId = User.Claims.FirstOrDefault(f => f.Type == "proxyUserId").Value,
                CompanyId = Convert.ToInt32(User.Claims.FirstOrDefault(f => f.Type == "companyId").Value),
                BudgetCode = Convert.ToInt32(User.Claims.FirstOrDefault(f => f.Type == "budgetcode").Value),
                Roles = string.IsNullOrEmpty(User.Claims.FirstOrDefault(f => f.Type == "systemUserRoles").Value) ? new List<string>() : User.Claims.FirstOrDefault(f => f.Type == "systemUserRoles").Value.Split(',').ToList(),

            };
            return userContext;
        }
    }
}
