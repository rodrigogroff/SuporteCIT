﻿using LinqToDB;
using System.Linq;
using System.Collections.Generic;

namespace DataModel
{
	public partial class Task
	{
		public List<TaskClientGroup> LoadClientGroups(DevKitDB db)
		{
			var ret = (from e in db.TaskClientGroup
					   where e.fkTask == id
					   select e).
					   OrderByDescending(t => t.id).
					   ToList();

			foreach (var item in ret)
				item.sfkClientGroup = db.GetClientGroup(item.fkClientGroup).stName;

			return ret;
		}
	}
}
