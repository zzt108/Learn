using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaunchSitecoreMvc.Helpers
{
    public class CustomerID
    {
        public static string Get(Sitecore.Data.Items.Item item)
        {
            return item.ID.ToShortID().ToString();
        }

        public static Sitecore.Data.ID Set(string customerId)
        {
            Sitecore.Data.ID result = null;
            Sitecore.Data.ShortID shortId = null;

            if (Sitecore.Data.ShortID.TryParse(customerId, out shortId))
            {
                result = shortId.ToID();
            }

            return result;
        }

    }
}