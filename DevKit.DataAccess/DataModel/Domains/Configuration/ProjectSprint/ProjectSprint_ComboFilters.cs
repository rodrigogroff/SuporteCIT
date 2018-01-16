﻿using LinqToDB;
using System.Linq;

namespace DataModel
{
	public partial class ProjectSprint
	{
        public ComboReport ComboFilters(DevKitDB db, ProjectSprintFilter filter)
        {
            var query = from e in db.ProjectSprint
                        join proj in db.Project on e.fkProject equals proj.id
                        where proj.fkEmpresa == filter.fkEmpresa
                        select e;

            if (filter.busca != "")
                query = from e in query
                        where e.stName.ToUpper().Contains(filter.busca)
                        select e;

            if (filter.fkProject != null)
                query = from e in query
                        where e.fkProject == filter.fkProject
                        select e;

            if (filter.fkPhase != null)
                query = from e in query
                        where e.fkPhase == filter.fkPhase
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
