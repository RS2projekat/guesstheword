using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GTW_Server.Services;

namespace GTW_Server
{
    public class ServiceContainer : IDisposable
    {
         private static ServiceContainer instance;

        public DatabaseServices databaseServices 
        {
            get;
            set;
        }
        private ServiceContainer() 
        {
            databaseServices = new DatabaseServices();

        }
        public static ServiceContainer Instance 
        {
            get
            {
                if(instance == null)
                {
                    instance = new ServiceContainer();
                }
                return instance;
            }
        }

        public void Dispose()
        {
        }
    }
}