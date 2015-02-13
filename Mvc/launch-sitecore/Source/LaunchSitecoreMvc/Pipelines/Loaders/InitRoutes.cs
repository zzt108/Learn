using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Pipelines;
using System.Web.Routing;
using Sitecore.Mvc.Configuration;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using Sitecore.Mvc.Extensions;

namespace LaunchSitecoreMvc.Pipelines.Loaders
{
    public class InitRoutes : Sitecore.Mvc.Pipelines.Loader.InitializeRoutes
    {
        protected static readonly string tagsRouteName = string.Format(
                "{0}_tags", 
                MvcSettings.SitecoreRouteName);

        protected static readonly string glossaryRouteName = string.Format(
                "{0}_fakeGlossary",
                MvcSettings.SitecoreRouteName);
        protected static readonly string customRouteName = string.Format(
                "{0}_custom",
                MvcSettings.SitecoreRouteName);
        protected static readonly string searchIndexRouteName = string.Format(
                "{0}_searchIndex",
                MvcSettings.SitecoreRouteName);
        protected override void RegisterRoutes(System.Web.Routing.RouteCollection routes, PipelineArgs args)
        {

            MapTagRoute(routes);

            // ~/fakeGlossary/Item
            // ~/fakeGlossary/Content%20Editor
            //MapFakeGlossary(routes, glossaryRouteName);

            MapCustomerController(routes);
            MapSearchIndexController(routes);
            MapCustomController(routes);

            // base.RegisterRoutes(routes, args);

            RouteCollectionExtensions.MapRoute(routes, MvcSettings.SitecoreRouteName, "{*pathInfo}", (object)new
            {
                scIsFallThrough = true
            }, (object)new
            {
                isContent = new IsContentUrlRestraint()
            });
            SetDefaultValues(routes, args);
            SetRouteHandlers(routes, args);

            
        }

        private static void MapCustomerController(System.Web.Routing.RouteCollection routes)
        {
            // test
            // ~/customer/details/999999/customers/profile/
            // ~/customer/edit/999999/customers/profile/edit/
            // ~/customer/edit/000000/customers/profile/edit/
            // ~/customer/delete/000000/customers/delete/
            System.Web.Mvc.RouteCollectionExtensions.MapRoute(routes, "CustomerRoute", "customer/{action}/{customerId}/{*scItemPath}",
                new
                {
                    controller = "Customer",
                    scKeysToIgnore = new string[] { "customerId" }
                },
                new
                {
                    //isValidCustomerId = new isValidCustomerId() 
                }
            );
        }

        private static void MapSearchIndexController(System.Web.Routing.RouteCollection routes)
        {
            // ~/searchIndex/testsearchindex/{indexName}
            // ~/searchIndex/rebuildsearchindex/{indexName}
            System.Web.Mvc.RouteCollectionExtensions.MapRoute(routes, searchIndexRouteName, "searchIndex/{action}/{indexName}",
                new
                {
                    controller = "SearchIndex",
                    scItemPath = "/sitecore/content/home/customcontroller"
                },
                new
                {
                    //isValidCategory = new isValidId() 
                }
            );
        }

        private static void MapCustomController(System.Web.Routing.RouteCollection routes)
        {
            // ~/custom/test/one
            // ~/custom/test/two
            System.Web.Mvc.RouteCollectionExtensions.MapRoute(routes, customRouteName, "custom/{action}/{id}",
                new
                {
                    controller = "Custom",
                    scItemPath = "/sitecore/content/home/customcontroller"
                },
                new
                {
                    //isValidCategory = new isValidId() 
                }
            );
        }

        private static void MapFakeGlossary(System.Web.Routing.RouteCollection routes)
        {
            // doesn't recognize item url replacements like - to [space]
            // why do the Html.BeginForm() extensions pick up this route?
            // answer: use Html.BeginFormRoute()
            System.Web.Mvc.RouteCollectionExtensions.MapRoute(routes, glossaryRouteName, "fakeglossary/{glossaryTerm}",
                new
                {
                    glossaryTerm = "",
                    scItemPath = "/sitecore/content/home/glossary/{glossaryTerm}"
                },
                new { }
            );
        }

        private static void MapTagRoute(System.Web.Routing.RouteCollection routes)
        {
            System.Web.Mvc.RouteCollectionExtensions.MapRoute(routes, tagsRouteName, "tags/{tagName}",
                new
                {
                    tagName = "",
                    controller = "Tags",
                    action = "showTag",
                    scItemPath = "/sitecore/content/home/tags"
                },
                new
                {

                }
            );
        }
    }

    public class isValidId : IRouteConstraint
    {
        public bool Match(
            HttpContextBase httpContext, 
            Route route, 
            string parameterName, 
            RouteValueDictionary values, 
            RouteDirection routeDirection)
        {
            string str = values["id"] as string;
            if (String.IsNullOrEmpty(str))
            {
                return false;
            }
            else
            {
                // run better check for id existance
                return true;
            }
        }
    }



    public class IsContentUrlRestraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (routeDirection == RouteDirection.IncomingRequest)
            {
                string str = httpContext.Items[(object)"sc::IsContentUrl"] as string;
                if (str == null)
                    return false;
                else
                    return StringExtensions.ToBool(str);
            }
            else
            {
                return true;
            }
        }
    }

}