using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.ItemExtensions;
using LaunchSitecoreMvc.Helpers;

namespace LaunchSitecoreMvc.Models
{
    public class BreadcrumbNavigationList : Sitecore.Mvc.Presentation.RenderingModel
    {
        public NavigationItem[] DisplayItems { get; protected set; }

        public HtmlString GetBreadCrumbText() 
        {            
            var breadcrumbString = string.Join("&nbsp;<span>&gt;</span>&nbsp;\n",
                DisplayItems.Select(
                    n => PageContext.Current.HtmlHelper.RenderNavigationItem(n).ToString()).ToArray());
            return new HtmlString(breadcrumbString);
        }

        public override void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            base.Initialize(rendering);
            using (Glimpse.Core.Extensibility.GlimpseTrace.Time(string.Format("{0} Initialize", GetType().FullName)))
            {
                var mainNavRootItemId = !string.IsNullOrEmpty(Item["Menu Root"]) ? Item["Menu Root"] : "{8705CD12-DEED-451F-8A57-C7F02CB9222B}";

                var mainNavRootItem = Item.Database.SelectSingleItem(mainNavRootItemId);

                List<LaunchSitecoreMvc.Models.NavigationItem> items = new List<LaunchSitecoreMvc.Models.NavigationItem>();

                Sitecore.Data.Items.Item rootItem = mainNavRootItem;
                Sitecore.Data.Items.Item currentPage = PageItem;


                while (currentPage != null && rootItem != null && !currentPage.ID.Equals(rootItem.Parent.ID))
                {
                    var navItem = LaunchSitecoreMvc.Business.NavigationBusiness.GetNavigationItem(currentPage, 0);
                    navItem.CssClass = "";
                    items.Add(navItem);
                    currentPage = currentPage.Parent;
                }

                items.Add(new NavigationItem() { Title = new HtmlString("Sitecore.Mvc") });

                items.Reverse();

                DisplayItems = items.ToArray();
            }
        }
    }
}