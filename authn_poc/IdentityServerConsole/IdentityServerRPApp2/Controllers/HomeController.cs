using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace IdentityServerMVC.Controllers
{
    public class HomeController : Controller
    {

 
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [Authorize]
        public ActionResult Identity()
        {
            return View((User as ClaimsPrincipal).Claims);
        }

        [Authorize]
        public async Task<ViewResult> Resource()
        {
            var request = System.Web.HttpContext.Current.Request;
            var token = request.Cookies.Get("access_token")?.Value;
           
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); 
            
            var json = await client.GetStringAsync(ConfigurationManager.AppSettings["res1:Url"]);
            ViewBag.Json = JArray.Parse(json).ToString();
            return View();
        }

        public ActionResult AjaxResource()
        {
            return View();
        }
    }
}
