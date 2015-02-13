using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Mvc;
using Sitecore.Mvc.Presentation;

namespace LaunchSitecoreMvc.Controllers
{
    public class StandardController : Sitecore.Mvc.Controllers.SitecoreController
    {
        public StandardController() : base()
        {
            Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} Constructor", "StandardController");
        }

        public override ActionResult Index()
        {
            // temporarily disable page editor for items using controllers
            DisablePageEditor();
            Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} Index()", "StandardController");
            return base.Index();
        }

        private static void DisablePageEditor()
        {
            Sitecore.Context.Site.SetDisplayMode(Sitecore.Sites.DisplayMode.Normal, Sitecore.Sites.DisplayModeDuration.Temporary);
        }
        
        public ActionResult RedirectTemporary()
        {
            var redirectString = PageContext.Current.Item["Menu Link"];

            if (!string.IsNullOrEmpty(redirectString) && !redirectString.Contains("://"))
            {
                var redirectLink = Sitecore.Links.LinkManager.GetItemUrl(PageContext.Current.Database.GetItem(redirectString));
                return base.Redirect(redirectLink);
            }
            else if (!string.IsNullOrEmpty(redirectString) && redirectString.Contains("://"))
            {
                return base.Redirect(redirectString);
            }
            else
            {
                ViewData["StandardException"] = "Error: no redirect item found";
                return GetDefaultAction();
            }
        }
        
    }
}
