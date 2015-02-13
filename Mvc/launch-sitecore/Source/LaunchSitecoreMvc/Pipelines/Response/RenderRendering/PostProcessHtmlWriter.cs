using Sitecore.Mvc.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaunchSitecoreMvc.Pipelines.Response.RenderRendering
{
    public class PostProcessHtmlWriter : Sitecore.Mvc.Pipelines.Response.RenderRendering.RenderRenderingProcessor
    {
        public override void Process(Sitecore.Mvc.Pipelines.Response.RenderRendering.RenderRenderingArgs args)
        {
            RecordingTextWriter recordingTextWriter = args.Writer as RecordingTextWriter;
            if (recordingTextWriter == null)
                return;
            string recording = recordingTextWriter.GetRecording();
      
        }
    }
}