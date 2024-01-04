using Castle.DynamicProxy;
using Core.Utulities.Interceptors;
using Core.Utulities.IOC;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessAspect.Performance
{
    public class PerformanceAspect : MethodInterception
    {
        private readonly int _interval;
        private readonly Stopwatch _stopwatch;
        private readonly IPerformanceLogDal _performanceDal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PerformanceAspect(int interval)
        {
            _interval = interval;
            _stopwatch = (Stopwatch)ServiceTool.ServiceProvider.GetService(typeof(Stopwatch));

            _performanceDal = (EfPerformanceLogDal)Activator.CreateInstance(typeof(EfPerformanceLogDal));
            _httpContextAccessor = (IHttpContextAccessor)ServiceTool.ServiceProvider.GetService(typeof(IHttpContextAccessor));
        }

        protected override void OnBefore(IInvocation invocation)
        {
            _stopwatch.Start();
        }

        protected override void OnAfter(IInvocation invocation)
        {
            var cliUserId = _httpContextAccessor?.HttpContext?.User?.Claims?.SingleOrDefault(x => x.Type == "systemUserId");

            int userId = 0;

            if (_stopwatch.Elapsed.TotalSeconds > _interval)
            {
                Debug.WriteLine($"Performance : {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}-->{_stopwatch.Elapsed.TotalSeconds}");
                _performanceDal.Add(new PerformanceLog()
                {
                    Class = invocation.Method.DeclaringType.FullName,
                    Method = invocation.Method.Name,
                    Duration = ((int)_stopwatch.Elapsed.TotalSeconds),
                    //CreatedBy = userId

                });
            }
            _stopwatch.Reset();
        }
    }
}
