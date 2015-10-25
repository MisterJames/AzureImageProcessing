using Autofac;
using MediatR;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Autofac.Features.Variance;
using System.Collections.Generic;
using Autofac.Extras.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
using AzureApplicationDemo.Features.Upload;

namespace AzureApplicationDemo
{
    public class ContainerConfig
    {
        public static void RegisterServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterSource(new ContravariantRegistrationSource());
            RegisterMediatr(builder);
           
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterAssemblyTypes(typeof(UploadFileCommand).Assembly).AsImplementedInterfaces();
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterFilterProvider();

            // build up list of services
            // ...


            // register AutoFac as the container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
        private static void RegisterMediatr(ContainerBuilder builder)
        {
            builder.Register(x => new ServiceLocatorProvider(() => new AutofacServiceLocator(AutofacDependencyResolver.Current.RequestLifetimeScope)))
                                        .InstancePerRequest();
            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();
        }
    }
}