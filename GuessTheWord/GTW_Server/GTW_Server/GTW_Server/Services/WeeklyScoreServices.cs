using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GTW_Server.DAL;
using GTW_Server.DAL.Models;
using System.Data.Entity;

namespace GTW_Server.Services
{
    public class WeeklyScoreServices : IDisposable
    {
        public IEnumerable<WeeklyScore> getWeeklyScores()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                try
                {
                    return db.WeeklyScores.ToList().OrderByDescending(x => x.Score);
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