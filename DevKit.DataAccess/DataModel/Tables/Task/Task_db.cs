﻿using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataModel
{
	public class TaskFilter
	{
		public int skip, take;

		public long?	nuPriority,
						fkProject;

		public string busca;
	}

	// --------------------------
	// properties
	// --------------------------

	public partial class Task
	{
		public string sdtStart = "";
		public string snuPriority = "";
		public string sfkUserStart = "";
		public string sfkUserResponsible = "";
		public string sfkTaskType = "";
		public string sfkTaskCategory = "";
		public string sfkProject = "";
		public string sfkPhase = "";
		public string sfkSprint = "";
		public string sfkVersion = "";

		public List<TaskProgress> usrProgress;
		public List<TaskMessage> usrMessages;

		// up commands

		public string stUserMessage = "";

		public string updateCommand = "";
		public object anexedEntity;
	}

	// --------------------------
	// functions
	// --------------------------

	public partial class Task
	{
		public IQueryable<Task> ComposedFilters(DevKitDB db, TaskFilter filter)
		{
			var query = from e in db.Tasks select e;

			if (filter.busca != null)
				query = from e in query
						where	e.stDescription.ToUpper().Contains(filter.busca) ||
								e.stLocalization.ToUpper().Contains(filter.busca) ||
								e.stTitle.ToUpper().Contains(filter.busca) 
						select e;

			if (filter.nuPriority != null)
				query = from e in query
						where e.nuPriority == filter.nuPriority
						select e;

			if (filter.fkProject != null)
				query = from e in query
						where e.fkProject == filter.fkProject
						select e;

			return query;
		}

		public Task LoadAssociations(DevKitDB db)
		{
			var setup = db.Setup();

			sdtStart = dtStart?.ToString(setup.stDateFormat);
			snuPriority = new EnumPriority().lst.Where(t => t.id == nuPriority).FirstOrDefault().stName;
			sfkUserStart = db.User(fkUserStart).stLogin;
			sfkUserResponsible = db.User(fkUserResponsible).stLogin;
			sfkTaskCategory = db.TaskCategory(fkTaskCategory).stName;
			sfkTaskType = db.TaskType(fkTaskType).stName;
			sfkProject = db.Project(fkProject).stName;
			sfkPhase = db.ProjectPhase(fkPhase).stName;
			sfkSprint = db.ProjectSprint(fkSprint).stName;
			sfkVersion = db.ProjectSprintVersion(fkVersion).stName;

			usrProgress = LoadProgress(db);
			usrMessages = LoadMessages(db);

			return this;
		}

		public List<TaskProgress> LoadProgress(DevKitDB db)
		{
			var ret = (from e in db.TaskProgresses where e.fkTask == id select e).OrderByDescending(t => t.dtLog).ToList();

			var setup = db.Setup();

			for (int i = 0; i < ret.Count(); i++)
			{
				var item = ret.ElementAt(i);

				item.sdtLog = item.dtLog?.ToString(setup.stDateFormat);
				item.sfkUserAssigned = db.User(item.fkUserAssigned).stLogin;
			}

			return ret;
		}

		public List<TaskMessage> LoadMessages(DevKitDB db)
		{
			var ret = (from e in db.TaskMessages where e.fkTask == id select e).OrderByDescending(t => t.dtLog).ToList();

			var setup = db.Setup();

			for (int i = 0; i < ret.Count(); i++)
			{
				var item = ret.ElementAt(i);

				item.sdtLog = item.dtLog?.ToString(setup.stDateFormat);
				item.sfkUser = db.User(item.fkUser).stLogin;
			}

			return ret;
		}

		bool CheckDuplicate(Task item, DevKitDB db)
		{
			var query = from e in db.Tasks select e;

			if (item.stTitle != null)
			{
				var _st = item.stTitle.ToUpper();
				query = from e in query where e.stTitle.ToUpper().Contains(_st) select e;
			}

			if (item.fkProject != null)
				query = from e in query where e.fkProject == item.fkProject select e;

			if (item.fkSprint != null)
				query = from e in query where e.fkSprint == item.fkSprint select e;

			if (item.id > 0)
				query = from e in query where e.id != item.id select e;

			return query.Any();
		}

		public bool Create(DevKitDB db, User usr, ref string resp)
		{
			if (CheckDuplicate(this, db))
			{
				resp = "Task title cannot be duplicated";
				return false;
			}

			dtStart = DateTime.Now;
			fkUserStart = usr.id;
			
			id = Convert.ToInt64(db.InsertWithIdentity(this));

			return true;
		}

		public bool Update(DevKitDB db, User userLogged, ref string resp)
		{
			if (CheckDuplicate(this, db))
			{
				resp = "Task title cannot be duplicated";
				return false;
			}

			switch (updateCommand)
			{
				case "entity":
					{
						var oldTask = (from ne in db.Tasks where ne.id == id select ne).FirstOrDefault();

						if (oldTask.fkUserResponsible != fkUserResponsible)
						{
							db.Insert(new TaskProgress()
							{
								dtLog = DateTime.Now,
								fkTask = id,
								fkUserAssigned = fkUserResponsible
							});
						}

						if (stUserMessage != "")
						{
							db.Insert(new TaskMessage()
							{
								stMessage = stUserMessage,
								dtLog = DateTime.Now,
								fkTask = id,
								fkUser = userLogged.id
							});

							stUserMessage = "";
						}

						db.Update(this);
						LoadAssociations(db);

						break;
					}				
			}

			return true;
		}

		public bool CanDelete(DevKitDB db, ref string resp)
		{
			return true;
		}

		public void Delete(DevKitDB db)
		{
			db.Delete(this);
		}
	}
}
