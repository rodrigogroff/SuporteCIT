﻿using DataModel;
using System.Net;
using System.Web.Http;

namespace DevKit.Web.Controllers
{
	public class TaskCategoryController : ApiControllerBase
	{
		public IHttpActionResult Get()
		{
            if (!StartDatabaseAndAuthorize())
                return BadRequest();

            var mdl = new TaskCategory();

			var results = mdl.ComposedFilters(db, ref count, new TaskCategoryFilter()
			{
				skip = Request.GetQueryStringValue("skip", 0),
				take = Request.GetQueryStringValue("take", 15),
				busca = Request.GetQueryStringValue("busca")?.ToUpper(),
				fkTaskType = Request.GetQueryStringValue<long?>("fkTaskType", null)
			});

			return Ok(new { count = count, results = results });			
		}

		public IHttpActionResult Get(long id)
		{
            if (!StartDatabaseAndAuthorize())
                return BadRequest();

            var mdl = db.GetTaskCategory(id);

            if (mdl != null)
                return Ok(mdl);

            return StatusCode(HttpStatusCode.NotFound);
		}
	}
}
