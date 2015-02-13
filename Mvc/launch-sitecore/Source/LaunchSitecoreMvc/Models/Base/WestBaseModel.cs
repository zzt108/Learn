namespace Sitecore.Sharedsource.Mvc.Models
{
    using System.Web;
    using System.Web.Mvc;

    using SC = Sitecore;

    public class ItemViewModel : SC.Mvc.Presentation.IRenderingModel
    {
        private HtmlHelper _htmlHelper;
        private SC.Mvc.Helpers.SitecoreHelper _scHelper;

        public void Initialize(SC.Mvc.Presentation.Rendering rendering)
        {
            ViewContext current =
              SC.Mvc.Common.ContextService.Get().GetCurrent<ViewContext>();
            this._htmlHelper = new HtmlHelper(
              current,
              new SC.Mvc.Presentation.ViewDataContainer(current.ViewData));
            this._scHelper = SC.Mvc.HtmlHelperExtensions.Sitecore(
              this._htmlHelper);
        }

        protected HtmlString RenderField(
          string fieldName,
          bool disableWebEdit = false,
          SC.Collections.SafeDictionary<string> parameters = null)
        {
            if (parameters == null)
            {
                parameters = new SC.Collections.SafeDictionary<string>();
            }

            return this._scHelper.Field(
              fieldName,
              new
              {
                  DisableWebEdit = disableWebEdit,
                  Parameters = parameters
              });
        }

        public HtmlString Created
        {
            get
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                 new System.Globalization.CultureInfo(SC.Context.Language.Name);
                System.Threading.Thread.CurrentThread.CurrentCulture =
                  System.Globalization.CultureInfo.CreateSpecificCulture(SC.Context.Language.Name);
                SC.Collections.SafeDictionary<string> parameters =
                  new SC.Collections.SafeDictionary<string>();
                parameters["format"] = "D";
                return this.RenderField(
                  SC.FieldIDs.Created.ToString(),
                  false,
                  parameters);
            }
        }
    }
}