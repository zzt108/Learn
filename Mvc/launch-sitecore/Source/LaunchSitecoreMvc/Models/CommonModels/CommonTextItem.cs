using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc;
using Sitecore.Mvc.Presentation;

namespace LaunchSitecoreMvc.Models
{
    public class CommonTextItem : TextItem
    {
        protected override string rootTextPath { get { return string.Concat(base.rootTextPath, "Common Text/"); } }

        public CommonTextItem(string CommonText) : base(CommonText)
        {
        }

        public  const string ReadMoreTextKey = "Read More";
        
    }
}