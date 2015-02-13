using LaunchSitecoreMvc.Models.CommonModels;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaunchSitecoreMvc.Models.Test
{

    public class TestItem : RenderingCustomItem
    {
        public const string TitleFieldName = "Title";
        public const string AbstractFieldName = "Abstract";
        public const string ImageFieldName = "Image";

        public string Title
        {
            get
            {
                return InnerItem[TitleFieldName];
            }
            set
            {
                InnerItem[TitleFieldName] = value;
            }
        }

        public string Abstract
        {
            get
            {
                return InnerItem[AbstractFieldName];
            }
            set
            {
                InnerItem[AbstractFieldName] = value;
            }
        }

        public string Image
        {
            get
            {
                return InnerItem[ImageFieldName];
            }
            set
            {
                InnerItem[ImageFieldName] = value;
            }
        }
    }
    
}