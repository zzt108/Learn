using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System.Threading;

namespace LaunchSitecoreMvc.Controllers
{
    public class SearchIndexController : StandardController
    {
        public override ActionResult Index()
        {
            return base.Index();
        }

        public ActionResult TestSearchIndex(string indexName)
        {
            
            string result = false.ToString();
            try
            {
                var searchIndex = Sitecore.Search.SearchManager.GetIndex(indexName);
                result = RunTest(searchIndex);
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Glimpse.Core.Extensibility.GlimpseTrace.Error(result);
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, GetType());
            }

            var contentRendering = new ContentRendering()
                {
                    Id = new Guid(),
                    UniqueId = new Guid(),
                    Placeholder = "main-content",
                    DeviceId = PageContext.Current.Device.Id
                };

            contentRendering.Content = string.Format(
                 "<h3>{0} - {1} Action - indexName: {2}</h3><code>Result: {3}</code>",
                 GetType().Name,
                 "TestSearchIndex",
                 indexName,
                 result);

            var currentRenderings = PageContext.Current.PageDefinition.Renderings;
            currentRenderings.Add(contentRendering);
            
            return Index();
        }


        public ActionResult RebuildSearchIndex(string indexName, string indexAction)
        {   
            string result = false.ToString();

            try
            {
                if (!string.IsNullOrEmpty(indexAction) 
                    && indexAction.Equals("Run Rebuild", StringComparison.InvariantCultureIgnoreCase) 
                    && RunRebuild(indexName))
                {
                    result = "Rebuild Started";
                }
                else if (rebuildRunning)
                {
                    result = "Rebuild Running";
                }
                else
                {
                    result = "Rebuild Stopped";
                }
                
            }
            catch(Exception ex)
            {
                result = ex.ToString();
                Glimpse.Core.Extensibility.GlimpseTrace.Error(result);
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, GetType());
            }

            var currentRenderings = PageContext.Current.PageDefinition.Renderings;

            AddActionButtonsContentRendering(currentRenderings);

            AddResultContentRendering(indexName, result, currentRenderings);


            return Index();
        }

        private static void AddActionButtonsContentRendering(List<Rendering> currentRenderings)
        {
            var contentActionRendering = new ContentRendering()
            {
                Id = new Guid(),
                UniqueId = new Guid(),
                Placeholder = "main-content",
                DeviceId = PageContext.Current.Device.Id
            };
            contentActionRendering.Content = string.Format(
                "<form><input type=\"submit\" name=\"indexAction\" value=\"Refresh\"> <input type=\"submit\" name=\"indexAction\" value=\"Run Rebuild\"></form>");

            currentRenderings.Add(contentActionRendering);
        }

        private void AddResultContentRendering(string indexName, string result, List<Rendering> currentRenderings)
        {
            var contentRendering = new ContentRendering()
            {
                Id = new Guid(),
                UniqueId = new Guid(),
                Placeholder = "main-content",
                DeviceId = PageContext.Current.Device.Id
            };
            contentRendering.Content = string.Format(
                "<h3>{0} - {1} Action - indexName: {2}</h3><code>Result: {3}</code>",
                GetType().Name,
                "RebuildSearchIndex",
                indexName,
                result);
            currentRenderings.Add(contentRendering);
        }



        private static string RunTest(Sitecore.Search.Index searchIndex)
        {
            string result = false.ToString();
            Sitecore.Diagnostics.Assert.IsNotNull(searchIndex, "Search Index Is Null");
            using (Sitecore.Search.IndexSearchContext searchContext = searchIndex.CreateSearchContext())
            {
                var hits = searchContext.Search("+_lastestversion:\"1\"", 1);
                var resultCollection = hits.FetchResults(0, 1);
                var resultHit = resultCollection.FirstOrDefault();
                Sitecore.Diagnostics.Assert.IsNotNull(resultHit, "Search Hit Is Null");
                var resultItem = resultHit.GetObject<Sitecore.Data.Items.Item>();
                if (resultItem != null)
                {
                    result = resultItem.Paths.FullPath;
                }
            }
            return result;
        }


        protected static object rebuildLock = new object();
        protected static bool rebuildRunning = false;
        protected static string rebuildIndexName = null;

        protected static Thread t = null;

        private static bool RunRebuild(string indexName)
        {
            var result = false;
            if (!rebuildRunning)
            {
                lock (rebuildLock)
                {
                    if (!rebuildRunning)
                    {
                        try
                        {
                            rebuildRunning = true;
                            rebuildIndexName = indexName;
                            t = new Thread(RebuildIndexThread);
                            t.Start();                            
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            Glimpse.Core.Extensibility.GlimpseTrace.Error(ex.Message);
                            rebuildRunning = false;
                            throw ex;
                        }
                        finally
                        {                   
                        }
                    }
                }
            }
            return result;
        }

        private static void RebuildIndexThread()
        {
            try
            {
                var idx = Sitecore.Search.SearchManager.GetIndex(rebuildIndexName);
                idx.Rebuild();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error Rebuilding Index In Background Thread", ex, typeof(SearchIndexController));
            }
            finally
            {
                rebuildRunning = false;
            }
        }

    }
}
