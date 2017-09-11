﻿using System.Collections.Generic;
using System.Web.Http;
using System;
using System.Linq;
using LinqToDB;

namespace DevKit.Web.Controllers
{
    public class EfetuaVendaController : ApiControllerBase
    {
        public string cnet_server = "54.233.109.221";
        public int cnet_port = 2000;

        [NonAction]
        public long ObtemValor(string valor)
        {
            if (valor == null)
                return 0;

            var iValor = 0;

            if (!valor.Contains(","))
                valor += ",00";

            valor = valor.Replace(",", "").Replace(".", "");
            iValor = Convert.ToInt32(valor);

            return iValor;
        }

        public string terminal, empresa, matricula, codAcesso, stVencimento, strMessage, retorno, nsu_retorno;
        public long idCartao, valor, parcelas, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12;

        public IHttpActionResult Get()
        {
            empresa = Request.GetQueryStringValue("empresa").PadLeft(6,'0');
            matricula = Request.GetQueryStringValue("matricula").PadLeft(6, '0');
            codAcesso = Request.GetQueryStringValue("codAcesso");
            stVencimento = Request.GetQueryStringValue("stVencimento");
            idCartao = Request.GetQueryStringValue<int>("cartao");
            valor = ObtemValor(Request.GetQueryStringValue("valor"));            
            parcelas = Request.GetQueryStringValue<int>("parcelas");
            
            p1 = ObtemValor(Request.GetQueryStringValue("p1"));
            p2 = ObtemValor(Request.GetQueryStringValue("p2"));
            p3 = ObtemValor(Request.GetQueryStringValue("p3"));
            p4 = ObtemValor(Request.GetQueryStringValue("p4"));
            p5 = ObtemValor(Request.GetQueryStringValue("p5"));
            p6 = ObtemValor(Request.GetQueryStringValue("p6"));
            p7 = ObtemValor(Request.GetQueryStringValue("p7"));
            p8 = ObtemValor(Request.GetQueryStringValue("p8"));
            p9 = ObtemValor(Request.GetQueryStringValue("p9"));
            p10 = ObtemValor(Request.GetQueryStringValue("p10"));
            p11 = ObtemValor(Request.GetQueryStringValue("p11"));
            p12 = ObtemValor(Request.GetQueryStringValue("p12"));

            if (!StartDatabaseAndAuthorize())
                return BadRequest("Não autorizado!");

            // =============================
            // obtem terminal
            // =============================

            terminal = (from e in db.T_Terminal
                        where e.fk_loja == db.currentUser.i_unique
                        select e).
                        FirstOrDefault().
                        nu_terminal;

            // =============================
            // verifica cartão
            // =============================

            #region - code - 

            var associadoPrincipal = (from e in db.T_Cartao
                             where e.st_empresa == empresa
                             where e.st_matricula == matricula
                             where e.st_titularidade == "01"
                             select e).
                             FirstOrDefault();

            var dadosProprietario = ( from e in db.T_Proprietario
                                      where e.i_unique == associadoPrincipal.fk_dadosProprietario
                                      select e).
                                      FirstOrDefault();

            int titularidadeFinal = 1, via = 1;

            var calcAcesso = new CodigoAcesso();

            var codAcessoCalc = calcAcesso.Obter ( empresa,
                                                   matricula, 
                                                   titularidadeFinal, 
                                                   via, 
                                                   dadosProprietario.st_cpf );
            
            while (codAcessoCalc != codAcesso)
            {
                via = 0;

                for (int t=0; t < 9; ++t)
                {
                    codAcessoCalc = calcAcesso.Obter ( empresa, 
                                                       matricula, 
                                                       titularidadeFinal, 
                                                       ++via, 
                                                       dadosProprietario.st_cpf );

                    if (codAcessoCalc == codAcesso)
                        break;
                }

                if (codAcessoCalc == codAcesso)
                    break;

                titularidadeFinal++;
            }

            if (titularidadeFinal != 1 )
            {
                var associado = (from e in db.T_Cartao
                                 where e.st_empresa == empresa
                                 select e).
                                 FirstOrDefault();

                if (associado.st_venctoCartao != stVencimento)
                    return BadRequest("Cartão inválido (0xA1)");
            }
            else
            {
                if (associadoPrincipal.st_venctoCartao != stVencimento)
                    return BadRequest("Cartão inválido (0xA)");
            }

            #endregion

            var sc = new SocketConvey();

            var sck = sc.connectSocket(cnet_server, cnet_port);

            if (sck == null)
                return BadRequest("Falha de comunicação com servidor (0x1)");

            // #############################################################
            // #############################################################

            // Venda

            // #############################################################
            // #############################################################

            strMessage = MontaVendaDigitada(titularidadeFinal);
            
            if (!sc.socketEnvia(sck, strMessage))
            {
                sck.Close();
                return BadRequest("Falha de comunicação com servidor (0x2)");
            }
                
            retorno = sc.socketRecebe(sck);

            if (retorno.Length < 6 )
            {
                sck.Close();
                return BadRequest("Falha de comunicação com servidor (0x3)");
            }
                
            var codResp = retorno.Substring(2, 4);

            if (codResp != "0000")
            {
                sck.Close();
                return BadRequest("Falha VC (0xE" + codResp + " - " + retorno.Substring(73, 20) + " )");
            }                

            nsu_retorno = ObtemNsuRetorno(retorno);

            // #############################################################
            // #############################################################

            // Confirmação

            // #############################################################
            // #############################################################

            strMessage = MontaConfirmacao(titularidadeFinal);
            

            if (!sc.socketEnvia(sck, strMessage))
            {
                sck.Close();
                return BadRequest("Falha de comunicação com servidor (0x4)");
            }
                
            retorno = sc.socketRecebe(sck);

            sck.Close();

            if (retorno.Length < 6)
                return BadRequest("Falha de comunicação com servidor (0x5)");
                
            codResp = retorno.Substring(2, 4);

            if (codResp != "0000")
                return BadRequest("Falha VC (0xE" + codResp + " - " + retorno.Substring(73, 20) + " )");

            CleanCache(db, CacheTags.associado, idCartao );

            sck.Close();

            return Ok(new
            {
                count = 0,
                results = new List<string> { "OK" }
            });
        }

        public string MontaVendaDigitada(int titularidade)
        {
            var reg = "09DICE";

            reg += terminal.PadRight(8, ' ');

            reg += "000000";

            reg += empresa.PadLeft(6, '0');
            reg += matricula.PadLeft(6, '0');
            reg += titularidade.ToString().PadLeft(2, '0');
            reg += "       ";
            reg += "            ";
            reg += codAcesso;
            reg += valor.ToString().PadLeft(12, '0');
            reg += parcelas.ToString().PadLeft(2, '0');
            
            string valores = "";

            if (parcelas >= 1) valores += p1.ToString().PadLeft(12, '0');
            if (parcelas >= 2) valores += p2.ToString().PadLeft(12, '0');
            if (parcelas >= 3) valores += p3.ToString().PadLeft(12, '0');
            if (parcelas >= 4) valores += p4.ToString().PadLeft(12, '0');
            if (parcelas >= 5) valores += p5.ToString().PadLeft(12, '0');
            if (parcelas >= 6) valores += p6.ToString().PadLeft(12, '0');
            if (parcelas >= 7) valores += p7.ToString().PadLeft(12, '0');
            if (parcelas >= 8) valores += p8.ToString().PadLeft(12, '0');
            if (parcelas >= 9) valores += p9.ToString().PadLeft(12, '0');
            if (parcelas >= 10) valores += p10.ToString().PadLeft(12, '0');
            if (parcelas >= 11) valores += p11.ToString().PadLeft(12, '0');
            if (parcelas >= 12) valores += p12.ToString().PadLeft(12, '0');

            reg += valores;

            return reg;
        }

        public string ObtemNsuRetorno(string message)
        {
            return message.Substring(7,6).TrimStart('0');
        }

        public string MontaConfirmacao(int titularidade)
        {
            var reg = "09CECC";

            reg += terminal.PadRight(8, ' ');

            reg += "000000";

            reg += empresa.PadLeft(6, '0');
            reg += matricula.PadLeft(6, '0');
            reg += titularidade.ToString().PadLeft(2, '0');
            reg += "       ";
            reg += nsu_retorno;
            
            return reg.PadRight(100, ' ');
        }
    }
}