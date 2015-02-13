using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;

namespace LaunchSitecoreMvc.Controllers
{
    public class CustomController : StandardController
    {
        public override ActionResult Index()
        {
            return base.Index();
        }

        public ActionResult Test(string id)
        {
            string result = true.ToString();

            var contentRendering = new ContentRendering()
                {
                    Id = new Guid(),
                    UniqueId = new Guid(),
                    Placeholder = "main-content",
                    DeviceId = PageContext.Current.Device.Id
                };

            contentRendering.Content = string.Format(
                 "<h3>{0} - {1} Action - id: {2} - Result: {3} </h3>",
                 GetType().Name,
                 "Test",
                 id,
                 result);

            var currentRenderings = PageContext.Current.PageDefinition.Renderings;
            currentRenderings.Add(contentRendering);
            
            return Index();
        }
    }
}
