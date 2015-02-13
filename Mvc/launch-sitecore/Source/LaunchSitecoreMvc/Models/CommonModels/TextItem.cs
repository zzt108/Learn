using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc;
using Sitecore.Mvc.Presentation;

namespace LaunchSitecoreMvc.Models
{
    public abstract class TextItem
    {

        public const string TextFieldName = "Text";

        protected virtual string rootTextPath { get { return "/sitecore/content/Configuration/"; } }

        public virtual string TextItemName { get; private set; }

        public virtual Sitecore.Data.Items.Item DisplayItem { get; private set; }
        
        public virtual string FullTextItemPath { get { return rootTextPath + TextItemName; } }

        protected virtual string ErrorResultString { get { return string.Format("[Text Item Error {0}({1})]", GetType(), FullTextItemPath); } }
        
        public TextItem(string textItemName)
        {
            TextItemName = textItemName;
            Init();
        }
        
        protected virtual void Init()
        {
            DisplayItem = PageContext.Current.Database.SelectSingleItem(FullTextItemPath);
        }
        
        public HtmlString Text
        {
            get
            {
                return PageContext.Current.HtmlHelper.Sitecore().Field(TextFieldName, DisplayItem, new { DisableWebEdit = true });
            }
        }

        public HtmlString FormatText(params object[] args)
        {
            if (DisplayItem != null)
            {
                return new HtmlString(string.Format(DisplayItem[TextFieldName], args));
            }
            else
            {
                return new HtmlString(ErrorResultString);
            }
        }

        public string Raw()
        {
            if (DisplayItem != null)
            {
                return DisplayItem[TextFieldName];
            }
            else
            {
                return ErrorResultString;
            }
        }

        public override string ToString()
        {
            return Raw();
        }

    }
}