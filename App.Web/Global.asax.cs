﻿using System.Web.Optimization;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Web
{
	public class Application : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			// Map attributes
			RouteTable.Routes.MapMvcAttributeRoutes();

			//Upload de arquivos feito pela url /Backload/UploadHandler
			RouteTable.Routes.MapRoute("UploadHandler", "Backload/FileHandler", new { controller = "Backload", action = "FileHandler" });

			RouteTable.Routes.MapRoute(
				name: "Default",
				url: "{*url}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
