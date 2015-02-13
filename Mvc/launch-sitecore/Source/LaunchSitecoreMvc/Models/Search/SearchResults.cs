using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Mvc.Common;
using Sitecore.Data.Items;

namespace LaunchSitecoreMvc.Models
{
    public class SearchResults : Sitecore.Mvc.Presentation.RenderingModel
    {
        public SearchTextItem SearchButtonText { get; protected set; }
        public SearchTextItem PrevButtonText { get; protected set; }
        public SearchTextItem NextButtonText { get; protected set; }
        public SearchTextItem PageDescriptionText { get; protected set; }
        public SearchTextItem LastUpdatedText { get; protected set; }
        public SearchTextItem SearchCriteriaText { get; protected set; }
        public SearchTextItem DisplayingResultsText { get; protected set; }

        public string SearchString { get; protected set; }
        public SearchResultItems SearchResultItems { get; protected set; }
        
        [System.Xml.Serialization.XmlIgnore]
        protected ViewContext CurrentViewContext
        {
            get
            {
                return ContextService.Get().GetCurrentOrDefault<ViewContext>();
            }
        }


        public override void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            base.Initialize(rendering);
            try
            {            
                SearchButtonText = new SearchTextItem(SearchTextItem.SearchButtonTextKey);
                PrevButtonText = new SearchTextItem(SearchTextItem.PrevButtonTextKey);
                NextButtonText = new SearchTextItem(SearchTextItem.NextButtonTextKey);
                PageDescriptionText = new SearchTextItem(SearchTextItem.PageDescriptionKey);
                LastUpdatedText = new SearchTextItem(SearchTextItem.LastUpdatedKey);
                SearchCriteriaText = new SearchTextItem(SearchTextItem.SearchCriteriaKey);
                DisplayingResultsText = new SearchTextItem(SearchTextItem.DisplayingResultsKey);

                SearchString = CurrentViewContext.ViewData["SearchString"] as string;
                SearchResultItems = CurrentViewContext.ViewData["SearchResultItems"] as SearchResultItems ?? new SearchResultItems(new Item[] { });            
            }
            catch (Exception ex)
            {
                Glimpse.Core.Extensibility.GlimpseTrace.Error(ex.Message);
            }

        }

    }
}