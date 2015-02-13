using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Xml.Serialization;

namespace LaunchSitecoreMvc.Models
{
    public class FeaturedAbstractList : AbstractItemList
    {
        [XmlIgnore]
        public Item FeaturedTitleItem { get; protected set; }

        public override void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {

            DisplayItems = DisplayItems ?? null;

            var featuredItemsList = new List<Item>();

            try
            {
                var FeaturedItemsTitleParam = string.Empty;
                FeaturedItemsTitleParam = rendering.Parameters["Title"];
                FeaturedTitleItem = PageContext.Current.Database.SelectSingleItem(FeaturedItemsTitleParam);
            }
            catch (Exception)
            {
                // Write(new HtmlString("<h6>" + ex.Message + "</h6>"));
                FeaturedTitleItem = PageContext.Current.Database.SelectSingleItem("{338707BF-3C38-4C1E-9986-29EF2CF2A2BF}");
            }

            try
            {
                var FeaturedItemsListParam = string.Empty;
                FeaturedItemsListParam = rendering.Parameters["Items"];
                var splitItems = FeaturedItemsListParam.Split(new char[] { '|' });

                foreach (var itemId in splitItems)
                {
                    var tempItem = PageContext.Current.Database.SelectSingleItem(itemId);
                    if (tempItem != null)
                    {
                        featuredItemsList.Add(tempItem);
                    }
                }

            }
            catch (Exception)
            {
                // Write(new HtmlString("<h6>" + ex.Message + "</h6>"));
            }

            DisplayItems = featuredItemsList.Select(
                    i => new AbstractItem(null, i)).ToArray();

            base.Initialize(rendering);
        }
    }
}