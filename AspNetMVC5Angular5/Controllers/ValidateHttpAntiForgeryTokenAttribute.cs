using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;


// https://kamranicus.com/posts/2013-04-24-protip-using-anti-forgery-token-with-aspnet-web-ap
namespace Web.Attributes
{

    /// <summary>
    /// Validates Anti-Forgery CSRF tokens for Web API
    /// </summary>
    /// <remarks>
    /// See MVC 4 SPA template
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateHttpAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            HttpRequestMessage request = actionContext.Request;

            try
            {
                if (IsAjaxRequest(request))
                {
                    ValidateRequestHeader(request);
                }
                else
                {
                    AntiForgery.Validate();
                }
            }
            catch (Exception ex)
            {

                actionContext.Response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    RequestMessage = actionContext.ControllerContext.Request
                };
                return FromResult(actionContext.Response);
            }
            return continuation();
        }

        private Task<HttpResponseMessage> FromResult(HttpResponseMessage result)
        {
            var source = new TaskCompletionSource<HttpResponseMessage>();
            source.SetResult(result);
            return source.Task;
        }

        private bool IsAjaxRequest(HttpRequestMessage request)
        {
            IEnumerable<string> xRequestedWithHeaders;
            if (request.Headers.TryGetValues("X-Requested-With", out xRequestedWithHeaders))
            {
                string headerValue = xRequestedWithHeaders.FirstOrDefault();
                if (!String.IsNullOrEmpty(headerValue))
                {
                    return String.Equals(headerValue, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
                }
            }

            // ToDo: old-school control for ajax request to change.
            // https://kamranicus.com/posts/2013-04-24-protip-using-anti-forgery-token-with-aspnet-web-ap
            // return false;
            return true;
        }

        private void ValidateRequestHeader(HttpRequestMessage request)
        {
            var headers = request.Headers;
            var cookie = headers
                .GetCookies()
                .Select(c => c[AntiForgeryConfig.CookieName])
                .FirstOrDefault();

            IEnumerable<string> xXsrfHeaders;

            if (headers.TryGetValues("X-XSRF-Token", out xXsrfHeaders))
            {
                var rvt = xXsrfHeaders.FirstOrDefault();

                if (cookie == null)
                {
                    throw new InvalidOperationException(String.Format("Missing {0} cookie", AntiForgeryConfig.CookieName));
                }

                AntiForgery.Validate(cookie.Value, rvt);
            }
            else
            {
                var headerBuilder = new StringBuilder();

                headerBuilder.AppendLine("Missing X-XSRF-Token HTTP header:");

                foreach (var header in headers)
                {
                    headerBuilder.AppendFormat("- [{0}] = {1}", header.Key, header.Value);
                    headerBuilder.AppendLine();
                }

                throw new InvalidOperationException(headerBuilder.ToString());
            }
        }
    }
}