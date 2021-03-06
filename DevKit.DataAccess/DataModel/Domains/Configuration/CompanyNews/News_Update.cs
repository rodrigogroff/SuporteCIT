﻿using LinqToDB;
using System;

namespace DataModel
{
	public partial class CompanyNews
	{
		public bool Update(DevKitDB db, ref string resp)
		{
			var user = db.currentUser;
            
			switch (updateCommand)
			{
				case "entity":
					{
                        db.Update(this);
						break;
					}

				case "maskAsRead":
					{
						db.Insert(new UserNewsRead {
							dtLog = DateTime.Now,
							fkNews = this.id,
							fkUser = user.id
						});

						break;
					}
			}

			return true;
		}
	}
}
