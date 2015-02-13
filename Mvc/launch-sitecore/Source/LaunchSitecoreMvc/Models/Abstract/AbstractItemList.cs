using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc;
using Sitecore.ItemExtensions;
using Sitecore.Mvc.Presentation;
using System.Xml.Serialization;

namespace LaunchSitecoreMvc.Models
{
    public class AbstractItemList : RenderingModel
    {
        public AbstractItemList()
            : this(null)
        {
        }

        public AbstractItemList(AbstractItem[] modelItems)
        {
            DisplayItems = modelItems;
        }

        [XmlIgnore]
        public AbstractItem[] DisplayItems { get; protected set; }
        
        [XmlIgnore]
        public CommonTextItem ReadMoreTextItem { get; protected set; }
 
        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);

            DisplayItems = DisplayItems ?? GetItemChildren();
                        
            ReadMoreTextItem = ReadMoreTextItem ?? new CommonTextItem(CommonTextItem.ReadMoreTextKey);

        }

        private AbstractItem[] GetItemChildren()
        {
            return Item.Children.Where(
                c => !c.GetCheckBoxFieldValue("Hide Item From Menu")).Select(
                    i => new AbstractItem(null, i)).ToArray();
        }
    }
}