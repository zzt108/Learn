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
    public class CacheKeyCheckProcessor : Sitecore.Mvc.Pipelines.Response.RenderRendering.RenderRenderingProcessor
    {
        public override void Process(Sitecore.Mvc.Pipelines.Response.RenderRendering.RenderRenderingArgs args)
        {
            if (!args.Cacheable) { return; }

            try
            {
                Glimpse.Core.Extensibility.GlimpseTrace.Info("{0} - Check Rendering CacheKey: {1}", GetType().FullName, args.Rendering.Caching.CacheKey);

                if (string.IsNullOrEmpty(args.Rendering.Caching.CacheKey))
                {
                    args.Rendering.Caching.CacheKey = args.Rendering.UniqueId.ToGuidString();
                    Glimpse.Core.Extensibility.GlimpseTrace.Warn("{0} - Set Rendering CacheKey: {1}", GetType().FullName, args.Rendering.Caching.CacheKey);
                }

            }
            catch (Exception ex)
            {
                Glimpse.Core.Extensibility.GlimpseTrace.Error("{0} - Exception: {1}", GetType().FullName, ex.Message);

            }

        }
    }
}