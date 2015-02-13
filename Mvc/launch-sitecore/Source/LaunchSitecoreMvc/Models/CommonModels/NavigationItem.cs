using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using Sitecore.Mvc;
using Sitecore.Mvc.Presentation;
using Sitecore.ItemExtensions;

namespace LaunchSitecoreMvc.Models
{
    public class NavigationItem
    {
        [XmlIgnore]
        public Sitecore.Data.Items.Item DisplayItem { get; protected set; }

        public List<NavigationItem> Children { get; protected set; }

        public HtmlString Title { get; set; }

        public string CssClass { get; set; }
        public string Target { get; set; }
        public string Href { get; set; }


        public NavigationItem()
            : this(null, new List<NavigationItem>()) { }

        public NavigationItem(Sitecore.Data.Items.Item item)
            : this(item, new List<NavigationItem>()) { }

        public NavigationItem(Sitecore.Data.Items.Item item, List<NavigationItem> children)
        {
            DisplayItem = item;
            Children = children;

            if (DisplayItem != null)
            {
                Init();
            }
        }
        
        protected void Init()
        {
            Title = PageContext.Current.HtmlHelper.Sitecore().Field(MenuTitleFieldName, DisplayItem, new { DisableWebEdit = true });

            Href = PageContext.Current.HtmlHelper.Sitecore().Field(MenuLinkFieldName, DisplayItem, new { DisableWebEdit = true }).ToString();
            CssClass = PageContext.Current.HtmlHelper.Sitecore().Field(MenuClassFieldName, DisplayItem, new { DisableWebEdit = true }).ToString();

            if (string.IsNullOrEmpty(Title.ToHtmlString()))
            {
                Title = new HtmlString(DisplayItem.DisplayName);
            }
            if (string.IsNullOrEmpty(Href))
            {
                Href = DisplayItem.GetLink();
            }
        }

        public const string MenuLinkFieldName = "Menu Link";
        public const string MenuTitleFieldName = "Menu Title";
        public const string MenuClassFieldName = "Menu Class";


    }
}