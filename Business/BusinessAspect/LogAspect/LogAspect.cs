using Castle.DynamicProxy;
using Core.Utulities.Interceptors;
using Core.Utulities.IOC;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Microsoft.AspNetCore.Http;

namespace Business.BusinessAspect.LogAspect
{
    public class LogAspect : MethodInterception
    {
        private readonly IExceptionLogDal _exceptionLogDal;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// test
        /// </summary>
        public LogAspect()
        {

            _exceptionLogDal = (EfExceptionLogDal)Activator.CreateInstance(typeof(EfExceptionLogDal));
            _httpContextAccessor = (IHttpContextAccessor)ServiceTool.ServiceProvider.GetService(typeof(IHttpContextAccessor));
        }
        protected override void OnException(IInvocation invocation, Exception e)
        {
            var cliUserID = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "systemUserId");

            int userId = 0;

            int.TryParse(cliUserID?.Value, out userId);

            _exceptionLogDal.Add(new Entities.Concrete.ExceptionLog()
            {

                Class = invocation.Method.DeclaringType.FullName,
                Method = invocation.Method.Name,
                Message = e?.Message,
                CreatedBy = userId,
                CreatedDate = DateTime.Now,
            });

        }
    }
}
