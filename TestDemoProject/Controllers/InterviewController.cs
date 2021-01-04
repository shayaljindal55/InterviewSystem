using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InterviewSystem.Controllers
{
    public class InterviewController : Controller
    {
        // GET: Interview
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ScoreBoard()
        {
            return View();
        }

        public ActionResult RegisterCandidate()
        {
            return View();
        }

        public ActionResult RegisterInterviewer()
        {
            return View();
        }
    }
}