using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc;
using Sitecore.Mvc.Presentation;

namespace LaunchSitecoreMvc.Models
{
    public class SiteQueriesTextItem : TextItem
    {
        protected override string rootTextPath { get { return string.Concat(base.rootTextPath, "Site Queries/"); } }

        public SiteQueriesTextItem(string QueryTextKey)
            : base(QueryTextKey)
        {
        }

        public  const string GetFurtherReadingQueryTextKey = "Get Further Reading Query";
        
    }
}