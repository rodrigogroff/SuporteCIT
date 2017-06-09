﻿using System.Linq;
using System.Collections.Generic;

namespace DataModel
{
	public partial class DevKitDB
	{				
		public User currentUser = null;

        public bool ValidateUser (LoginInfo login)
        {
            if (currentUser == null)
                GetCurrentUser(login.idUser);

            if (currentUser == null)
                return false;

            if (currentUser.stCurrentSession != login.session)
                return false;
            
            return true;
        }

		User GetCurrentUser(long fkUser)
		{
			if (currentUser == null)
				currentUser = (from ne in User
							   where ne.id == fkUser
                               select ne).
                               FirstOrDefault();

			return currentUser;
		}
        
		public List<long?> GetCurrentUserProjects()
		{
			return (from e in ProjectUser
					where e.fkUser == currentUser.id
					select e.fkProject).
					ToList();
		}
    }
}
