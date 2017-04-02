﻿using LinqToDB;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace DataModel
{
	public partial class TaskType
	{
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

						ent.fkTaskType = id;

						if (ent.id == 0)
						{
							if ((from ne in db.TaskCategories
								 where ne.stName.ToUpper() == ent.stName.ToUpper() 
								 where ne.fkTaskType == id
								 select ne).Any())
							{
								resp = "Category already added to task type!";
								return false;
							}

							ent.id = Convert.ToInt64(db.InsertWithIdentity(ent));

							int order = 1;

							db.Insert(new TaskFlow()
							{
								fkTaskCategory = ent.id,
								fkTaskType = id,
								nuOrder = order++,
								bForceOpen = true,
								stName = "Open"
							});

							db.Insert(new TaskFlow()
							{
								fkTaskCategory = ent.id,
								fkTaskType = id,
								nuOrder = order++,
								bForceOpen = true,
								stName = "Re-Open"
							});

							db.Insert(new TaskFlow()
							{
								fkTaskCategory = ent.id,
								fkTaskType = id,
								nuOrder = order++,
								stName = "Analysis"
							});

							db.Insert(new TaskFlow()
							{
								fkTaskCategory = ent.id,
								fkTaskType = id,
								nuOrder = order++,
								stName = "Development"
							});

							db.Insert(new TaskFlow()
							{
								fkTaskCategory = ent.id,
								fkTaskType = id,
								nuOrder = order++,
								bForceComplete = true,
								stName = "Closed"
							});

							db.Insert(new TaskFlow()
							{
								fkTaskCategory = ent.id,
								fkTaskType = id,
								nuOrder = order++,
								bForceComplete = true,
								stName = "Cancelled"
							});
						}
						else
							db.Update(ent);

						categories = LoadCategories(db);

						break;
					}

				case "removeCategorie":
					{
						var categDel = JsonConvert.DeserializeObject<TaskCategory>(anexedEntity.ToString());

						if ((from e in db.Tasks where e.fkTaskCategory == categDel.id select e).Any())
						{
							resp = "This category is being used in a task";
							return false;
						}

						db.Delete(categDel);

						categories = LoadCategories(db);
						break;
					}

				case "newFlow":
					{
						var ent = JsonConvert.DeserializeObject<TaskFlow>(anexedEntity.ToString());

						if (ent.id == 0)
						{
							if ((from e in db.TaskFlows
								 where e.fkTaskCategory == ent.fkTaskCategory
								 where e.fkTaskType == ent.fkTaskType
								 where e.stName.ToUpper() == ent.stName.ToUpper() 
								 select e).Any())
							{
								resp = "Flow already added to task type!";
								return false;
							}

							ent.fkTaskType = id;

							db.Insert(ent);
						}
						else
							db.Update(ent);
						
						break;
					}

				case "removeFlow":
					{
						var flowDel = JsonConvert.DeserializeObject<TaskFlow>(anexedEntity.ToString());

						if ((from e in db.Tasks where e.fkTaskFlowCurrent == flowDel.id select e).Any())
						{
							resp = "This flow is being used in a task";
							return false;
						}

						db.Delete(flowDel);
						break;
					}

				case "newAcc":
					{
						var ent = JsonConvert.DeserializeObject<TaskTypeAccumulator>(anexedEntity.ToString());

						if (ent.id == 0)
						{
							if ((from e in db.TaskTypeAccumulators
								 where e.fkTaskFlow == ent.fkTaskFlow
								 where e.fkTaskCategory == ent.fkTaskCategory
								 where e.fkTaskType == ent.fkTaskType
								 where e.stName.ToUpper() == ent.stName.ToUpper() 
								 select e).Any())
							{
								resp = "Accumulator already added to task type!";
								return false;
							}

							ent.fkTaskType = id;

							db.Insert(ent);
						}
						else
							db.Update(ent);

						break;
					}

				case "removeAcc":
					{
						var accDel = JsonConvert.DeserializeObject<TaskTypeAccumulator>(anexedEntity.ToString());

						db.Delete(accDel);
						break;
					}
			}

			return true;
		}
	}
}