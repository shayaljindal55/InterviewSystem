using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI.WebControls;
using InterviewSystem.Models;

namespace InterviewSystem.Areas.Interviewer.Controllers
{
    [Authorize(Roles = "Interviewer")]
    public class Int_ManageController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        public string interviewForPost;
        // GET: Interviewer/Manage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InterviewerProfile()
        {
            return View();
        }

        //[HttpPost]
        //public JsonResult RemoveProfileImage()
        //{
        //    var user1 = context.Users.FirstOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
        //    InterviewerDetails user = new InterviewerDetails();
        //    user.Image = DateTime.UtcNow.ToBinary() + System.IO.Path.GetExtension("~/images/default-user.png");
        //    context.InterviewerDetails.Add(user);
        //    //user.CandidateDetails.Image=
        //    return Json(new { success = true });
        //}

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpGet]
        public ActionResult EditInterviewerProfile()
        {
            var user1 = context.Users.FirstOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            //ViewBag.Image = null;

            var getInterviewDetail = context.InterviewerDetails.Where(x => x.User.UserName == HttpContext.User.Identity.Name).OrderByDescending(y=>y.ID).FirstOrDefault();       
            InterviewerDetailsModel interviewerDetailsModel = new InterviewerDetailsModel();
            if (getInterviewDetail != null)
            {
                interviewerDetailsModel.FirstName = user1.FirstName;
                interviewerDetailsModel.LastName = user1.LastName;
                interviewerDetailsModel.InterviewerDetails = getInterviewDetail;
                interviewerDetailsModel.InterviewerDetails.Image = getInterviewDetail.Image;
            }
            return View(interviewerDetailsModel);
        }

        [HttpPost]
        public ActionResult SaveInterviewerProfile(InterviewerDetailsModel interviewer, HttpPostedFileBase file)
        {
            InterviewerDetailsModel user = new InterviewerDetailsModel();

            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string filename = DateTime.UtcNow.ToBinary() + System.IO.Path.GetExtension(file.FileName);
                user.InterviewerDetails.Image = filename;
                string path = System.IO.Path.Combine(
                Server.MapPath("~/images/Int_Profile"), filename);
                file.SaveAs(path);
            }

            if (ModelState.IsValid)
            {
                var model = context.InterviewerDetails.FirstOrDefault(x => x.ID == user.InterviewerDetails.ID);
                if (model != null)
                {
                    context.Entry(model).State = EntityState.Deleted;
                    context.SaveChanges();
                }

                var user1 = context.Users.FirstOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
                user.InterviewerDetails.User = user1;
                user.InterviewerDetails.ID = interviewer.InterviewerDetails.ID;
                user.InterviewerDetails.Phone = interviewer.InterviewerDetails.Phone;
                user.InterviewerDetails.Designation = interviewer.InterviewerDetails.Designation;
                context.InterviewerDetails.Add(user.InterviewerDetails);
                context.SaveChanges();
                user1.FirstName = interviewer.FirstName;
                user1.LastName = interviewer.LastName;
                context.Entry(user1).State = EntityState.Modified;
                context.SaveChanges();
            }

            return RedirectToAction("EditInterviewerProfile");
        }


        public ActionResult DeleteInterviewerProfile()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GiveRating()
        {
            ViewBag.InterviewForProfile = context.Departments.Select(x => new SelectListItem
            {
                Value = x.DepartmentName,
                Text = x.DepartmentName,
            });
            int postId = 0;
            ViewBag.CandidateDetails = context.CandidateDetails.Where(x => x.ApplyingForPost.Id == postId).Select(y => new SelectListItem
            {
                Value = y.User.Id,
                Text = y.User.FirstName + " " + y.User.LastName,
            }).ToList();

            ViewBag.Rounds = context.InterviewRounds.Where(x => x.Department.Id == postId).Select(y => new SelectListItem
            {
                Value = y.Id.ToString(),
                Text = y.Description,
            }).ToList();
            var giveRating = new GiveRating();
            CandidateDetails candDetail = new CandidateDetails();
            return View(giveRating);
        }

        [HttpPost]
        public ActionResult SaveRating(GiveRating model)
        {
            InterviewDetail intDetail = new InterviewDetail();

            intDetail.InterviewForPost = context.Departments.FirstOrDefault(x => x.DepartmentName == model.InterviewForPost);
            intDetail.Round = context.InterviewRounds.FirstOrDefault(x => x.Id == model.Rounds);
            intDetail.Interviewer = context.Users.FirstOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            intDetail.Candidate = context.Users.FirstOrDefault(x => x.Id == model.CandidateId);
            intDetail.Rating = model.Rate;
            intDetail.InterviewDate = DateTime.UtcNow;
            context.InterviewDetail.Add(intDetail);
            context.SaveChanges();
            return RedirectToAction("GiveRating");


        }


        public JsonResult GetRoundsList(string postName)
        {
            InterviewDetail details = new InterviewDetail();

            details.InterviewForPost = context.Departments.FirstOrDefault(x => x.DepartmentName == postName);
            int postId = details.InterviewForPost.Id;
            var CandidateDetails = context.CandidateDetails.Where(x => x.ApplyingForPost.Id == postId).Select(y => new SelectListItem
            {
                Value = y.User.Id,
                Text = y.User.FirstName + " " + y.User.LastName,
            }).ToList();

            var Rounds = context.InterviewRounds.Where(x => x.Department.Id == postId).Select(y => new SelectListItem
            {
                Value = y.Id.ToString(),
                Text = y.Description,
            }).ToList();
            dynamic data = new { candidateName = CandidateDetails, rounds = Rounds };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        public JsonResult GetRatingForTest(int roundId, string userId, string postName)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var interviewer = context.Users.FirstOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            var getRating = context.InterviewDetail.Where(x => x.Interviewer.Id == interviewer.Id);
            var getInterviewedUser = getRating.Where(x => x.Candidate.Id == userId);
            var getRatingforRound = getInterviewedUser.Where(x => x.Round.Id == roundId);

            return Json(getRatingforRound.Count());
        }

        public ActionResult ViewRank()
        {
            IList<CandidateResult> candidateResults = new List<CandidateResult>();
            var interviewDetails = context.InterviewDetail.ToList();
            var candIds = interviewDetails.Select(y => y.Candidate.Id).ToList();
            var allUsers = context.Users.Where(x => candIds.Contains(x.Id)).ToList();

            var interviewDetailsGroups = interviewDetails.OrderBy(l => l.InterviewDate).GroupBy(x => x.Candidate.UserName).Select(g => new
            {
                CurrentUser = g.Key,
                TotalRatings = g.Sum(k => k.Rating),
                RoundsTaken = g.GroupBy(a => a.Round.Id).Select(r => new RoundDetails { RoundName = r.FirstOrDefault().Round.Description, TotalRound = r.Count(), TotalRating = r.Sum(t => t.Rating) })
            }).OrderByDescending(y => y.TotalRatings);

            foreach (var user in allUsers)
            {
                CandidateResult candidateResult = new CandidateResult();

                var candidate = interviewDetails.Where(x => x.Candidate.UserName == user.UserName);
                candidateResult.FirstName = user.FirstName;
                candidateResult.LastName = user.LastName;
                candidateResult.InterviewRounds.AddRange(interviewDetailsGroups.Where(x => x.CurrentUser == user.UserName).Select(a => a.RoundsTaken).FirstOrDefault().ToList());
                candidateResult.Rank = 0;
                foreach (var interviewDetailsGroup in interviewDetailsGroups)
                {
                    if (interviewDetailsGroup.CurrentUser == user.UserName)
                    {
                        candidateResult.Rank++;
                        break;
                    }
                    candidateResult.Rank++;
                }
                candidateResults.Add(candidateResult);
            }

            return View(candidateResults);
        }
    }
}