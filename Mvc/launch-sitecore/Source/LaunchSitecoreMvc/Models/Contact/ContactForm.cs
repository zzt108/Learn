using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Sitecore.Mvc.Presentation;

namespace LaunchSitecoreMvc.Models
{
    
    public class ContactForm : FormRenderingModel
    {
        [Display(Name = "First Name", ShortName="FirstName")]
        [Required(AllowEmptyStrings = false), StringLength(60)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false), StringLength(60)]
        public string LastName { get; set; }
        
        [Display(Name = "Email Address")]
        [Required(AllowEmptyStrings = false), StringLength(250)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Ask a question")]
        [DataType(DataType.MultilineText)]
        public string Question { get; set; }

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);

            ContactForm modelFromViewData = CurrentViewContext.ViewData["contactFormModel"] as ContactForm;

            if (modelFromViewData != null)
            {
                FirstName = modelFromViewData.FirstName;
                LastName = modelFromViewData.LastName;
                Email = modelFromViewData.Email;
            }

        }
    }
}