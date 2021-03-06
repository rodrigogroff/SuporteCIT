﻿using LinqToDB;
using System.Linq;

namespace DataModel
{
	public partial class Person
	{
        public ComboReport ComboFilters(DevKitDB db, string searchItem)
        {
            var query = from e in db.Person select e;

            if (searchItem != "")
                query = from e in query
                        where e.stName.ToUpper().Contains(searchItem)
                        select e;

            query = from e in query orderby e.stName select e;

            return new ComboReport
            {
                count = query.Count(),
                results = (from e in query select new BaseComboResponse { id = e.id, stName = e.stName }).ToList()
            };
        }
    }
}
