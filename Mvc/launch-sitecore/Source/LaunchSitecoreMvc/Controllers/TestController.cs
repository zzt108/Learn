using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaunchSitecoreMvc.Controllers
{
    public class TestController : Sitecore.Mvc.Controllers.SitecoreController
    {
        //
        // GET: /Test/

        public override ActionResult Index()
        {
            ViewData["Test"] = GetType();

            ViewData.Model = true;

            return base.Index();

        }

    }
}
