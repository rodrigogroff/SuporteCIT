﻿using DataModel;
using System.Net;
using System.Web.Http;

namespace DevKit.Web.Controllers
{
	public class TaskTypeController : ApiControllerBase
	{
		public IHttpActionResult Get()
		{
            if (!AuthorizeAndStartDatabase())
                return BadRequest();

            var mdl = new TaskType();

			var results = mdl.ComposedFilters(db, ref count, new TaskTypeFilter
			{
				skip = Request.GetQueryStringValue("skip", 0),
				take = Request.GetQueryStringValue("take", 15),
				busca = Request.GetQueryStringValue("busca")?.ToUpper(),
                fkProject = Request.GetQueryStringValue<long?>("fkProject", null)
			});

			return Ok(new { count = count, results = results });
		}
		
		public IHttpActionResult Get(long id)
		{
            if (!AuthorizeAndStartDatabase())
                return BadRequest();

            var model = db.GetTaskType(id);

            if (model != null)
            {
                var combo = Request.GetQueryStringValue("combo", false);

                if (combo)
                    return Ok(model);

                return Ok(model.LoadAssociations(db, new loaderOptionsTaskType(setupTaskType.TaskTypeEdit)));
            }

            return StatusCode(HttpStatusCode.NotFound);
		}

		public IHttpActionResult Post(TaskType mdl)
		{
            if (!AuthorizeAndStartDatabase(mdl.login))
                return BadRequest();

            if (!mdl.Create(db, ref serviceResponse))
				return BadRequest(serviceResponse);
            
            return Ok();
		}

		public IHttpActionResult Put(long id, TaskType mdl)
		{
            if (!AuthorizeAndStartDatabase(mdl.login))
                return BadRequest();

            if (!mdl.Update(db, ref serviceResponse))
				return BadRequest(serviceResponse);

            SetupCacheReport(CacheObject.TaskReports).Clear();

            return Ok();			
		}

		public IHttpActionResult Delete(long id)
		{
            if (!AuthorizeAndStartDatabase())
                return BadRequest();
            
			var model = db.GetTaskType(id);

			if (model == null)
				return StatusCode(HttpStatusCode.NotFound);

			if (!model.CanDelete(db, ref serviceResponse))
				return BadRequest(serviceResponse);
				
			model.Delete(db);
				
			return Ok();			
		}
	}
}
