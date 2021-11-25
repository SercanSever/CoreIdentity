using Autofac;
using Identity.DataAccess.Abstract;
using Identity.DataAccess.Concrete;
using Identity.Service.Abstract;
using Identity.Service.Concrete;

namespace Identity.Service.DependencyResolvers.Autofac
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommonService>().As<ICommonService>();
        }
    }
}