using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Apd.WebApi.Extension {
    public static class ApiControllerExtension {
        public static HttpResponseMessage CreateResponsWithJsonContent(this ApiController controller, HttpStatusCode statusCode, string jsonString) {
            var response = controller.Request.CreateResponse(statusCode);
            response.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            return response;
        }
    }
}