using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTW_Server
{
    public class ServerContext : IDisposable
    {
        private static ServerContext instance;

        public int proba
        {
            get;
            set;
        }
        private ServerContext() 
        { }
        public static ServerContext Instance 
        {
            get
            {
                if(instance == null)
                {
                    instance = new ServerContext();
                }
                return instance;
            }
        }


        public void Dispose()
        {
        }
    }
}