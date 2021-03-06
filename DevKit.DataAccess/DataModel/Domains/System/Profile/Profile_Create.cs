﻿using LinqToDB;
using System.Linq;
using System;

namespace DataModel
{
	public partial class Profile
	{
		bool CheckDuplicate(Profile item, DevKitDB db)
		{
			var query = from e in db.Profile select e;

			if (item.stName != null)
			{
				var _stName = item.stName.ToUpper();
				query = from e in query where e.stName.ToUpper().Contains(_stName) select e;
			}

			if (item.id > 0)
				query = from e in query where e.id != item.id select e;

			return query.Any();
		}

		public bool Create(DevKitDB db, ref string resp)
		{
			var user = db.currentUser;

			if (CheckDuplicate(this, db))
			{
				resp = "The name '" + stName + "' is already taken";
				return false;
			}

			id = Convert.ToInt64(db.InsertWithIdentity(this));

			new AuditLog {
				fkUser = user.id,
				fkActionLog = EnumAuditAction.SystemProfileAdd,
				nuType = EnumAuditType.Profile,
				fkTarget = this.id
			}.
			Create(db, "Novo perfil: " + this.stName, "");

			return true;
		}
	}
}
