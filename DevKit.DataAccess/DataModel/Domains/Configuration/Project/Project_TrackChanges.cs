﻿
namespace DataModel
{
	public partial class Project
	{
		public string TrackChanges(DevKitDB db)
		{
			var ret = "";

			var oldEntity = db.GetProject(this.id);

			if (oldEntity.stName != this.stName)
				ret += "Nome: " + oldEntity.stName + " => " + this.stName + ";";

			return ret;
		}
	}
}
