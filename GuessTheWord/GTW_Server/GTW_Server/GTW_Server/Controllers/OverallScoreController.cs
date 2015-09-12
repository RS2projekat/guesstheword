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
    public class OverallScoresController : ApiController
    {
        [HttpGet]
        [Route("GTW/OverallScores/")]
        public IEnumerable<OverallScore> GetOverallScores()
        {
            try
            {
                using (OverallScoreServices ws = new OverallScoreServices())
                {
                    return ws.getOverallScores();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("GTW/OverallScores/")]
        public IEnumerable<OverallScore> GetOverallScores(User user)
        {
            try
            {
                using (OverallScoreServices ws = new OverallScoreServices())
                {
                    return ws.getOverallScores(user);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}