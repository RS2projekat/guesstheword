using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GTW_Server.DAL.Models;
using GTW_Server.Services;
using System.Web.Http.Description;

namespace GTW_Server.Controllers
{
    public class WeeklyScoresController : ApiController
    {
        [HttpGet]
        [Route("GTW/WeeklyScores/")]
        public IEnumerable<WeeklyScore> GetWeeklyScores()
        {
            try
            {
                using (WeeklyScoreServices ws = new WeeklyScoreServices())
                {
                    return ws.getWeeklyScores();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}