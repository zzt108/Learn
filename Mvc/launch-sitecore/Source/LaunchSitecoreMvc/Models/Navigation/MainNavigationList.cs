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
    public class MainNavigationList : RenderingModel
    {
        protected int maxDepth = 5;
        
        public int MaxDepth { get { return maxDepth; } }

        public NavigationItem[] DisplayItems { get; protected set; }

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
            //Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} Cachable: {1}", GetType().Name, Rendering.Caching.Cacheable);
            //Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} CacheKey: {1}", GetType().Name, Rendering.Caching.CacheKey);
            //Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} VaryByData: {1}", GetType().Name, Rendering.Caching.VaryByData);

            using (Glimpse.Core.Extensibility.GlimpseTrace.Time(string.Format("{0} Initialize", GetType().FullName)))
            {

                try
                {
                    Glimpse.Core.Extensibility.GlimpseTrace.Info(string.Format("[ 'Type' , '{0}' ]", GetType().FullName));

                    var mainNavRootItemId = !string.IsNullOrEmpty(Item["Menu Root"]) ? Item["Menu Root"] : "{8705CD12-DEED-451F-8A57-C7F02CB9222B}";
                    var mainNavRootItem = Item.Database.SelectSingleItem(mainNavRootItemId);

                    var filterChildren = new Func<Item, bool>(i => !i.GetCheckBoxFieldValue("Hide Item from Menu"));

                    var navItems = LaunchSitecoreMvc.Business.NavigationBusiness.GetNavigationItems(mainNavRootItem, maxDepth, filterChildren, true);

                    DisplayItems = navItems.ToArray();
                }
                catch (Exception ex)
                {
                    DisplayItems = new NavigationItem[] { new NavigationItem() { Title = new HtmlString(ex.Message) } };
                }
            }
        }
    }
}