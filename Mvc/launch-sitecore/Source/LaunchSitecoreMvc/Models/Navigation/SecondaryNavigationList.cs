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
    public class SecondaryNavigationList : RenderingModel
    {
        public NavigationItem[] DisplayItems { get; protected set; }

        protected int maxDepth = 99;

        public int MaxDepth { get { return maxDepth; } }
        
        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
            using (Glimpse.Core.Extensibility.GlimpseTrace.Time(string.Format("{0} Initialize", GetType().FullName)))
            {
                Item currentPage = PageItem;
                var mainNavRootItemId = !string.IsNullOrEmpty(Item["Menu Root"]) ? Item["Menu Root"] : "{8705CD12-DEED-451F-8A57-C7F02CB9222B}";
                var mainNavRootItem = Item.Database.SelectSingleItem(mainNavRootItemId);

                var secondaryNavRootitem = mainNavRootItem.Children.FirstOrDefault(i => currentPage.Paths.FullPath.StartsWith(i.Paths.FullPath));

                var filterChildren = new Func<Item, bool>(
                    i => !i.GetCheckBoxFieldValue("Hide Item from Menu"));

                try
                {
                    var secondaryNavItem = LaunchSitecoreMvc.Business.NavigationBusiness.GetSecondaryNavigationItems(currentPage, secondaryNavRootitem, filterChildren, null, maxDepth);

                    DisplayItems = secondaryNavItem.Children.ToArray();
                }
                catch (Exception ex)
                {
                    DisplayItems = new NavigationItem[] { new NavigationItem() { Title = new HtmlString(ex.Message) } };
                }
            }
        }


    }
}