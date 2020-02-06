﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Master
{
    public class LocalNetwork
    {
        public const string Secret = "ciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEiLCJuYmYiOjE1NTc5Mjk4ODcsImV4cCI6MTU1fhdsjhfeuyrejhdfj73333";

        public string sqlServer { get; set; }

        public string sqlServerProd { get; set; }

        public string conveynetApi { get; set; }

        public string conveynetApiProd { get; set; }

        public string GetSqlServer ()
        {
#if DEBUG
            return sqlServer;
#endif


#if RELEASE
            return sqlServerProd;
#endif
        }

        public string GetConveyNetAPI()
        {
#if DEBUG
            return conveynetApi;
#endif


#if RELEASE
            return conveynetApiProd;
#endif
        }
    }
}