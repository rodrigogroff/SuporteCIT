﻿using DataModel;
using System.Web.Http;

namespace DevKit.Web.Controllers
{
	public class PrecoOPMEController : ApiControllerBase
	{
        public IHttpActionResult Get()
		{
            if (!StartDatabaseAndAuthorize())
                return BadRequest();

            var filter = new PrecoOPMEFilter
            {                
                skip = Request.GetQueryStringValue("skip", 0),
                take = Request.GetQueryStringValue("take", 15),
                codigo = Request.GetQueryStringValue("codigo"),
            };

            return Ok(new SaudeValorOPME().Listagem(db, filter));
        }
	}
}
