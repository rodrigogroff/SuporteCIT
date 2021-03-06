﻿using LinqToDB;
using System.Linq;
using System.Text;

namespace DataModel
{
	public class UserFilter : BaseFilter
    {
		public string email, 
                      phone;

		public bool? ativo;
		public long? fkPerfil;

        public string Parameters()
        {
            return Export();
        }

        string Export()
        {
            var ret = new StringBuilder();

            // base
            ret.Append(skip + ",");
            ret.Append(take + ",");
            ret.Append(busca + ",");
            ret.Append(email + ",");
            ret.Append(phone + ",");

            if (ativo != null)
                ret.Append(ativo);
            ret.Append(",");

            if (fkPerfil != null)
                ret.Append(fkPerfil);
            ret.Append(",");

            return ret.ToString();
        }
    }

	public partial class User
	{
		public UserReport ComposedFilters(DevKitDB db, UserFilter filter)
		{
			var query = from e in db.User select e;

			if (filter.ativo != null)
				query = from e in query where e.bActive == filter.ativo select e;

            if (!string.IsNullOrEmpty(filter.busca))
                query = from e in query where e.stLogin.ToUpper().Contains(filter.busca) select e;

			if (filter.fkPerfil != null)
				query = from e in query where e.fkProfile == filter.fkPerfil select e;

			if (filter.email != null)
			{
				query = from e in query
						join eMail in db.UserEmail on e.id equals eMail.fkUser
						where e.id == eMail.fkUser
						where eMail.stEmail.ToUpper().Contains (filter.email)
						select e;
			}

			if (filter.phone != null)
			{
				query = from e in query
						join ePhone in db.UserPhone on e.id equals ePhone.fkUser
						where e.id == ePhone.fkUser
						where ePhone.stPhone.ToUpper().Contains(filter.phone)
						select e;
			}

			var count = query.Count();

			query = query.OrderBy(y => y.stLogin);

            return new UserReport
            {
                count = count,
                results = Loader(db, (query.Skip(filter.skip).Take(filter.take)).ToList())
            };
        }
	}
}
