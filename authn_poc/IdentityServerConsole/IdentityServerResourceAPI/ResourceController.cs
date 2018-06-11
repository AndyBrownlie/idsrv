using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;
using log4net;

namespace IdentityServerResourceAPI
{
    [Authorize]
    [Route("resource")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ResourceController : ApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IHttpActionResult Get()
        {
            Logger.Debug("Remote call made");
            var user = User as ClaimsPrincipal;
            var claims = from c in user.Claims
                select new
                {
                    type = c.Type,
                    value = c.Value
                };

            return Json(claims);
        }
    }
}