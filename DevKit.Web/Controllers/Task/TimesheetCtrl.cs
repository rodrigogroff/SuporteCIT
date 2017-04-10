﻿using DataModel;
using System.Net;
using System.Web.Http;

namespace DevKit.Web.Controllers
{
	public class TimesheetController : ApiControllerBase
	{
		public IHttpActionResult Get()
		{
			using (var db = new DevKitDB())
			{
				var filter = new TimesheetFilter()
				{
					nuYear = Request.GetQueryStringValue<long?>("nuYear", null),
					nuMonth = Request.GetQueryStringValue<long?>("nuMonth", null),
					fkUser = Request.GetQueryStringValue<long?>("fkUser", null),
				};

				var mdl = new Timesheet();
				var dto = mdl.ComposedFilters(db, filter);

				return Ok(dto);				
			}
		}
	}
}