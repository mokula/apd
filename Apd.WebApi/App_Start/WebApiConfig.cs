using System.Web.Http;
using Apd.Common.Container;
using Apd.Model.Repository;
using Apd.WebApi.Controllers;
using Apd.WebApi.Factory;
using Apd.WebApi.Repository;
using Newtonsoft.Json;

namespace Apd.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new TinyIoCContainer();
            container.Register<IContactFactory, ContactFactory>();
            container.Register<IContactRepository, DictionaryContactRepository>();
            container.Register<ContactController>();
            config.DependencyResolver = new TinyIoCResolver(container);
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings();
            
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
