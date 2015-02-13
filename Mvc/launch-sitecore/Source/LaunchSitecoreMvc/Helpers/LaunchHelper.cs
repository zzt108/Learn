using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc.Helpers;
using System.Web.Mvc;
using LaunchSitecoreMvc.Models;
using Sitecore.Extensions.StringExtensions;
using Sitecore.ItemExtensions;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.RenderField;
using Sitecore.Pipelines;
using Sitecore.Xml.Xsl;
using Sitecore.Mvc.Extensions;
using Sitecore.Data.Items;
using System.Reflection;

namespace LaunchSitecoreMvc.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static LaunchHelper Launch(this HtmlHelper htmlHelper)
        {
            Sitecore.Diagnostics.Assert.ArgumentNotNull((object)htmlHelper, "htmlHelper");
            LaunchHelper threadData = Sitecore.Mvc.Helpers.ThreadHelper.GetThreadData<LaunchHelper>();
            if (threadData != null)
                return threadData;
            LaunchHelper data = new LaunchHelper(htmlHelper);
            Sitecore.Mvc.Helpers.ThreadHelper.SetThreadData<LaunchHelper>(data);
            return data;
        }

        // sample HtmlHelper extension
        public static string Echo(this HtmlHelper helper, string Echo)
        {
            return Echo;
        }

        public static string HiddenFormRenderingId(this HtmlHelper helper, string formRenderingId)
        {
            return string.Format("<input type=\"hidden\" name=\"FormRenderingId\" value=\"{0}\" />", formRenderingId);
        }


        public static HtmlString WriteFirstRow(this HtmlHelper helper, HtmlString write, int row)
        {
            var result = row == 0 ? write : new HtmlString(string.Empty);
            return result;
        }
    }

    public class LaunchHelper : Sitecore.Mvc.Helpers.SitecoreHelper
    {
        public LaunchHelper(HtmlHelper htmlHelper)
            : base(htmlHelper)
        {

        }

        public override HtmlString BeginField(string fieldName, Item item, object parameters)
        {
            Assert.ArgumentNotNull((object)fieldName, "fieldName");
            RenderFieldArgs renderFieldArgs = new RenderFieldArgs();
            renderFieldArgs.Item = item;
            renderFieldArgs.FieldName = fieldName;
            if (parameters != null)
            {
                TypeHelper.CopyProperties(parameters, renderFieldArgs);

                foreach (PropertyInfo propertyInfo in parameters.GetType().GetProperties())
                {
                    renderFieldArgs.Parameters.Add(propertyInfo.Name, propertyInfo.GetValue(parameters, (object[])null).ToStringOrEmpty());
                }
            }
            renderFieldArgs.Item = renderFieldArgs.Item ?? this.CurrentItem;
            if (renderFieldArgs.Item == null)
            {
                this.EndFieldStack.Push(string.Empty);
                return new HtmlString(string.Empty);
            }
            else
            {
                CorePipeline.Run("renderField", (PipelineArgs)renderFieldArgs);
                RenderFieldResult result = renderFieldArgs.Result;
                string str = Sitecore.Mvc.Extensions.StringExtensions.OrEmpty(ObjectExtensions.ValueOrDefault<RenderFieldResult, string>(result, (Func<RenderFieldResult, string>)(r => r.FirstPart)));
                this.EndFieldStack.Push(Sitecore.Mvc.Extensions.StringExtensions.OrEmpty(ObjectExtensions.ValueOrDefault<RenderFieldResult, string>(result, (Func<RenderFieldResult, string>)(r => r.LastPart))));
                return new HtmlString(str);
            }
        }

       
        

    }
}