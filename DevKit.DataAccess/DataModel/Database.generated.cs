//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/t4models).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------
using System;
using System.Linq;

using LinqToDB;
using LinqToDB.Mapping;

namespace DataModel
{
	/// <summary>
	/// Database       : DevKit
	/// Data Source    : localhost
	/// Server Version : 9.6.1
	/// </summary>
	public partial class DevKitDB : LinqToDB.Data.DataConnection
	{
		public ITable<Profile>              Profiles              { get { return this.GetTable<Profile>(); } }
		public ITable<Project>              Projects              { get { return this.GetTable<Project>(); } }
		public ITable<ProjectPhase>         ProjectPhases         { get { return this.GetTable<ProjectPhase>(); } }
		public ITable<ProjectSprint>        ProjectSprints        { get { return this.GetTable<ProjectSprint>(); } }
		public ITable<ProjectSprintVersion> ProjectSprintVersions { get { return this.GetTable<ProjectSprintVersion>(); } }
		public ITable<ProjectUser>          ProjectUsers          { get { return this.GetTable<ProjectUser>(); } }
		public ITable<Setup>                Setups                { get { return this.GetTable<Setup>(); } }
		public ITable<Task>                 Tasks                 { get { return this.GetTable<Task>(); } }
		public ITable<TaskAccumulatorValue> TaskAccumulatorValues { get { return this.GetTable<TaskAccumulatorValue>(); } }
		public ITable<TaskCategory>         TaskCategories        { get { return this.GetTable<TaskCategory>(); } }
		public ITable<TaskFlow>             TaskFlows             { get { return this.GetTable<TaskFlow>(); } }
		public ITable<TaskFlowChange>       TaskFlowChanges       { get { return this.GetTable<TaskFlowChange>(); } }
		public ITable<TaskMessage>          TaskMessages          { get { return this.GetTable<TaskMessage>(); } }
		public ITable<TaskProgress>         TaskProgresses        { get { return this.GetTable<TaskProgress>(); } }
		public ITable<TaskType>             TaskTypes             { get { return this.GetTable<TaskType>(); } }
		public ITable<TaskTypeAccumulator>  TaskTypeAccumulators  { get { return this.GetTable<TaskTypeAccumulator>(); } }
		public ITable<User>                 Users                 { get { return this.GetTable<User>(); } }
		public ITable<UserEmail>            UserEmails            { get { return this.GetTable<UserEmail>(); } }
		public ITable<UserPhone>            UserPhones            { get { return this.GetTable<UserPhone>(); } }

		public DevKitDB()
		{
			InitDataContext();
		}

		public DevKitDB(string configuration)
			: base(configuration)
		{
			InitDataContext();
		}

		partial void InitDataContext();
	}

	[Table(Schema="public", Name="Profile")]
	public partial class Profile
	{
		[PrimaryKey, Identity] public long   id            { get; set; } // bigint
		[Column,     Nullable] public string stName        { get; set; } // character varying(200)
		[Column,     Nullable] public string stPermissions { get; set; } // character varying(9999)
	}

	[Table(Schema="public", Name="Project")]
	public partial class Project
	{
		[PrimaryKey, Identity] public long      id                { get; set; } // bigint
		[Column,     Nullable] public string    stName            { get; set; } // character varying(99)
		[Column,     Nullable] public long?     fkUser            { get; set; } // bigint
		[Column,     Nullable] public long?     fkProjectTemplate { get; set; } // bigint
		[Column,     Nullable] public DateTime? dtCreation        { get; set; } // timestamp (6) without time zone
	}

	[Table(Schema="public", Name="ProjectPhase")]
	public partial class ProjectPhase
	{
		[PrimaryKey, Identity] public long      id        { get; set; } // bigint
		[Column,     Nullable] public long?     fkProject { get; set; } // bigint
		[Column,     Nullable] public string    stName    { get; set; } // character varying(99)
		[Column,     Nullable] public DateTime? dtStart   { get; set; } // timestamp (6) without time zone
		[Column,     Nullable] public DateTime? dtEnd     { get; set; } // timestamp (6) without time zone
		[Column,     Nullable] public bool?     bComplete { get; set; } // boolean
	}

	[Table(Schema="public", Name="ProjectSprint")]
	public partial class ProjectSprint
	{
		[PrimaryKey, Identity] public long      id            { get; set; } // bigint
		[Column,     Nullable] public long?     fkProject     { get; set; } // bigint
		[Column,     Nullable] public long?     fkPhase       { get; set; } // bigint
		[Column,     Nullable] public string    stName        { get; set; } // character varying(200)
		[Column,     Nullable] public string    stDescription { get; set; } // character varying(1000)
		[Column,     Nullable] public DateTime? dtStart       { get; set; } // timestamp (6) without time zone
		[Column,     Nullable] public DateTime? dtEnd         { get; set; } // timestamp (6) without time zone
		[Column,     Nullable] public bool?     bComplete     { get; set; } // boolean
	}

	[Table(Schema="public", Name="ProjectSprintVersion")]
	public partial class ProjectSprintVersion
	{
		[PrimaryKey, Identity] public long   id       { get; set; } // bigint
		[Column,     Nullable] public long?  fkSprint { get; set; } // bigint
		[Column,     Nullable] public string stName   { get; set; } // character varying(20)
	}

	[Table(Schema="public", Name="ProjectUser")]
	public partial class ProjectUser
	{
		[PrimaryKey, Identity] public long      id        { get; set; } // bigint
		[Column,     Nullable] public long?     fkProject { get; set; } // bigint
		[Column,     Nullable] public long?     fkUser    { get; set; } // bigint
		[Column,     Nullable] public string    stRole    { get; set; } // character varying(99)
		[Column,     Nullable] public DateTime? dtJoin    { get; set; } // timestamp (6) without time zone
	}

	[Table(Schema="public", Name="Setup")]
	public partial class Setup
	{
		[PrimaryKey, Identity] public long   id           { get; set; } // bigint
		[Column,     Nullable] public string stPhoneMask  { get; set; } // character varying(99)
		[Column,     Nullable] public string stDateFormat { get; set; } // character varying(99)
	}

	[Table(Schema="public", Name="Task")]
	public partial class Task
	{
		[PrimaryKey, Identity] public long      id                { get; set; } // bigint
		[Column,     Nullable] public string    stTitle           { get; set; } // character varying(200)
		[Column,     Nullable] public string    stLocalization    { get; set; } // character varying(200)
		[Column,     Nullable] public string    stDescription     { get; set; } // character varying(4000)
		[Column,     Nullable] public long?     fkProject         { get; set; } // bigint
		[Column,     Nullable] public long?     nuPriority        { get; set; } // bigint
		[Column,     Nullable] public long?     fkPhase           { get; set; } // bigint
		[Column,     Nullable] public long?     fkSprint          { get; set; } // bigint
		[Column,     Nullable] public long?     fkUserStart       { get; set; } // bigint
		[Column,     Nullable] public long?     fkVersion         { get; set; } // bigint
		[Column,     Nullable] public long?     fkTaskType        { get; set; } // bigint
		[Column,     Nullable] public long?     fkTaskCategory    { get; set; } // bigint
		[Column,     Nullable] public long?     fkTaskFlowCurrent { get; set; } // bigint
		[Column,     Nullable] public long?     fkReleaseVersion  { get; set; } // bigint
		[Column,     Nullable] public long?     fkUserResponsible { get; set; } // bigint
		[Column,     Nullable] public DateTime? dtStart           { get; set; } // timestamp (6) without time zone
		[Column,     Nullable] public DateTime? dtLastEdit        { get; set; } // timestamp (6) without time zone
		[Column,     Nullable] public bool?     bComplete         { get; set; } // boolean
	}

	[Table(Schema="public", Name="TaskAccumulatorValue")]
	public partial class TaskAccumulatorValue
	{
		[PrimaryKey, Identity] public long      id          { get; set; } // bigint
		[Column,     Nullable] public long?     fkTask      { get; set; } // bigint
		[Column,     Nullable] public long?     fkTaskAcc   { get; set; } // bigint
		[Column,     Nullable] public long?     fkUser      { get; set; } // bigint
		[Column,     Nullable] public DateTime? dtLog       { get; set; } // timestamp (6) without time zone
		[Column,     Nullable] public long?     nuValue     { get; set; } // bigint
		[Column,     Nullable] public long?     nuHourValue { get; set; } // bigint
		[Column,     Nullable] public long?     nuMinValue  { get; set; } // bigint
	}

	[Table(Schema="public", Name="TaskCategory")]
	public partial class TaskCategory
	{
		[PrimaryKey, Identity] public long   id            { get; set; } // bigint
		[Column,     Nullable] public long?  fkTaskType    { get; set; } // bigint
		[Column,     Nullable] public string stName        { get; set; } // character varying(200)
		[Column,     Nullable] public bool?  bManagement   { get; set; } // boolean
		[Column,     Nullable] public string stAbreviation { get; set; } // character varying(10)
		[Column,     Nullable] public string stDescription { get; set; } // character varying(500)
	}

	[Table(Schema="public", Name="TaskFlow")]
	public partial class TaskFlow
	{
		[PrimaryKey, Identity] public long   id             { get; set; } // bigint
		[Column,     Nullable] public long?  fkTaskType     { get; set; } // bigint
		[Column,     Nullable] public long?  fkTaskCategory { get; set; } // bigint
		[Column,     Nullable] public string stName         { get; set; } // character varying(200)
		[Column,     Nullable] public long?  nuOrder        { get; set; } // bigint
		[Column,     Nullable] public bool?  bForceComplete { get; set; } // boolean
		[Column,     Nullable] public bool?  bForceOpen     { get; set; } // boolean
	}

	[Table(Schema="public", Name="TaskFlowChange")]
	public partial class TaskFlowChange
	{
		[PrimaryKey, Identity] public long      id             { get; set; } // bigint
		[Column,     Nullable] public long?     fkTask         { get; set; } // bigint
		[Column,     Nullable] public long?     fkUser         { get; set; } // bigint
		[Column,     Nullable] public long?     fkOldFlowState { get; set; } // bigint
		[Column,     Nullable] public long?     fkNewFlowState { get; set; } // bigint
		[Column,     Nullable] public DateTime? dtLog          { get; set; } // timestamp (6) without time zone
		[Column,     Nullable] public string    stMessage      { get; set; } // character varying(300)
	}

	[Table(Schema="public", Name="TaskMessage")]
	public partial class TaskMessage
	{
		[PrimaryKey, Identity] public long      id            { get; set; } // bigint
		[Column,     Nullable] public long?     fkTask        { get; set; } // bigint
		[Column,     Nullable] public long?     fkUser        { get; set; } // bigint
		[Column,     Nullable] public long?     fkCurrentFlow { get; set; } // bigint
		[Column,     Nullable] public string    stMessage     { get; set; } // character varying(999)
		[Column,     Nullable] public DateTime? dtLog         { get; set; } // timestamp (6) without time zone
	}

	[Table(Schema="public", Name="TaskProgress")]
	public partial class TaskProgress
	{
		[PrimaryKey, Identity] public long      id             { get; set; } // bigint
		[Column,     Nullable] public long?     fkTask         { get; set; } // bigint
		[Column,     Nullable] public long?     fkUserAssigned { get; set; } // bigint
		[Column,     Nullable] public DateTime? dtLog          { get; set; } // timestamp (6) without time zone
	}

	[Table(Schema="public", Name="TaskType")]
	public partial class TaskType
	{
		[PrimaryKey, Identity] public long   id        { get; set; } // bigint
		[Column,     Nullable] public string stName    { get; set; } // character varying(200)
		[Column,     Nullable] public long?  fkProject { get; set; } // bigint
	}

	[Table(Schema="public", Name="TaskTypeAccumulator")]
	public partial class TaskTypeAccumulator
	{
		[PrimaryKey, Identity] public long   id             { get; set; } // bigint
		[Column,     Nullable] public long?  fkTaskType     { get; set; } // bigint
		[Column,     Nullable] public long?  fkTaskAccType  { get; set; } // bigint
		[Column,     Nullable] public long?  fkTaskFlow     { get; set; } // bigint
		[Column,     Nullable] public long?  fkTaskCategory { get; set; } // bigint
		[Column,     Nullable] public string stName         { get; set; } // character varying(30)
		[Column,     Nullable] public string stDisplay      { get; set; } // character varying(30)
	}

	[Table(Schema="public", Name="User")]
	public partial class User
	{
		[PrimaryKey, Identity] public long      id          { get; set; } // bigint
		[Column,     Nullable] public string    stLogin     { get; set; } // character varying(200)
		[Column,     Nullable] public string    stPassword  { get; set; } // character varying(30)
		[Column,     Nullable] public bool?     bActive     { get; set; } // boolean
		[Column,     Nullable] public long?     fkProfile   { get; set; } // bigint
		[Column,     Nullable] public DateTime? dtLastLogin { get; set; } // timestamp (6) without time zone
		[Column,     Nullable] public DateTime? dtCreation  { get; set; } // timestamp (6) without time zone
	}

	[Table(Schema="public", Name="UserEmail")]
	public partial class UserEmail
	{
		[PrimaryKey, Identity] public long   id      { get; set; } // bigint
		[Column,     Nullable] public long?  fkUser  { get; set; } // bigint
		[Column,     Nullable] public string stEmail { get; set; } // character varying(250)
	}

	[Table(Schema="public", Name="UserPhone")]
	public partial class UserPhone
	{
		[PrimaryKey, Identity] public long   id            { get; set; } // bigint
		[Column,     Nullable] public long?  fkUser        { get; set; } // bigint
		[Column,     Nullable] public string stPhone       { get; set; } // character varying(50)
		[Column,     Nullable] public string stDescription { get; set; } // character varying(50)
	}

	public static partial class TableExtensions
	{
		public static Profile Find(this ITable<Profile> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static Project Find(this ITable<Project> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static ProjectPhase Find(this ITable<ProjectPhase> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static ProjectSprint Find(this ITable<ProjectSprint> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static ProjectSprintVersion Find(this ITable<ProjectSprintVersion> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static ProjectUser Find(this ITable<ProjectUser> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static Setup Find(this ITable<Setup> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static Task Find(this ITable<Task> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static TaskAccumulatorValue Find(this ITable<TaskAccumulatorValue> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static TaskCategory Find(this ITable<TaskCategory> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static TaskFlow Find(this ITable<TaskFlow> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static TaskFlowChange Find(this ITable<TaskFlowChange> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static TaskMessage Find(this ITable<TaskMessage> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static TaskProgress Find(this ITable<TaskProgress> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static TaskType Find(this ITable<TaskType> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static TaskTypeAccumulator Find(this ITable<TaskTypeAccumulator> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static User Find(this ITable<User> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static UserEmail Find(this ITable<UserEmail> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}

		public static UserPhone Find(this ITable<UserPhone> table, long id)
		{
			return table.FirstOrDefault(t =>
				t.id == id);
		}
	}
}
