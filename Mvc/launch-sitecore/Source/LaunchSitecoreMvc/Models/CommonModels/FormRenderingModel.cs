using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Sitecore.Mvc;
using Sitecore.Mvc.Common;
using Glimpse.Core.Extensibility;
using LaunchSitecoreMvc.Common;

namespace LaunchSitecoreMvc.Models
{
    public class FormRenderingModel : RenderingModel
    {
        public FormRenderingModel()
        {
            FormRenderingId = Guid.Empty;
        }

        public virtual Guid FormRenderingId { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        protected FormControllerContext MatchingFormControllerContext 
        { 
            get
            {
                var result = FormControllerContext.Current;
                return (result != null && FormRenderingId.Equals(result.FormRenderingId)) ? result : null;
            }
        }

        
        [System.Xml.Serialization.XmlIgnore]
        protected Guid CurrentFormRenderingId
        {
            get
            {
                return Rendering.UniqueId;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        protected ViewContext CurrentViewContext
        {
            get
            {
                return ContextService.Get().GetCurrentOrDefault<ViewContext>();
            }
        }

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);

            try 
            {
                initializeForm();
            }
            catch (Exception ex)
            {
                Glimpse.Core.Extensibility.GlimpseTrace.Error("{0} {1}", ex.Message, ex.StackTrace);
            }
        }

        private void initializeForm()
        {
            // FormRenderingId = Guid.Empty;
            FormRenderingId = CurrentFormRenderingId;

            GlimpseTrace.Info(string.Format("{0} FormRenderingId: {1}", GetType().Name, FormRenderingId));
            
        }

        
    }
}