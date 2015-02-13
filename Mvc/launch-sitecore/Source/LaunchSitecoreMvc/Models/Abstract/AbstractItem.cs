using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using Sitecore.Mvc;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System.Web.Routing;
using LaunchSitecoreMvc.Helpers;


namespace LaunchSitecoreMvc.Models
{
    public class AbstractItem : RenderingModel
    {
        public AbstractItem() 
            : this(new AbstractItemList()) { }

        public AbstractItem(AbstractItemList children)
            : this(children, null)
        {
        }

        public AbstractItem(Sitecore.Data.Items.Item item)
            : this(new AbstractItemList(), item)
        {
        }

        public AbstractItem(AbstractItemList relatedItems, Sitecore.Data.Items.Item modelItem)
            : base()
        {
            DisplayItem = modelItem;
            RelatedItems = relatedItems;
            ReadMoreTextItem = new CommonTextItem(CommonTextItem.ReadMoreTextKey);
            LoadData();
        }

        public HtmlString Title { get; private set; }
        public HtmlString Abstract { get; private set; }
        public HtmlString Image { get; private set; }
        
        [XmlIgnore]
        public Item DisplayItem { get; private set; }
        
        [XmlIgnore]
        public CommonTextItem ReadMoreTextItem { get; private set; }

        [XmlIgnore]
        public AbstractItemList RelatedItems { get; private set; }
        
        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
            DisplayItem = Rendering.Item;
            LoadData();
        }

        private void LoadData()
        {
            if (DisplayItem != null)
            {                
                Title = PageContext.Current.HtmlHelper.Sitecore().Field(TitleFieldName, DisplayItem);
                Abstract = PageContext.Current.HtmlHelper.Sitecore().Field(AbstractFieldName, DisplayItem);
                Image = PageContext.Current.HtmlHelper.Launch().Field(ImageFieldName, DisplayItem, new object { } );
            }
        }

        public const string TitleFieldName = "Title";
        public const string AbstractFieldName = "Abstract";
        public const string ImageFieldName = "Image";



    }
}