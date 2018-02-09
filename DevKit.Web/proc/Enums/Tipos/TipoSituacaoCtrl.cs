﻿using DataModel;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace DevKit.Web.Controllers
{
	public class TipoSituacaoController : ApiControllerBase
	{
		public IHttpActionResult Get()
		{
            string busca = Request.GetQueryStringValue("busca", "").ToUpper();

            var query = (from e in new EnumTipoSituacao().itens select e);

			if (busca != "")
				query = from e in query where e.stName.ToUpper().Contains(busca) select e;

            var ret = new ComboReport
            {
                count = query.Count(),
                results = query.ToList()
            };

            return Ok(ret);
		}

		public IHttpActionResult Get(long id)
		{
            var mdl = new EnumTipoExpedicao().Get(id);

			if (mdl == null)
                return StatusCode(HttpStatusCode.NotFound);

            return Ok(mdl);
		}
	}
}
