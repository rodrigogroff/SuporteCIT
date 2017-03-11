﻿using DataModel;
using LinqToDB;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace DevKit.Web.Controllers
{
	public class SetupController : ApiControllerBase
	{
		public IHttpActionResult Get(long id)
		{
			using (var db = new DevKitDB())
			{
				var model = (from ne in db.Setups select ne).Where(t => t.id == id).FirstOrDefault();
				
				if (model != null)
					return Ok(model);

				return StatusCode(HttpStatusCode.NotFound);
			}
		}

		public IHttpActionResult Put(long id, Setup mdl)
		{
			using (var db = new DevKitDB())
			{
				var resp = ""; if (!mdl.Update(db, ref resp))
					return BadRequest(resp);

				return Ok(mdl);
			}
		}
	}
}