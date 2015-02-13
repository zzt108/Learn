using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LaunchSitecoreMvc.Models;
using System.Web.Mvc;

namespace LaunchSitecoreMvc.Helpers
{
    public static class NavigationExtensions
    {
        public static HtmlString RenderNavigationItem(this HtmlHelper helper, NavigationItem navigationItem)
        {
            if (!string.IsNullOrEmpty(navigationItem.Href))
            {
                return new HtmlString(string.Format(
                    "<a href=\"{0}\" class=\"{1}\">{2}</a>",
                    navigationItem.Href,
                    string.Empty,
                    navigationItem.Title
                ));
            }
            else
            {
                return new HtmlString(string.Format(
                    "<span class=\"{1}\">{0}</span>",
                    navigationItem.Title,
                    string.Empty
                ));
            }
        }

        public static HtmlString RenderNavigationListItem(this HtmlHelper helper, NavigationItem navigationItem, int depth = 0, string cssClass = "ListItem")
        {
            HtmlString childrenRendering = helper.RenderNavigationList(navigationItem.Children.ToArray(), depth - 1);
            return new HtmlString(string.Format(
                   "<li class=\"{0}\">{1}{2}</li>\n",
                   navigationItem.CssClass,
                   helper.RenderNavigationItem(navigationItem),
                   childrenRendering
            ));
        }

        public static HtmlString RenderNavigationList(this HtmlHelper helper, NavigationItem[] navigationItems, int depth = 0, string cssClass = "NavigationList", string id = "")
        {
            if (navigationItems == null || navigationItems.Length == 0 || depth < 0) { return new HtmlString(string.Empty); }

            return new HtmlString(string.Format("<ul{0} class=\"{1}\">\n{2}</ul>\n",
                (!string.IsNullOrEmpty(id)) ? string.Format(" id={0}", id) : string.Empty,
                cssClass,
                string.Concat(
                    navigationItems.Select(i => helper.RenderNavigationListItem(i, depth)))
            ));

        }


    }
}