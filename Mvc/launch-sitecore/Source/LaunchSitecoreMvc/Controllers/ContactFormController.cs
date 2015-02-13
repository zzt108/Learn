using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Sitecore.Mvc.Presentation;
using Sitecore.Mvc.Common;
using Glimpse.Core.Extensibility;
using LaunchSitecoreMvc.Common;
using Sitecore.Mvc.Presentation;
using Sitecore.ItemExtensions;
using Sitecore.Mvc.Configuration;
using Sitecore.Resources.Media;

namespace LaunchSitecoreMvc.Controllers
{
    public class ContactFormController : StandardController
    {
        public ContactFormController()
        {
            Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} Constructor", "ContactFormController");
        }

        public override ActionResult Index()
        {
            Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} Index()", "ContactFormController");            
            return base.Index();
        }

        // Save Contact Form
        // Form Controller Action
        public ActionResult Save(LaunchSitecoreMvc.Models.ContactForm contactForm)
        {
            Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} Action: {1}", GetType().Name, "Save");
            Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} ModelState.IsValid: {1}", GetType().Name, ModelState.IsValid);

            var formControllerContext = FormControllerContext.Get(
                () => new FormControllerContext(contactForm.FormRenderingId));

            // Do something specific to the sitecore form handler here
            return null;
        }
       
        // Item Controller Action
        [HttpGet]
        public ActionResult BindModel()
        {
            Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} Action: BindModel() HttpGet", "ContactFormController");
            return Index();
        }

        // Item Controller Action
        [HttpPost]
        public ActionResult BindModel(LaunchSitecoreMvc.Models.ContactForm contactForm)
        {
            Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} Action: BindModel() HttpPost", "ContactFormController");
            ViewData.Model = contactForm;

            if (ModelState.IsValid)
            {
                TempData["contactFormModel"] = contactForm;

                var itemid = new Sitecore.Data.ID("{DC630220-08EB-4FAE-9B8B-080590815648}");

                var item = Sitecore.Context.Database.GetItem(itemid);
                var mediaItem = MediaManager.GetMedia(item);
                MediaStream mediaStream = mediaItem.GetStream();
                
                return new FileStreamResult(mediaStream.Stream, mediaItem.MimeType);

                //var successItem = PageContext.Current.Item.Children.FirstOrDefault();
                //successItem = successItem ?? PageContext.Current.Item;

                //return RedirectToRoute(
                //    MvcSettings.SitecoreRouteName,
                //    new { pathInfo = successItem.GetLink().TrimStart(new char[] { '/' }) });
            }
            return Index();
        }

        // Item Controller Action
        public ActionResult Success()
        {
            LaunchSitecoreMvc.Models.ContactForm formModel = TempData["contactFormModel"] as LaunchSitecoreMvc.Models.ContactForm;
            
            // if we find the form model, the submission was a success and we will show the page, otherwise we will redirect back to the parent
            if (formModel != null)
            {
                ViewData["contactFormModel"] = formModel;
                return Index();
            }
            else
            {
                return RedirectToRoute(
                    MvcSettings.SitecoreRouteName,
                    new { pathInfo = PageContext.Current.Item.Parent.GetLink().TrimStart(new char[] { '/' }) });
            }
        }

        

    }
}
