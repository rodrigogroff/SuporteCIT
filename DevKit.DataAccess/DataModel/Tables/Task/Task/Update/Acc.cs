﻿using LinqToDB;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace DataModel
{
	public partial class Task
	{
		public bool Update_newAcc(DevKitDB db, User user, ref string resp)
		{
			var ent = JsonConvert.DeserializeObject<TaskAccumulatorValue>(anexedEntity.ToString());

			ent.fkTask = id;
			ent.fkUser = user.id;
			ent.dtLog = DateTime.Now;

			db.Insert(ent);

			new AuditLog
			{
				fkUser = user.id,
				fkActionLog = EnumAuditAction.TaskUpdateAccSaved,
				nuType = EnumAuditType.Task,
				fkTarget = this.id
			}.
			Create(db, "New time added: " + ent.nuHourValue.ToString() + ":" + ent.nuMinValue.ToString() + " >> " + db.User(ent.fkUser).stLogin, "");

			accs = LoadAccs(db);
			logs = LoadLogs(db);

			return true;
		}

		public bool Update_removeAccValue(DevKitDB db, User user, ref string resp)
		{
			var ent = JsonConvert.DeserializeObject<LogAccumulatorValue>(anexedEntity.ToString());

			var entDb = (from e in db.TaskAccumulatorValues
							where e.id == ent.id
							select e).
							FirstOrDefault();

			new AuditLog
			{
				fkUser = user.id,
				fkActionLog = EnumAuditAction.TaskUpdateAccRemoved,
				nuType = EnumAuditType.Task,
				fkTarget = this.id
			}.
			Create(db, "Time removed: " + entDb.nuHourValue.ToString() + ":" + entDb.nuMinValue.ToString() + " >> " + db.User(entDb.fkUser).stLogin, "");

			db.Delete(entDb);

			accs = LoadAccs(db);
			logs = LoadLogs(db);

			return true;
		}
	}
}