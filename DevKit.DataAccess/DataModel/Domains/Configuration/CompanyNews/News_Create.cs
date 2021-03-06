﻿using LinqToDB;
using System;
using System.Linq;

namespace DataModel
{
	public partial class CompanyNews
	{
		bool CheckDuplicate(CompanyNews item, DevKitDB db)
		{
			var query = from e in db.CompanyNews select e;

			if (item.stTitle != null)
				query = from e in query
                        where e.stTitle.ToUpper() == item.stTitle.ToUpper()
                        select e;

			if (item.id > 0)
				query = from e in query
                        where e.id != item.id
                        select e;

			return query.Any();
		}
		
		public bool Create(DevKitDB db, ref string resp)
		{
			var user = db.currentUser;

			fkUser = user.id;
			dtLog = DateTime.Now;

			id = Convert.ToInt64(db.InsertWithIdentity(this));

			return true;
		}
	}
}
