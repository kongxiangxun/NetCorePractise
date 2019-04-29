using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyModel;
using ServiceProvider;
using ServiceProvider.Interface;
using Module = Autofac.Module;

namespace NetCorePractise.ConfigureExtensions
{
    public class RegistrationModule
    {
    }
    /// <summary>
    /// 重写依赖注入的业务
    /// </summary>
    public class ServiceProviderModel : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //所有程序集 和程序集下类型
            var deps = DependencyContext.Default;
            var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package");//排除所有的系统程序集、Nuget下载包
            var listAllType = new List<Type>();
            foreach (var lib in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    listAllType.AddRange(assembly.GetTypes().Where(type => true));
                }
                catch
                {
                    // ignored
                }
            }


            var singletonDependencyType = typeof(ISingletonDependency);
            var arrSingletonDependencyType = listAllType.Where(t => singletonDependencyType.IsAssignableFrom(t) && t != singletonDependencyType).ToArray();
            builder.RegisterTypes(arrSingletonDependencyType)
                .AsImplementedInterfaces()
                .SingleInstance()
                .PropertiesAutowired().EnableInterfaceInterceptors();

            foreach (Type type in arrSingletonDependencyType)
            {
                if (type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object))
                {
                    builder.RegisterType(type).As(type.BaseType)
                        .SingleInstance()
                        .PropertiesAutowired();
                }
            }
        }
    }
}
