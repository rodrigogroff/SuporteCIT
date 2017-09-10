﻿using DataModel;
using LinqToDB;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Linq;
using System.Security.Claims;

namespace DevKit.Web
{
	public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
	{
		public override async System.Threading.Tasks.Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			context.Validated();
			await System.Threading.Tasks.Task.FromResult(0);
		}

        public override System.Threading.Tasks.Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            string nameUser = context.Identity.Claims.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();

            string m1 = context.Identity.Claims.Where(x => x.Type == "m1").Select(x => x.Value).FirstOrDefault();
            string m2 = context.Identity.Claims.Where(x => x.Type == "m2").Select(x => x.Value).FirstOrDefault();
            //string Session = context.Identity.Claims.Where(x => x.Type == "Session").Select(x => x.Value).FirstOrDefault();

            context.AdditionalResponseParameters.Add("nameUser", nameUser);
            context.AdditionalResponseParameters.Add("m1", m1);
            context.AdditionalResponseParameters.Add("m2", m2);  
            
            return System.Threading.Tasks.Task.FromResult<object>(null);
        }

        public override async System.Threading.Tasks.Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			using (var db = new AutorizadorCNDB())
			{
                if (context.UserName == "DBA")
                {
                    if (context.Password == "X3POR2D2")
                    {
                        var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                        identity.AddClaim(new Claim(ClaimTypes.Name, "DBA"));

                        var ticket = new AuthenticationTicket(identity, null);

                        context.Validated(ticket);
                    }
                    else
                    {
                        context.SetError("invalid_grant", "Senha de DBA inválida");
                        return;
                    }
                }
                else
                {
                    try
                    {
                        var lojista = (from e in db.T_Loja
                                       where e.st_loja == context.UserName
                                       where e.st_senha.ToUpper() == context.Password.ToUpper()
                                       select e).
                               FirstOrDefault();

                        if (lojista != null)
                        {
                            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                            identity.AddClaim(new Claim(ClaimTypes.Name, lojista.st_loja));
                            identity.AddClaim(new Claim("m1", lojista.st_nome));
                            identity.AddClaim(new Claim("m2", (lojista.st_endereco + " / " +
                                                               lojista.st_cidade + " " +
                                                               lojista.st_estado).Replace("{SE$3}", "")));

                            var ticket = new AuthenticationTicket(identity, null);

                            if (lojista.tg_blocked == '1')
                            {
                                //   context.SetError("invalid_grant", "Terminal BLOQUEADO pela administradora");
                                // return;
                            }

                            context.Validated(ticket);
                        }
                        else
                        {
                            context.SetError("invalid_grant", "Senha ou terminal inválido");
                            return;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        context.SetError("invalid_grant", ex.ToString());                        
                    }
                }

				await System.Threading.Tasks.Task.FromResult(0);
			}
		}
	}
}
