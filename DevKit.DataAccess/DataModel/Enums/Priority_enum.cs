﻿using System.Collections.Generic;

namespace DataModel
{
	public class Priority
	{
		public long id { get; set; }
		public string stName { get; set; }
	}

	public class EnumPriority
	{
		public List<Priority> lst = new List<Priority>();

		public EnumPriority()
		{
			lst.Add(new Priority() { id = 1, stName = "Emergency" });
			lst.Add(new Priority() { id = 2, stName = "High" });
			lst.Add(new Priority() { id = 3, stName = "Normal" });
			lst.Add(new Priority() { id = 4, stName = "Low" });
			lst.Add(new Priority() { id = 5, stName = "Register" });
		}
	}
}