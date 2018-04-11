﻿using DataModel;
using System.Web.Http;

namespace DevKit.Web.Controllers
{
    public class EmissorListagemAutorizacaoController : ApiControllerBase
    {
        public IHttpActionResult Get()
        {
            if (!StartDatabaseAndAuthorize())
                return BadRequest();

            return Ok(new Empresa().ListagemAutorizacao(db, new ListagemEmissorAutorizacaoFilter
            {
                skip = Request.GetQueryStringValue("skip", 0),
                take = Request.GetQueryStringValue("take", 0),
                fkSecao = Request.GetQueryStringValue("fkSecao", 0),
                tuss = Request.GetQueryStringValue("tuss"),
                espec = Request.GetQueryStringValue("espec"),
                mes = Request.GetQueryStringValue("mes"),
                ano = Request.GetQueryStringValue("ano"),
                nomeAssociado = Request.GetQueryStringValue("nomeAssociado"),
                nomeCredenciado = Request.GetQueryStringValue("nomeCredenciado"),
                codCredenciado = Request.GetQueryStringValue("codCredenciado"),
                matricula = Request.GetQueryStringValue("matricula"),
                dtInicial = Request.GetQueryStringValue("dtInicial"),
                dtFim = Request.GetQueryStringValue("dtFim"),
            }));
        }
    }
}