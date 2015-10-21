using System.Web.Mvc;
using Autofac;
using Autofac.Features.Variance;
using Autofac.Integration.Mvc;
using MediatR;

namespace AzureApplicationDemo
{
    public class AutoFacConfig
    {
        public static void RegisterServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterFilterProvider();

            // build up list of services
            // ...

            // register AutoFac as the container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }

    }
}