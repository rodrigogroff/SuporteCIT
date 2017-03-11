﻿using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using DataModel;

namespace DataModel
{
	public class TaskTypeFilter
	{
		public int skip, take;
		public string busca;
	}

	// --------------------------
	// properties
	// --------------------------

	public partial class TaskType
	{
		public List<TaskCategory> categories;

		public string updateCommand = "";
		public object anexedEntity;
	}

	// --------------------------
	// functions
	// --------------------------

	public partial class TaskType
	{
		public IQueryable<TaskType> ComposedFilters(DevKitDB db, TaskTypeFilter filter)
		{
			var query = from e in db.TaskTypes select e;

			if (filter.busca != null)
				query = from e in query where e.stName.ToUpper().Contains(filter.busca) select e;

			return query;
		}

		public TaskType LoadAssociations(DevKitDB db)
		{
			categories = LoadCategories(db);

			return this;
		}

		List<TaskCategory> LoadCategories(DevKitDB db)
		{
			var lst = (from e in db.TaskCategories where e.fkTaskType == id select e).
				OrderBy(t => t.stName).
				ToList();

			return lst;
		}

		bool CheckDuplicate(TaskType item, DevKitDB db)
		{
			var query = from e in db.TaskTypes select e;

			if (item.stName != null)
			{
				var _st = item.stName.ToUpper();
				query = from e in query where e.stName.ToUpper().Contains(_st) select e;
			}

			if (item.id > 0)
				query = from e in query where e.id != item.id select e;

			return query.Any();
		}

		public bool Create(DevKitDB db, User usr, ref string resp)
		{
			if (CheckDuplicate(this, db))
			{
				resp = "Project name already taken";
				return false;
			}
			
			id = Convert.ToInt64(db.InsertWithIdentity(this));

			return true;
		}

		public bool Update(DevKitDB db, ref string resp)
		{
			if (CheckDuplicate(this, db))
			{
				resp = "Task type name already taken";
				return false;
			}

			switch (updateCommand)
			{
				case "entity":
					{
						db.Update(this);
						break;
					}

				case "newCategorie":
					{
						var ent = JsonConvert.DeserializeObject<TaskCategory>(anexedEntity.ToString());

						if ((from ne in db.TaskCategories
							 where ne.stName.ToUpper() == ent.stName.ToUpper() && ne.fkTaskType == id
							 select ne).Any())
						{
							resp = "Category already added to task type!";
							return false;
						}

						ent.fkTaskType = id;

						db.Insert(ent);
						categories = LoadCategories(db);
						break;
					}

				case "removeCategorie":
					{
						db.Delete(JsonConvert.DeserializeObject<TaskCategory>(anexedEntity.ToString()));
						categories = LoadCategories(db);
						break;
					}
			}

			return true;
		}

		public bool CanDelete(DevKitDB db, ref string resp)
		{
			return true;
		}
	}
}