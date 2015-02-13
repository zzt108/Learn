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
    public class AjaxContactFormController : AsyncController
    {
        public AjaxContactFormController()
        {
        }
        
        public void IndexAsync(Models.ContactForm contactForm)
        {
            ViewData.Model = contactForm;
            
            if (Request.HttpMethod == "POST" && ModelState.IsValid)
            {
                AsyncManager.OutstandingOperations.Increment();
                AsyncManager.Finished += AsyncManager_Finished;
                
                System.Threading.Tasks.Task.Factory.StartNew<Guid>(
                    () =>
                    {
                        return RunBackgroundTask();
                    }
                ).ContinueWith(
                    (t) =>
                    {
                        AsyncManager.Parameters["controllerResult"] = t.Result;
                        AsyncManager.OutstandingOperations.Decrement();
                    }
                );

            }
            return;
        }

        private static Guid RunBackgroundTask()
        {
            System.Threading.Thread.Sleep(5000);
            return Guid.NewGuid();
        }

        void AsyncManager_Finished(object sender, EventArgs e)
        {            
        }

        protected override IAsyncResult BeginExecute(System.Web.Routing.RequestContext requestContext, AsyncCallback callback, object state)
        {
            return base.BeginExecute(requestContext, callback, state);
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            base.EndExecute(asyncResult);
        }

        //[HttpPost]
        //[ValidateInput(false)]
        public ActionResult IndexCompleted(Guid controllerResult)
        {

            if (Request.HttpMethod == "POST" && ModelState.IsValid)
            {
                TempData["contactFormModel"] = ViewData.Model;
                TempData["contactFormControllerResult"] = ViewData.Model;

                var successItem = PageContext.Current.Item.Children.FirstOrDefault();
                successItem = successItem ?? PageContext.Current.Item;

                var requestedWith = Request.IsAjaxRequest() ? "XMLHttpRequest" : "";
                var routeParameters = new System.Web.Routing.RouteValueDictionary()
                {
                    { "X-Requested-With", requestedWith },
                    { "pathInfo", successItem.GetLink().TrimStart(new char[] { '/' }) }
                };

                return RedirectToRoute(
                    MvcSettings.SitecoreRouteName,
                    routeParameters);

            }
            else if (Request.HttpMethod == "POST")
            {
                ModelState.AddModelError("Main", 
                    string.Format("POST Controller Async Action Completed With Invalid Model: {0}", 
                        DateTime.Now.ToLongTimeString()));

                if (Request.IsAjaxRequest())
                {
                    var ajaxRendering = PageContext.Current.PageDefinition.Renderings.Where(r => r.Parameters.Contains("ajaxContainer")).FirstOrDefault();
                    if (ajaxRendering != null)
                    {
                        var partialView = new Sitecore.Mvc.Presentation.RenderingView(ajaxRendering);
                        return this.View(partialView);
                    }
                    else
                    {
                        return this.Content("Error loading partial view");
                    }
                }
            }
            else if (Request.IsAjaxRequest())
            {
                return this.Content(string.Format("<div>Controller Ajax Async Action Completed: {0}</div>", DateTime.Now.ToLongTimeString()));
            }
            return GetDefaultAction();
        }
        
        protected virtual ActionResult GetDefaultAction()
        {
            IView pageView = PageContext.Current.PageView;
            if (pageView == null)
            {
                return new HttpNotFoundResult();
            }
            else
            {
                return (ActionResult)this.View(pageView);
            }
        }
               

        public ActionResult Success(string success)
        {
            LaunchSitecoreMvc.Models.ContactForm formModel = TempData["contactFormModel"] as LaunchSitecoreMvc.Models.ContactForm;
            ViewData.Model = formModel;

            // if we find the form model, the submission was a success and we will show the page, otherwise we will redirect back to the parent
            if (formModel != null)
            {
                ViewData["contactFormModel"] = formModel;

                if (Request.IsAjaxRequest())
                {
                    //var successRenderingId = "{1E527F29-4A6C-4675-BA61-9AB83E384A05}";

                    //var partialRendering = new Sitecore.Mvc.Presentation.Rendering()
                    //{
                    //    RenderingType = "View",
                    //    RenderingItemPath = successRenderingId
                    //};

                    var ajaxRendering = PageContext.Current.PageDefinition.Renderings.Where(r => r.Parameters.Contains("ajaxContainer")).FirstOrDefault();

                    var partialView = new Sitecore.Mvc.Presentation.RenderingView(ajaxRendering);
                    return this.View(partialView);
                }

                return GetDefaultAction();
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
