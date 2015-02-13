using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Diagnostics;
using Sitecore.Caching;
using Sitecore.Mvc.Extensions;
using Sitecore.Sites;

namespace LaunchSitecoreMvc.Pipelines.Response.RenderRendering
{
    public class TraceProcessor : Sitecore.Mvc.Pipelines.Response.RenderRendering.RenderRenderingProcessor
    {
        public override void Process(Sitecore.Mvc.Pipelines.Response.RenderRendering.RenderRenderingArgs args)
        {

            if (!args.Cacheable) { return; }

            try
            {
                // Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} - Cacheable: {1}", GetType().FullName, args.Cacheable);
                Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} - CacheKey: {1}", GetType().FullName, args.CacheKey);
                Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} - Rendering CacheKey: {1}", GetType().FullName, args.Rendering.Caching.CacheKey);
                //Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} - Rendered: {1}", GetType().FullName, args.Rendered);
            
                Debug.ArgumentNotNull((object)args, "args");
                Debug.ArgumentNotNull((object)args.CacheKey, "cacheKey");

                HtmlCache htmlCache = ObjectExtensions.ValueOrDefault<SiteContext, HtmlCache>(Sitecore.Context.Site, (Func<SiteContext, HtmlCache>)(site => CacheManager.GetHtmlCache(site)));
                if (htmlCache == null)
                {
                    Glimpse.Core.Extensibility.GlimpseTrace.Warn("{0} - htmlCache IS NULL", GetType().FullName);
                }
            
                //Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} - htmlCache Enabled: {1}", GetType().FullName, htmlCache.Enabled);

                string html = !string.IsNullOrEmpty(args.CacheKey) ? htmlCache.GetHtml(args.CacheKey) : null;
                if (html == null)
                {
                    Glimpse.Core.Extensibility.GlimpseTrace.Warn("{0} - htmlCache GetHtml IS NULL", GetType().FullName, args.CacheKey);
                }
                else
                {
                    Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} - htmlCache GetHtml FOUND", GetType().FullName, args.CacheKey);
                }
                                
                //var renderingProps = args.Rendering.Properties.Select(k => string.Format("{0}: {1}", k.Key, k.Value)).ToArray();

                //Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} - renderingProps: {1}", GetType().FullName, string.Join(", ", renderingProps));

            }
            catch (Exception ex)
            {
                Glimpse.Core.Extensibility.GlimpseTrace.Error("{0} - Exception: {1}", GetType().FullName, ex.Message);

            }

        }
    }
}