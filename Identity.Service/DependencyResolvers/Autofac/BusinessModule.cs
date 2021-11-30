using Autofac;
using Identity.DataAccess.Abstract;
using Identity.DataAccess.Concrete;
using Identity.Service.Abstract;
using Identity.Service.Concrete;
using Identity.Service.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Service.DependencyResolvers.Autofac
{
   public class BusinessModule : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         builder.RegisterType<CommonService>().As<ICommonService>();
         builder.RegisterType<ImageManager>().As<IImageService>();
         builder.RegisterType<ClaimProvider>().As<IClaimsTransformation>();
         builder.RegisterType<ExpireDateExchangeHandler>().As<IAuthorizationHandler>();
      }
   }
}