using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Xml.Serialization;

namespace LaunchSitecoreMvc.Models
{
    public class EvaluatorList : RenderingModel
    {
        public static readonly Sitecore.Data.ID EvaluatorTemplateId = new Sitecore.Data.ID("{B65CCE04-BCCB-4A58-B988-753D523C99A7}");

        [XmlIgnore]
        public Item[] DisplayItems { get; protected set; }

        public override void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            base.Initialize(rendering);
            DisplayItems = Item.Children.Where(i => i.TemplateID.Equals(EvaluatorTemplateId)).ToArray();
        }

    }
}