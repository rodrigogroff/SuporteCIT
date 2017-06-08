﻿using DataModel;

using System.Net;
using System.Web.Http;

namespace DevKit.Web.Controllers
{
	public class TaskTypeController : ApiControllerBase
	{
		public IHttpActionResult Get()
		{
            if (!GetAuthorizationAndDatabase())
                return BadRequest();

            var count = 0; var mdl = new TaskType();

			var results = mdl.ComposedFilters(db, ref count, new TaskTypeFilter
			{
				skip = Request.GetQueryStringValue("skip", 0),
				take = Request.GetQueryStringValue("take", 15),
				busca = Request.GetQueryStringValue("busca")?.ToUpper(),
                fkCurrentUser = login.idUser,
                fkProject = Request.GetQueryStringValue<long?>("fkProject", null)
			});

			return Ok(new { count = count, results = results });
		}
		
		public IHttpActionResult Get(long id)
		{
            if (!GetAuthorizationAndDatabase())
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
            using (var db = new DevKitDB())
			{
				var resp = "";

				if (!mdl.Create(db, mdl.login.idUser, ref resp))
					return BadRequest(resp);

				return Ok(mdl);
			}
		}

		public IHttpActionResult Put(long id, TaskType mdl)
		{
            using (var db = new DevKitDB())
			{
				var resp = "";

				if (!mdl.Update(db, mdl.login.idUser, ref resp))
					return BadRequest(resp);

				return Ok(mdl);				
			}
		}

		public IHttpActionResult Delete(long id)
		{
            var login = GetLoginInfo();

            using (var db = new DevKitDB())
			{
				var model = db.GetTaskType(id);

				if (model == null)
					return StatusCode(HttpStatusCode.NotFound);

				var resp = "";

				if (!model.CanDelete(db, ref resp))
					return BadRequest(resp);
				
				model.Delete(db, login.idUser);
				
				return Ok();
			}
		}
	}
}
