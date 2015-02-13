using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using LaunchSitecoreMvc.Models;
using Sitecore.Data.Items;
using Sitecore.Search;
using Sitecore.Mvc.Presentation;
using Sitecore.ItemExtensions;

namespace LaunchSitecoreMvc.Controllers
{
    public class SearchController : StandardController
    {
        public SearchController()
        {
            GlimpseTrace.Info("{0} Constructor", "SearchController");
        }


        // Base Item Controller Action
        public override ActionResult Index()
        {
            GlimpseTrace.Info("{0} Index()", "SearchController");       
            return base.Index();
        }


        // Item Controller Action
        public ActionResult Submit(string SearchString)
        {
            GlimpseTrace.Info("{0} Action: {1}", GetType().Name, "SearchForm");
            GlimpseTrace.Info("{0} SearchString: {1}", GetType().Name, SearchString);

            if (!string.IsNullOrEmpty(SearchString))
            {
                RouteData.Values["SearchString"] = SearchString;
            }

            return null;
        }

        // Item Controller Action
        public ActionResult Search(string SearchString, string PageIndex)
        {
            GlimpseTrace.Info("{0} Action: {1}", GetType().Name, "Search");
            GlimpseTrace.Info("{0} SearchString: {1}", GetType().Name, SearchString);
            
            ViewData["SearchString"] = string.Empty;
            ViewData["SearchResultItems"] = new SearchResultItems(new Item[] { });

            try
            {
                if (!string.IsNullOrEmpty(SearchString))
                {

                    ViewData["SearchString"] = SearchString;

                    var searchIndexName = new SearchTextItem(SearchTextItem.SearchIndexKey);
                    var searchPageSize = new SearchTextItem(SearchTextItem.PageSizeKey);
                    var rootItem = GetSiteRoot();

                    var searchString = GetSearchString(SearchString); 
                    var searchIndexNameTxt = searchIndexName.Raw();
                    var pageSize = Convert.ToInt32(searchPageSize.Raw());
                    pageSize = (pageSize > 0) ? pageSize : 10;
                    var pageIndex = Convert.ToInt32(PageIndex);
                    pageIndex = (pageIndex > 0 && pageIndex < 25) ? pageIndex : 0;
                    var totalResults = 0;
                    var resultsList = new List<Item>();

                    GlimpseTrace.Info("{0} searchIndexNameTxt: {1}", GetType().Name, searchIndexNameTxt);
                    GlimpseTrace.Info("{0} searchString: {1}", GetType().Name, searchString);
                    GlimpseTrace.Info("{0} pageIndex: {1}", GetType().Name, pageIndex);
                    GlimpseTrace.Info("{0} pageSize: {1}", GetType().Name, pageSize);
                    GlimpseTrace.Info("{0} rootItem: {1}", GetType().Name, rootItem.Paths.FullPath);

                    totalResults = GetSearchResults(resultsList, searchIndexNameTxt, searchString, pageSize, pageIndex, rootItem);
                                       
                    ViewData["SearchResultItems"] = new SearchResultItems(resultsList.ToArray())
                    {
                        PageIndex = pageIndex,
                        PageTotal = GetPageTotal(pageSize, totalResults),
                        PageItemCount = pageSize,
                        ItemsTotal = totalResults
                    };

                }
                else
                {
                    ModelState.AddModelError("SearchString", "Search String Is Null or Empty");
                }
            }
            catch (Exception ex)
            {
                GlimpseTrace.Error("{0} -- {1}", ex.Message, ex.StackTrace);
            }

            //return View(PageContext.Current.PageView, null);
            return Index();
        }

        private static int GetPageTotal(int pageSize, int totalResults)
        {
            return (totalResults > pageSize) ? 
                Convert.ToInt32(Math.Ceiling((double)totalResults / (double)pageSize)) : 1;
        }

        private static string GetSearchString(string SearchString)
        {
            //var searchString = string.Format("+_content:\"{0}\" +_language:\"{1}\" +_lastestversion:\"1\"", SearchString, PageContext.Current.Item.Language.Name).ToLower();
            var searchString = string.Format("{0}", SearchString).ToLower();
            return searchString;
        }

        private int GetSearchResults(List<Item> resultsList, string searchIndexNameTxt, string searchString, int pageSize, int pageIndex, Item rootItem)
        {
            var searchIndex = SearchManager.GetIndex(searchIndexNameTxt);
            var totalResults = 0;
            using (IndexSearchContext context = searchIndex.CreateSearchContext())
            {
                SearchHits hits = context.Search(searchString, 999, new SearchContext(rootItem));
                //SearchHits hits = context.Search(searchString);

                totalResults = hits.Length;
                GlimpseTrace.Info("{0} totalResults: {1}", GetType().Name, totalResults);

                var results = hits.FetchResults(pageIndex * pageSize, pageSize);
                foreach (SearchResult result in results)
                {
                    GetItemFromSearchResult(resultsList, result);
                }
            }
            return totalResults;
        }

        private static void GetItemFromSearchResult(List<Item> resultsList, SearchResult result)
        {
            Item hit = result.GetObject<Item>();
            if (hit != null)
            {
                resultsList.Add(hit);
            }
        }

        private static Item GetSiteRoot()
        {
            var rootItem = PageContext.Current.Item.GetDropLinkItem("Menu Root");
            return rootItem;
        }
 
    }
}
