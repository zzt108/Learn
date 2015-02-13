using System;
using System.Collections.Generic;
using System.Linq;
using LaunchSitecoreMvc.Models;
using Sitecore.Data.Items;

namespace LaunchSitecoreMvc.Business
{
    public static class NavigationBusiness
    {

        public static NavigationItem GetSecondaryNavigationItems(Item currentItem, Item rootItem, Func<Item, bool> filterChildren = null, NavigationItem prevresult = null, int depth = 0)
        {
            if (currentItem == null || rootItem == null || depth < 0) { return prevresult; }

            NavigationItem result = GetNavigationItem(currentItem, 2, filterChildren);
            result.CssClass = "selected";

            if (prevresult != null)
            {
                var findPreviousItemIndex = result.Children.FindIndex(n => prevresult.DisplayItem.ID == n.DisplayItem.ID);
                if (findPreviousItemIndex >= 0)
                {
                    result.Children[findPreviousItemIndex] = prevresult;
                }
            }

            if (!rootItem.ID.Equals(currentItem.ID))
            {
                return GetSecondaryNavigationItems(currentItem.Parent, rootItem, filterChildren, result, depth - 1);
            }
            else
            {
                return result;
            }
        }


        public static List<NavigationItem> GetNavigationItems(
           Item item,
           int depth = 0,
           Func<Item, bool> filterChildren = null,
           bool includeSelf = false)
        {

            List<NavigationItem> result = new List<NavigationItem>();

            if (item == null || depth < 0) { return null; }

            if (includeSelf)
            {
                result.Add(GetNavigationItem(item, 0));
            }

            if (!item.HasChildren || depth < 1 ) { return result; }
            
            var items = (filterChildren != null) ? 
                item.Children.Where(filterChildren).ToArray() : 
                item.Children.ToArray();

            if (items == null || items.Length == 0) { return result; }

            foreach (Sitecore.Data.Items.Item i in items)
            {
                result.Add(GetNavigationItem(i, depth, filterChildren));
            }

            return result;
        }

        public static NavigationItem GetNavigationItem(
            Item item,
            int depth = 0,
            Func<Item, bool> filterChildren = null)
        {
            if (item == null || depth < 0) { return null; }

            List<NavigationItem> Children = new List<NavigationItem>();

            if (depth > 0 && item.HasChildren)
            {
                Children = GetNavigationItems(item, depth - 1, filterChildren);
            }

            return new NavigationItem(item, Children);
        }
    }
}