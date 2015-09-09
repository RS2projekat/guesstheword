using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GTW_Server.DAL;
using GTW_Server.DAL.Models;
using System.Data.Entity;

namespace GTW_Server.Services
{
    public class OverallScoreServices : IDisposable
    {
        public IEnumerable<OverallScore> getOverallScores()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    return db.OverallScores.ToList();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}