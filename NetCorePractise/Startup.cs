using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetCorePractise.ConfigureExtensions;
using ServiceProvider;
using ServiceProvider.Interface;

namespace NetCorePractise
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //使用ServiceBasedControllerActivator替换DefaultControllerActivator（意味着框架现在会尝试从IServiceProvider中解析控制器实例，也就是return new AutofacServiceProvider(Container)
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //将Evolution注册到项目中来，实现依赖注入
            var builder = new ContainerBuilder();



            builder.RegisterType(typeof(LogInterceptor));
            //注册所有程序集下继承ControllerBase的类型，PropertiesAutowired 允许属性注入。
            var assembly = typeof(Startup).Assembly;
            var controllerType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(assembly).Where(t =>
                controllerType.IsAssignableFrom(t) && t != controllerType).PropertiesAutowired()
                .EnableClassInterceptors().InterceptedBy(typeof(LogInterceptor));

        
            builder.RegisterModule(new ServiceProviderModel());

            builder.Populate(services);
            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
