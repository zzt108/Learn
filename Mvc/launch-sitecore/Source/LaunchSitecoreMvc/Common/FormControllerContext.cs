using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc.Common;

namespace LaunchSitecoreMvc.Common
{
    public class FormControllerContext
    {
        public Guid FormRenderingId { get; protected set; }
        
        public FormControllerContext(Guid formRenderingId)
        {
            FormRenderingId = formRenderingId;
        }

        public static FormControllerContext Get(Func<FormControllerContext> defaultValue)
        {
            return ContextService.Get().GetOrAdd<FormControllerContext>(defaultValue);
        }

        public static FormControllerContext Current
        {
            get
            {
                return Get(() => new FormControllerContext(Guid.Empty));                
            }
        }

    }
}