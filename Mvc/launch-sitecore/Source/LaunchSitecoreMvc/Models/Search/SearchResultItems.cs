using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaunchSitecoreMvc.Models
{
    public class SearchResultItems
    {
        public Sitecore.Data.Items.Item[] Items { get; protected set; }
        public string SearchIndex { get; set; }
        public int? PageIndex { get; set; }
        public int? PageTotal { get; set; }
        public int? PageItemCount { get; set; }
        public int? ItemsTotal { get; set; }

        public SearchResultItems(Sitecore.Data.Items.Item[] items)
        {
            Items = items;
        }

        
    }
}