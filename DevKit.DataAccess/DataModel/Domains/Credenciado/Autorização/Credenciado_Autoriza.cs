﻿using DevKit.DataAccess;
using LinqToDB;
using System;
using System.Linq;

namespace DataModel
{
    public class AutorizaProcedimentoParams
    {
        public string ca, senha, titVia;
        public long emp, mat, tuss;
    }

    public partial class Credenciado
    {
		public string AutorizaProcedimento ( DevKitDB db, AutorizaProcedimentoParams _params )
		{
            var util = new Util();

            var nuTit = Convert.ToInt32(_params.titVia.Substring(0, 2));
            var nuVia = Convert.ToInt32(_params.titVia.Substring(2, 2));

            var secaoTb = (from e in db.EmpresaSecao
                           where e.nuEmpresa == _params.emp
                           select e).
                           FirstOrDefault();

            if (secaoTb == null)
                return "Empresa inválida";

            var empTb = (from e in db.Empresa where e.id == secaoTb.fkEmpresa select e).FirstOrDefault();

            var associado = (from e in db.Associado
                             where e.fkEmpresa == secaoTb.fkEmpresa
                             where e.nuMatricula == _params.mat
                             where e.nuTitularidade == nuTit
                             where e.nuViaCartao == nuVia
                             select e).
                             FirstOrDefault();

            if (associado == null)
                return "Matrícula inválida";

            var caCalc = util.calculaCodigoAcesso ( secaoTb.nuEmpresa.ToString().PadLeft(6, '0'),
                                                    _params.mat.ToString().PadLeft(6, '0'),
                                                    associado.nuTitularidade.ToString(),
                                                    associado.nuViaCartao.ToString(),
                                                    associado.stCPF );

            if (_params.ca != caCalc)
                return "Dados do cartão inválidos!";

            var associadoTit = (from e in db.Associado
                                where e.fkEmpresa == secaoTb.fkEmpresa
                                where e.nuMatricula == _params.mat
                                where e.nuTitularidade == 1
                                select e).
                             FirstOrDefault();

            if (_params.senha != associadoTit.stSenha)
                return "Senha inválida!";

            if (associado.tgStatus == TipoSituacaoCartao.Bloqueado)
                return "Cartão bloqueado!";

            if (!db.CredenciadoEmpresa.Any(y => y.fkCredenciado == db.currentCredenciado.id &&
                                           y.fkEmpresa == secaoTb.fkEmpresa))
                return "Credenciado não conveniado à empresa " + _params.emp;

            var proc = db.TUSS.
                            Where(y => y.nuCodTUSS == _params.tuss).
                            FirstOrDefault();

            if (proc == null)
                return "Procedimento " + _params.tuss + " inválido!";

            DateTime dt = DateTime.Now;

            if (dt.Day < empTb.nuDiaFech)
                dt = dt.AddMonths(-1);

            db.Insert(new Autorizacao
            {
                dtSolicitacao = DateTime.Now,
                fkAssociado = associado.id,
                fkCredenciado = db.currentCredenciado.id,
                fkEmpresa = associado.fkEmpresa,
                fkProcedimento = proc.id,
                nuAno = dt.Year,
                nuMes = dt.Month,
                tgSituacao = TipoSitAutorizacao.Autorizado,
            });

            return "";
		}
	}
}
