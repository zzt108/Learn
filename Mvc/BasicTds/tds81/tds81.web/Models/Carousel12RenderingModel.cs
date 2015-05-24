using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace tds81.web.Models
{
    public class Carousel12RenderingModel : Sitecore.Mvc.Presentation.RenderingModel
    {
      public override void Initialize(Rendering rendering)
      {
        base.Initialize(rendering);
        CarouselSlides =
          Item.Children.ToList(); //.Select(id => Item.Database.GetItem(id)).ToList();
      }

      public IList<Item> CarouselSlides { get; private set; }
    }
}