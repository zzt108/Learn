using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using System.Xml.Serialization;
using Sitecore.Mvc.Presentation;
using Sitecore.ItemExtensions;


namespace LaunchSitecoreMvc.Models
{
    public class GlossaryList : RenderingModel
    {
        private const string HideItemFromMenuFieldName = "Hide Item From Menu";
    
        [XmlIgnore]
        public Item[] DisplayItems { get; protected set; }

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);

            DisplayItems = DisplayItems ?? GetItemChildren();
        }

        private Sitecore.Data.Items.Item[] GetItemChildren()
        {
            return Item.Children.Where(
                  c => !c.GetCheckBoxFieldValue(HideItemFromMenuFieldName)).ToArray();
        }

    }
}