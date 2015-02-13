using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.ItemExtensions;
using System.Xml.Serialization;
using Sitecore.Mvc.Presentation;

namespace LaunchSitecoreMvc.Models
{
    public class RelatedItemsList : Sitecore.Mvc.Presentation.RenderingModel
    {
        protected const string PreReqArticlesFieldName = "Prerequisite Articles";
    
        [XmlIgnore]
        public Item[] DisplayRelatedItems { get; protected set; }

        [XmlIgnore]
        public Item[] DisplayDeepItems { get; protected set; }

        public override void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            base.Initialize(rendering);
            LoadRelatedItems();
            LoadReferencingItems();
        }

        private void LoadRelatedItems()
        {
            var relatedItems = Item.GetListItems(PreReqArticlesFieldName);
            if (relatedItems != null && relatedItems.Count > 0)
            {
                DisplayRelatedItems = relatedItems.ToArray();
            }
            else
            {
                DisplayRelatedItems = new Item[] { };
            }
        }

        private void LoadReferencingItems()
        {
            var referenceQueryTextItem = new SiteQueriesTextItem(SiteQueriesTextItem.GetFurtherReadingQueryTextKey);
            var referenceQueryString = string.Format(referenceQueryTextItem.Raw(), Item.ID.ToString());
            var referenceItems = PageContext.Current.Database.SelectItems(referenceQueryString);
            DisplayDeepItems = referenceItems.ToArray();
        }

    }
}