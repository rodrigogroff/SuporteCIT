﻿using Dapper;
using Entities.Database;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Master.Repository
{
    public interface IDapperRepository
    {
        T_Cartao ObterCartao(SqlConnection db, string empresa, string matricula, string titularidade);
        T_Cartao ObterCartao(SqlConnection db, string id);
        T_Proprietario ObterProprietario(SqlConnection db, long id);
        List<T_Cartao> ObterCartaoLista(SqlConnection db, string empresa, string matricula);
        List<T_Loja> ObterLojaLista(SqlConnection db, List<long> lstFkLoja);
        T_Loja ObterLoja(SqlConnection db, long? idLoja);
        T_Empresa ObterEmpresa(SqlConnection db, string empresa);
        T_Terminal ObterTerminal(SqlConnection db, string codigo);
        T_Terminal ObterTerminal(SqlConnection db, long id);
        List<T_Loja> ObterListaLojas(SqlConnection db, long idEmpresa, string pesquisa);
        List<T_Parcelas> ObterParcelaLista(SqlConnection db, List<long> lstFkParcela);
        List<T_Parcelas> ObterParcelaListaDeListaCartaoIgual(SqlConnection db, List<long> lstFkCartao, int nuParcela);
        List<T_Parcelas> ObterParcelaListaDeListaCartaoSuperior(SqlConnection db, List<long> lstFkCartao, int nuParcela);
        LOG_Transacoes ObterLogTransacao(SqlConnection db, long id);
        LOG_Transacoes ObterLogTransacaoNSU(SqlConnection db, long nsu);
        List<LOG_Transacoes> ObterLogTransacaoLista(SqlConnection db, List<long> lstFkLog);
        List<LOG_Fechamento> ObterFechamentoLista(SqlConnection db, string mes, string ano, long fkEmpresa, long fkCartao);
        void InserirSolicitacaoVenda(SqlConnection db, SolicitacaoVenda tbl);
        void AtualizarSolicitacaoVenda(SqlConnection db, SolicitacaoVenda tbl);
        SolicitacaoVenda ObterSolicVendaCartao(SqlConnection db, long fkCartao);
        SolicitacaoVenda ObterSolicLojistaEmAberto(SqlConnection db, long fkTerminal);
        SolicitacaoVenda ObterSolicVendaCartaoId(SqlConnection db, long id);
        List<SolicitacaoVenda> ObterSolicitacoesLista(SqlConnection db, long fkTerminal);
    }

    public class DapperRepository : IDapperRepository
    {
        public T_Empresa ObterEmpresa(SqlConnection db, string empresa)
        {
            return db.Query<T_Empresa>(@"select * from [T_Empresa] (nolock) 
                                        where   st_empresa = @empresa ", new { empresa }).FirstOrDefault();
        }
        public T_Cartao ObterCartao(SqlConnection db, string id)
        {
            return db.Query<T_Cartao>(@"select * from [T_Cartao] (nolock) 
                                        where   i_unique = @id", new { id }).FirstOrDefault();
        }

        public T_Proprietario ObterProprietario(SqlConnection db, long id)
        {
            return db.Query<T_Proprietario>(@"select * from [T_Proprietario] (nolock) 
                                        where   i_unique = @id ", new { id }).FirstOrDefault();
        }

        public List<T_Cartao> ObterCartaoLista(SqlConnection db, string empresa, string matricula)
        {
            return db.Query<T_Cartao>(@"select * from [T_Cartao] (nolock) 
                                        where   st_empresa = @empresa and
                                                st_matricula = @matricula ", new { empresa, matricula }).ToList();
        }

        public T_Cartao ObterCartao(SqlConnection db, string empresa, string matricula, string titularidade)
        {
            return db.Query<T_Cartao>(@"select * from [T_Cartao] (nolock) 
                                        where   st_empresa = @empresa and
                                                st_matricula = @matricula and 
                                                st_titularidade = @titularidade", new { empresa, matricula, titularidade }).FirstOrDefault();
        }
        
        public List<T_Parcelas> ObterParcelaListaDeListaCartaoIgual(SqlConnection db, List<long> lstFkCartao, int nuParcela)
        {
            return db.Query<T_Parcelas>(@"select * from [T_Parcelas] (nolock) 
                                        where   nu_parcela = @nuParcela and
                                                fk_cartao in @lstFkCartao", new { nuParcela, lstFkCartao }).ToList();
        }

        public List<T_Parcelas> ObterParcelaListaDeListaCartaoSuperior(SqlConnection db, List<long> lstFkCartao, int nuParcela)
        {
            return db.Query<T_Parcelas>(@"select * from [T_Parcelas] (nolock) 
                                        where   nu_parcela >= @nuParcela and
                                                fk_cartao in @lstFkCartao", new { nuParcela, lstFkCartao }).ToList();
        }

        public LOG_Transacoes ObterLogTransacao(SqlConnection db, long id)
        {
            return db.Query<LOG_Transacoes>(@"select * from [LOG_Transacoes] (nolock) 
                                        where   i_unique = @id ", new { id }).FirstOrDefault();
        }

        public LOG_Transacoes ObterLogTransacaoNSU(SqlConnection db, long nsu)
        {
            return db.Query<LOG_Transacoes>(@"select * from [LOG_Transacoes] (nolock) 
                                        where   nu_nsu = @nsu ", new { nsu }).FirstOrDefault();
        }

        public List<LOG_Transacoes> ObterLogTransacaoLista(SqlConnection db, List<long> lstFkLog)
        {
            return db.Query<LOG_Transacoes>(@"select * from [LOG_Transacoes] (nolock) 
                                        where i_unique in @lstFkLog", new { lstFkLog }).ToList();

        }

        public List<T_Loja> ObterLojaLista(SqlConnection db, List<long> lstFkLoja)
        {
            return db.Query<T_Loja>(@"select * from [T_Loja] (nolock) 
                                        where i_unique in @lstFkLoja", new { lstFkLoja }).ToList();
        }

        public List<LOG_Fechamento> ObterFechamentoLista(SqlConnection db, string mes, string ano, long fkEmpresa, long fkCartao)
        {
            return db.Query<LOG_Fechamento>(@"select * from [LOG_Fechamento] (nolock) 
                                        where st_mes = @mes and st_ano = @ano and fk_empresa = @fkEmpresa and fk_cartao = @fkCartao", 
                                        new { mes, ano, fkEmpresa, fkCartao }).ToList();
        }

        public List<T_Parcelas> ObterParcelaLista(SqlConnection db, List<long> lstFkParcela)
        {
            return db.Query<T_Parcelas>(@"select * from [T_Parcelas] (nolock) 
                                        where  i_unique in @lstFkParcela", new { lstFkParcela }).ToList();
        }

        public T_Terminal ObterTerminal(SqlConnection db, string codigo)
        {
            return db.Query<T_Terminal>(@"select * from [T_Terminal] (nolock) 
                                        where   nu_terminal = @codigo ", new { codigo }).FirstOrDefault();
        }

        public T_Loja ObterLoja(SqlConnection db, long? idLoja)
        {
            return db.Query<T_Loja>(@"select * from [T_Loja] (nolock) 
                                        where i_unique = @idLoja", new { idLoja }).FirstOrDefault();
        }

        public List<T_Loja> ObterListaLojas(SqlConnection db, long idEmpresa, string pesquisa)
        {
            var lst = db.Query<long>(@" select fk_loja from [LINK_LojaEmpresa] (nolock)
                                        where fk_empresa = @idEmpresa", new { idEmpresa }).ToList();

            if (string.IsNullOrEmpty(pesquisa))
                return db.Query<T_Loja>(@"  select * from [T_Loja] (nolock) 
                                        where i_unique in @lst order by st_nome", new { lst }).ToList();
            else
                return db.Query<T_Loja>(@"  select * from [T_Loja] (nolock) 
                                        where (st_nome like @pesquisa or st_endereco like @pesquisa) and i_unique in @lst order by st_nome", new { lst, pesquisa }).ToList();
        }

        public void InserirSolicitacaoVenda(SqlConnection db, SolicitacaoVenda tbl)
        {
            db.Execute(@"insert into [SolicitacaoVenda] (fkCartao,fkLoja,vrValor,nuParcelas,tgAberto,dtSolic,dtConf,fkTerminal,fkLogTrans)
                        values (@fkCartao,@fkLoja,@vrValor,@nuParcelas,@tgAberto,@dtSolic,@dtConf,@fkTerminal,@fkLogTrans)", 
                        new { 
                            tbl.fkCartao,
                            tbl.fkLoja,
                            tbl.vrValor,
                            tbl.nuParcelas,
                            tbl.tgAberto,
                            tbl.dtSolic,
                            tbl.dtConf,
                            tbl.fkTerminal,
                            tbl.fkLogTrans,
                        } );
        }

        public void AtualizarSolicitacaoVenda(SqlConnection db, SolicitacaoVenda tbl)
        {
            db.Execute(@"update [SolicitacaoVenda] 
                                set fkCartao = @fkCartao, 
                                    fkLoja = @fkLoja, 
                                    vrValor = @vrValor, 
                                    nuParcelas = @nuParcelas, 
                                    tgAberto = @tgAberto, 
                                    dtSolic = @dtSolic,
                                    dtConf = @dtConf,
                                    fkTerminal = @fkTerminal,
                                    fkLogTrans = @fkLogTrans
                                where id = @id",
                        new
                        {
                            tbl.id,
                            tbl.fkCartao,
                            tbl.fkLoja,
                            tbl.vrValor,
                            tbl.nuParcelas,
                            tbl.tgAberto,
                            tbl.dtSolic,
                            tbl.dtConf,
                            tbl.fkTerminal,
                            tbl.fkLogTrans,
                        });
        }

        public SolicitacaoVenda ObterSolicVendaCartao(SqlConnection db, long fkCartao)
        {
            return db.Query<SolicitacaoVenda>(@"select * from [SolicitacaoVenda] (nolock) 
                                                where fkCartao = @fkCartao and tgAberto = 1 
                                                order by dtSolic desc", new { fkCartao }).
                                                FirstOrDefault();
        }

        public SolicitacaoVenda ObterSolicVendaCartaoId(SqlConnection db, long id)
        {
            return db.Query<SolicitacaoVenda>(@"select * from [SolicitacaoVenda] (nolock) 
                                                where id = @id ", new { id }).
                                                FirstOrDefault();
        }

        public SolicitacaoVenda ObterSolicLojistaEmAberto(SqlConnection db, long fkTerminal)
        {
            return db.Query<SolicitacaoVenda>(@"select * from [SolicitacaoVenda] (nolock) 
                                                where fkTerminal = @fkTerminal and tgAberto = 1 
                                                order by dtSolic desc", new { fkTerminal }).
                                                FirstOrDefault();
        }

        public T_Terminal ObterTerminal(SqlConnection db, long id)
        {
            return db.Query<T_Terminal>(@"select * from [T_Terminal] (nolock) 
                                        where   i_unique = @id ", new { id }).FirstOrDefault();
        }

        public List<SolicitacaoVenda> ObterSolicitacoesLista(SqlConnection db, long fkTerminal)
        {
            return db.Query<SolicitacaoVenda>(@"    select * from [SolicitacaoVenda] (nolock) 
                                                    where fkTerminal = @fkTerminal order by dtSolic desc", new { fkTerminal }).ToList();
        }
    }
}