﻿using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DataModel
{
    public class SaldoDisponivel
    {
        public long vrUtilizadoAtual = 0;

        public void Obter(AutorizadorCNDB db, T_Cartao cart, ref long dispMensal, ref long dispTotal)
        {
            dispMensal = (long)cart.vr_limiteMensal;
            dispTotal = (long)cart.vr_limiteTotal;

            if (cart.vr_extraCota > 0)
            {
                dispMensal += (long)cart.vr_extraCota;
                dispTotal += (long)cart.vr_extraCota;
            }

            var lstCartoes = (from e in db.T_Cartao
                              where e.st_empresa == cart.st_empresa
                              where e.st_matricula == cart.st_matricula
                              select (int)e.i_unique).
                              ToList();

            var lstParcelas = (from e in db.T_Parcelas
                               where lstCartoes.Contains((int)e.fk_cartao)
                               where e.nu_parcela == 1
                               select e).
                               ToList();

            foreach (var parc in lstParcelas)
            {
                var transacao = (from e in db.LOG_Transacoes
                                 where e.i_unique == parc.fk_log_transacoes
                                 select e).
                                 FirstOrDefault();

                if (transacao == null)
                    continue;

                if (transacao.tg_confirmada.ToString() == TipoConfirmacao.Confirmada ||
                    transacao.tg_confirmada.ToString() == TipoConfirmacao.Pendente)
                {
                    dispMensal -= (int)parc.vr_valor;
                    vrUtilizadoAtual += (long)parc.vr_valor;
                }
            }

            var lstParcelasTotais = (from e in db.T_Parcelas
                                     where lstCartoes.Contains((int)e.fk_cartao)
                                     where e.nu_parcela >= 1
                                     select e).
                                     ToList();

            foreach (var parc in lstParcelasTotais)
            {
                var transacao = (from e in db.LOG_Transacoes
                                 where e.i_unique == parc.fk_log_transacoes
                                 select e).
                                 FirstOrDefault();

                if (transacao == null)
                    continue;

                var sit = transacao.tg_confirmada.ToString();

                if (sit == TipoConfirmacao.Confirmada ||
                    sit == TipoConfirmacao.Pendente)
                {
                    dispTotal -= (int)parc.vr_valor;
                }
            }

            if (dispMensal < 0) dispMensal = 0;
            if (dispTotal < 0) dispTotal = 0;
        }
    }

    public class BaseVenda
    {
        public StreamWriter sw;

        public string nomeFile = "", 
                      dirFile = "";
        
        public string SetupFile()
        {
            if (dirFile == "")
                dirFile = "cnet_logs";

            string dir = "c:\\" + dirFile;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            DateTime dt = DateTime.Now;

            dir += "\\" + dt.Year;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            dir += "\\" + dt.Month;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            dir += "\\" + dt.Day;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            do
            {
                nomeFile = dir + "\\" + GetRandomString(20) + ".txt";
            }
            while (File.Exists(nomeFile));

            sw = new StreamWriter(nomeFile, false, Encoding.UTF8);

            Registry(" ---- File Created ---- ");

            return dir;
        }        

        public void Registry (string text)
        {
            sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:ffff") + " " + text);
            sw.Flush();
        }

        public void CloseFile()
        {
            sw.Close();
        }

        private string GetRandomString(int length)
        {
            var rand = new Random();
            var ret = "";

            for (int i = 0; i < length; i++)
                ret += rand.Next(0, 9);

            return ret;
        }

        public string DESCript(string dados, string chave)
        {
            dados = dados.PadLeft(8, '*');

            byte[] key = System.Text.Encoding.ASCII.GetBytes(chave);//{1,2,3,4,5,6,7,8};
            byte[] data = System.Text.Encoding.ASCII.GetBytes(dados);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            des.Key = key;
            des.Mode = CipherMode.ECB;

            ICryptoTransform DESt = des.CreateEncryptor();
            DESt.TransformBlock(data, 0, 8, data, 0);

            string retorno = "";
            for (int n = 0; n < 8; n++)
            {
                retorno += String.Format("{0:X2}", data[n]);
            }

            return retorno;
        }

        public static string DESdeCript(string dados, string chave)
        {
            byte[] key = System.Text.Encoding.ASCII.GetBytes(chave);//{1,2,3,4,5,6,7,8};
            byte[] data = new byte[8];

            for (int n = 0; n < dados.Length / 2; n++)
            {
                data[n] = (byte)Convert.ToInt32(dados.Substring(n * 2, 2), 16);
            }

            DES des = new DESCryptoServiceProvider();
            des.Key = key;
            des.Mode = CipherMode.ECB;
            ICryptoTransform crypto = des.CreateDecryptor();
            MemoryStream cipherStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(cipherStream, crypto, CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            crypto.TransformBlock(data, 0, 8, data, 0);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            string retorno = enc.GetString(data);

            return retorno;
        }

        public DateTime ObtemData(string valor)
        {
            try
            {
                if (valor == null)
                    return new DateTime();

                if (valor.Length < 8)
                    return new DateTime();

                if (valor.Length == 8)
                    valor = valor.Substring(0, 2) + "/" +
                            valor.Substring(2, 2) + "/" +
                            valor.Substring(4, 4);

                return new DateTime(Convert.ToInt32(valor.Substring(6, 4)),
                                    Convert.ToInt32(valor.Substring(3, 2)),
                                    Convert.ToInt32(valor.Substring(0, 2)), 0, 0, 0);
            }
            catch (SystemException ex)
            {
                ex.ToString();
                return new DateTime();
            }
        }

        public long ObtemValor(string valor)
        {
            try
            {
                if (valor == null)
                    return 0;

                if (valor == "")
                    valor = "0";

                var iValor = 0;

                if (!valor.Contains(","))
                    valor += ",00";

                valor = valor.Replace(",", "").Replace(".", "");
                iValor = Convert.ToInt32(valor);

                return iValor;
            }
            catch (SystemException ex)
            {
                ex.ToString();
                return 0;
            }
        }

    }
}