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
    public class CustomExecuteRenderer : Sitecore.Mvc.Pipelines.Response.RenderRendering.ExecuteRenderer
    {
        public override void Process(Sitecore.Mvc.Pipelines.Response.RenderRendering.RenderRenderingArgs args)
        {
            base.Process(args);
            args.Rendered = true;
        }
    }
}