﻿using LinqToDB;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel
{
	public class SurveyFilter : BaseFilter
    {
		public long? fkProject;

		public string ExportString()
		{
			var ret = "";

			ret += "Skip: " + skip + " ";
			ret += "take: " + take + " ";

			if (fkProject != null)
				ret += "fkProject: " + fkProject + " ";

			return ret;
		}

        public string Parameters()
        {
            return Export();
        }

        string _exportResults = "";

        string Export()
        {
            if (_exportResults != "")
                return _exportResults;

            var ret = new StringBuilder();

            // base
            ret.Append(skip + ",");
            ret.Append(take + ",");
            ret.Append(busca + ",");

            if (fkProject != null)
                ret.Append(fkProject);
            ret.Append(",");

            _exportResults = ret.ToString();

            return _exportResults;
        }
    }
	
	public partial class Survey
	{
		public List<Survey> ComposedFilters(DevKitDB db, ref int count, SurveyFilter filter)
		{
			var user = db.currentUser;
			var lstUserProjects = db.GetCurrentUserProjects();

			var query = from e in db.Survey select e;

            if (!string.IsNullOrEmpty(filter.busca))
                query = from e in query
						where e.stTitle.ToUpper().Contains(filter.busca) ||
							  e.stMessage.ToUpper().Contains(filter.busca)
						select e;

			if (lstUserProjects.Count() > 0)
				query = from e in query where lstUserProjects.Contains(e.id) select e;

			if (filter.fkProject != null)
			{
				query = from e in query
						join eUser in db.ProjectUser on e.id equals eUser.fkProject
						where e.id == eUser.fkProject
						where eUser.fkUser == filter.fkProject
						select e;
			}

			count = query.Count();

			query = query.OrderBy(y => y.id);

            return Loader(db, (query.Skip(() => filter.skip).Take(() => filter.take)).ToList(), true);
        }
	}
}
