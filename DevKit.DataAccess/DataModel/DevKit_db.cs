﻿using System.Linq;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

namespace DataModel
{
	public partial class DevKitDB
	{				
		// ----------------------------------
		// Current system user 
		// ----------------------------------

		public User currentUser = null;

		public User GetCurrentUser()
		{
			if (currentUser == null)
				currentUser = (from ne in User
							   where ne.stLogin.ToUpper() == Thread.CurrentPrincipal.Identity.Name.ToUpper()
							   select ne).FirstOrDefault();

			return currentUser;
		}

		public List<long?> GetCurrentUserProjects()
		{
			if (currentUser == null)
				currentUser = GetCurrentUser();

			return (from e in ProjectUser
					where e.fkUser == currentUser.id
					select e.fkProject).
					ToList();
		}

		public List<long?> GetCurrentUserProjects(long userId)
		{
			return (from e in ProjectUser
					where e.fkUser == userId
					select e.fkProject).
					ToList();
		}

		public Hashtable Cache = new Hashtable();

        Setup _setup = null;

		public Setup GetSetup()
		{
			if (_setup == null)
				_setup = Setup.Find(1); 
			return _setup;
		}

		public User GetUser(long? id)
		{
			if (id == null) return null;
			var tag = "User" + id; var ret = Cache[tag] as User;
			if (ret == null) { ret = User.Find((long)id); Cache[tag] = ret; }
			return ret;
		}

		public Profile GetProfile(long? id)
		{
			if (id == null) return null;
			var tag = "Profile" + id; var ret = Cache[tag] as Profile;
			if (ret == null) { ret = Profile.Find((long)id); Cache[tag] = ret; }
			return ret;
		}

		public Project GetProject(long? id)
		{
			if (id == null) return null;
			var tag = "Project" + id; var ret = Cache[tag] as Project;
			if (ret == null) { ret = Project.Find((long)id); Cache[tag] = ret; }
			return ret;
		}

		public Client GetClient(long? id)
		{
			if (id == null) return null;
			var tag = "Client" + id; var ret = Cache[tag] as Client;
			if (ret == null) { ret = Client.Find((long)id); Cache[tag] = ret; }
			return ret;
		}

		public ClientGroup GetClientGroup(long? id)
		{
			if (id == null) return null;
			var tag = "ClientGroup" + id; var ret = Cache[tag] as ClientGroup;
			if (ret == null) { ret = ClientGroup.Find((long)id); Cache[tag] = ret; }
			return ret;
		}

		public CompanyNews GetNews(long? id)
		{
			if (id == null) return null;
			var tag = "CompanyNews" + id; var ret = Cache[tag] as CompanyNews;
			if (ret == null) { ret = CompanyNews.Find((long)id); Cache[tag] = ret; }
			return ret;
		}

		public Survey GetSurvey(long? id)
		{
			if (id == null) return null;
			var tag = "Survey" + id; var ret = Cache[tag] as Survey;
			if (ret == null) { ret = Survey.Find((long)id); Cache[tag] = ret; }
			return ret;
		}

		public ProjectPhase GetProjectPhase(long? id)
		{
			if (id == null) return null;
			var tag = "ProjectPhase" + id; var ret = Cache[tag] as ProjectPhase;
			if (ret == null) { ret = ProjectPhase.Find((long)id); Cache[tag] = ret; }
			return ret;
		}

		public ProjectSprint GetProjectSprint(long? id)
		{
			if (id == null) return null;
			var tag = "ProjectSprint" + id; var ret = Cache[tag] as ProjectSprint;
			if (ret == null) { ret = ProjectSprint.Find((long)id); Cache[tag] = ret; }
			return ret;
		}

		public ProjectSprintVersion GetProjectSprintVersion(long? id)
		{
			if (id == null) return null;
			var tag = "ProjectSprintVersion" + id; var ret = Cache[tag] as ProjectSprintVersion;
			if (ret == null) { ret = ProjectSprintVersion.Find((long)id); Cache[tag] = ret; }
			return ret;
		}

		public TaskType GetTaskType(long? id)
		{
			if (id == null) return null;
			var tag = "TaskType" + id; var ret = Cache[tag] as TaskType;
			if (ret == null) { ret = TaskType.Find((long)id); Cache[tag] = ret; }
			return ret;
		}

		public TaskTypeAccumulator GetTaskTypeAccumulator(long? id)
		{
			if (id == null) return null;
			var tag = "TaskTypeAccumulator" + id; var ret = Cache[tag] as TaskTypeAccumulator;
			if (ret == null) { ret = TaskTypeAccumulator.Find((long)id); Cache[tag] = ret; }
			return ret;
		}

		public TaskCategory GetTaskCategory(long? id)
		{
			if (id == null) return null;
			var tag = "TaskCategory" + id; var ret = Cache[tag] as TaskCategory;
			if (ret == null) { ret = TaskCategory.Find((long)id); Cache[tag] = ret; }
			return ret;
		}

		public TaskCheckPoint GetTaskCheckPoint(long? id)
		{
			if (id == null) return null;
			var tag = "TaskCheckPoint" + id; var ret = Cache[tag] as TaskCheckPoint;
			if (ret == null) { ret = TaskCheckPoint.Find((long)id); Cache[tag] = ret; }
			return ret;
		}

		public TaskFlow GetTaskFlow(long? id)
		{
			if (id == null) return null;
			var tag = "TaskFlow" + id; var ret = Cache[tag] as TaskFlow;
			if (ret == null) { ret = TaskFlow.Find((long)id); Cache[tag] = ret; }
			return ret;
		}

		public Task GetTask(long? id)
		{
			if (id == null) return null;
			var tag = "Task" + id; var ret = Cache[tag] as Task;
			if (ret == null) { ret = Task.Find((long)id); Cache[tag] = ret; }
			return ret;
		}
	}
}
