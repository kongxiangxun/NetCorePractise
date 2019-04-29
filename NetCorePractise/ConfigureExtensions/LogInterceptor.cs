using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using ServiceProvider.Interface;

namespace NetCorePractise.ConfigureExtensions
{
    public class LogInterceptor: IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {

            Console.WriteLine($"使用autofac+castle调用方法{invocation.Method.Name},参数是{string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())}");
            
            invocation.Proceed();

            if (invocation.ReturnValue != null && invocation.ReturnValue is string)
            {
                invocation.ReturnValue += "_LogInterceptor";
            }

            Console.WriteLine($"方法调用完毕，返回结果{invocation.ReturnValue}");
            
        }
    }

}
