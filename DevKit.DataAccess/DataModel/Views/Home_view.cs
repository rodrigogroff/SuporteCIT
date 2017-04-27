﻿using System.Linq;
using System.Collections.Generic;

namespace DataModel
{
	public class HomeDTO
	{
		public string name;

		public List<CompanyNews> news = new List<CompanyNews>();
		public List<Survey> surveys = new List<Survey>();
	}

	public class HomeView
	{
		public HomeDTO ComposedFilters(DevKitDB db)
		{
			var user = db.GetCurrentUser();
			var projects = db.GetCurrentUserProjects();

			var dto = new HomeDTO();

			dto.name = "Hi " + user.stLogin;

			#region - news - 
			{
				var news = from e in db.CompanyNews
						   where e.fkProject == null || projects.Contains(e.fkProject)
						   select e;

				var newsRead = (from e in db.UserNewsReads where e.fkUser == user.id select e.fkNews).ToList();

				if (newsRead.Count() > 0)
					news = from e in news where !newsRead.Contains(e.id) select e;

				dto.news = news.OrderByDescending(y => y.id).ToList();

				foreach (var item in dto.news)
					item.LoadAssociations(db);
			}
			#endregion

			#region - survey - 
			{
				var surveys = from e in db.Surveys
							  where e.fkProject == null || projects.Contains(e.fkProject)
							  where e.bActive == true
							  select e;

				var surveysMark = (from e in db.SurveyUserOptions
								   where e.fkUser == user.id
								   select e).
								   ToList();

				dto.surveys = surveys.OrderByDescending(y => y.id).ToList();

				var nuGlobalUsers = (from e in db.Users where e.bActive == true select e).Count();

				foreach (var survey in dto.surveys)
				{
					survey.LoadAssociations(db);

					var nuSelUsers = (from e in db.SurveyUserOptions
											where e.fkSurvey == survey.id
											select e).
											Count();

					if (survey.fkProject == null)
					{
						survey.sNuPct = (nuSelUsers * 100 / nuGlobalUsers).ToString();
					}
					else
					{
						var nuProjectUsers = (from e in db.ProjectUsers where e.fkProject == survey.fkProject select e).Count();

						survey.sNuPct = (nuSelUsers * 100 / nuProjectUsers).ToString();
					}					

					foreach (var option in survey.options)
					{
						option.bChecked = surveysMark.Any(y => y.fkSurvey == survey.id && y.fkUser == user.id && y.fkSurveyOption == option.id);

						if (option.bChecked)
							survey.bUserSelected = true;

						var nuSelThisOption = surveysMark.Count(y => y.fkSurvey == survey.id && y.fkSurveyOption == option.id);

						if (nuSelThisOption > 0)
							option.sNuOptionPct = (nuSelUsers * 100 / nuSelThisOption).ToString();
						else
							option.sNuOptionPct = "0";
					}
				}					
			}
			#endregion

			return dto;
		}
	}
}