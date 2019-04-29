using System;
using ServiceProvider.Interface;

namespace ServiceProvider
{
    //[Intercept(typeof(LogInterceptor))]
    public class ServiceInfo : IServiceInfo, ISingletonDependency
    {
        private string name { get; set; }
        public ServiceInfo()
        {
            name = "fdsfds";
        }
        public string GetName()
        {
            return nameof(ServiceInfo);
        }
    }
}
