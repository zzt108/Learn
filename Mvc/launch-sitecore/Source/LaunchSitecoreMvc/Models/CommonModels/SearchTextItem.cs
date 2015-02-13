using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc;
using Sitecore.Mvc.Presentation;

namespace LaunchSitecoreMvc.Models
{
    public class SearchTextItem : TextItem
    {
        protected override string rootTextPath { get { return string.Concat(base.rootTextPath, "Search/"); } }

        public SearchTextItem(string SearchText)
            : base(SearchText)
        {
        }

        public const string SearchButtonTextKey = "Search";
        public const string PrevButtonTextKey = "Previous Button";
        public const string NextButtonTextKey = "Next Button";
        public const string DidYouMeanKey = "Did You Mean";
        public const string LastUpdatedKey = "Last Updated";
        public const string NoCriteriaKey = "No Criteria";
        public const string NoResultsKey = "No Results";
        public const string PageDescriptionKey = "Page Description";
        public const string PageSizeKey = "Page Size";
        public const string SearchCriteriaKey = "Search Criteria";
        public const string SearchIndexKey = "Search Index";
        public const string DisplayingResultsKey = "Displaying Results";
                       
    }
}