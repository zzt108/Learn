using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaunchSitecoreMvc.Helpers;
using Sitecore.Mvc.Presentation;

namespace LaunchSitecoreMvc.Controllers
{
    public class CustomerController : StandardController
    {
        public static readonly string GlobalCustomerPath = "/sitecore/content/Global Content/Customers";

        protected Sitecore.Data.Items.Item CustomerRoot { get; set; }

        public CustomerController()
            : base()
        {
            CustomerRoot = PageContext.Current.Database.GetItem(GlobalCustomerPath);
        }
                
        public override ActionResult Index()
        {
            var customerList = new List<Sitecore.Data.Items.Item>();
            customerList.AddRange(CustomerRoot.GetChildren());
            ViewData["CustomerListData"] = customerList;
            return base.Index();
        }

        public ActionResult Redirect()
        {
            var parentItem = PageContext.Current.Item.Parent;
            var parentLink = Sitecore.Links.LinkManager.GetItemUrl(parentItem).TrimStart(new char[] { '/' });
            return RedirectToRoutePermanent("Sitecore", new { @pathInfo = parentLink });
        }

        public ActionResult Details(string customerId)
        {
            Sitecore.Data.ID customerDataId = CustomerID.Set(customerId);
            var customerData = CustomerRoot.Children.FirstOrDefault(c => c.ID.Equals(customerDataId));

            if (customerData == null)
            {
                 return HttpNotFound("Customer Not Found");
            }

            ViewData["CustomerData"] = customerData;
            return base.Index();
        }



        [HttpGet]
        public ActionResult Edit(string customerId)
        {
            return Details(customerId);
        }

        [HttpPost]
        public ActionResult Edit(string customerId, string DisplayName)
        {
            return base.Index();
        }

        [HttpGet]
        public ActionResult Delete(string customerId)
        {
            return base.Index();
        }




    }
}
