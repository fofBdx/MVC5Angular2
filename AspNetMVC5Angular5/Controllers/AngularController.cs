using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Attributes;

namespace AspNetMVC5Angular5.Controllers
{
    public class AngularController : ApiController
    {
        [Authorize]
        [ValidateHttpAntiForgeryToken]
        // GET: api/Angular
        public IEnumerable<string> Get()
        {
            return new string[] {
                "from WEB API",
                "Authorize validated",
                "AntiForgeryToken validated"
            };
        }

        // GET: api/Angular/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Angular
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Angular/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Angular/5
        public void Delete(int id)
        {
        }
    }
}
