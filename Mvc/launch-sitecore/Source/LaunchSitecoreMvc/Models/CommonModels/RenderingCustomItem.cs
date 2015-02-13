using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaunchSitecoreMvc.Models.CommonModels
{
    public abstract class RenderingCustomItem : IRenderingModel
    {
        public RenderingCustomItem()
        {

        }
        public Item InnerItem { get; private set; }

        public virtual void Initialize(Rendering rendering)
        {
            InnerItem = rendering.Item;
        }

        public ID ID
        {
            get
            {
                return InnerItem.ID;
            }
        }
        public string Name
        {
            get
            {
                return InnerItem.Name;
            }
        }
        public Database Database
        {
            get
            {
                return this.InnerItem.Database;
            }
        }

        public virtual string DisplayName
        {
            get
            {
                return InnerItem.Appearance.DisplayName;
            }
        }

        public virtual string Icon
        {
            get
            {
                return InnerItem.Appearance.Icon;
            }
        }

        public string this[ID fieldID]
        {
            get
            {
                return InnerItem[fieldID];
            }
            set
            {
                InnerItem[fieldID] = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of a specified field.
        /// 
        /// </summary>
        public string this[int fieldIndex]
        {
            get
            {
                return InnerItem[fieldIndex];
            }
        }

        /// <summary>
        /// Gets or sets the value of a specified field.
        /// 
        /// </summary>
        public string this[string fieldName]
        {
            get
            {
                return InnerItem[fieldName];
            }
            set
            {
                InnerItem[fieldName] = value;
            }
        }

        public void BeginEdit()
        {
            InnerItem.Editing.BeginEdit();
        }

        public void EndEdit()
        {
            InnerItem.Editing.EndEdit();
        }
    }
}