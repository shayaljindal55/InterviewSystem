using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using InterviewSystem.Models;

namespace InterviewSystem.Areas.Candidate.Controllers
{
    [Authorize(Roles = "Candidate")]
    public class Cand_ManageController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Candidate/Manage
        public ActionResult Index()
        {
            return View();
        }

     

        [HttpGet]
        public ActionResult EditUserProfile()       
        {
            var user1 = context.Users.FirstOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            var getCandidateDetails = context.CandidateDetails.Where(x => x.User.UserName == HttpContext.User.Identity.Name).OrderByDescending(y=>y.ID).FirstOrDefault();
            ViewBag.ApplyingForPost = context.Departments.Select(x => new SelectListItem
            {
                Value = x.DepartmentName,
                Text = x.DepartmentName,
            });          
            CandidateDetailsModel candidateDetailsMOdel = new CandidateDetailsModel();
            if(getCandidateDetails != null)
            {
                candidateDetailsMOdel.FirstName = user1.FirstName;
                candidateDetailsMOdel.LastName = user1.LastName;
                candidateDetailsMOdel.CandidateDetails = getCandidateDetails;
            }
            return View(candidateDetailsMOdel);
        }


        [HttpPost]
        public ActionResult EditUserProfile(CandidateDetailsModel cand, HttpPostedFileBase file,FormCollection form)
        {
            CandidateDetailsModel user = new CandidateDetailsModel();
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string filename = DateTime.UtcNow.ToBinary() + System.IO.Path.GetExtension(file.FileName);
                user.CandidateDetails.Image = filename;
                string path = System.IO.Path.Combine(
                Server.MapPath("~/images/Candidate_Profile"), filename);
                file.SaveAs(path);
            }

            if (ModelState.IsValid)
            {
                var model = context.CandidateDetails.FirstOrDefault(x => x.ID == cand.CandidateDetails.ID);
                if (model != null)
                {
                    context.Entry(model).State = EntityState.Deleted;
                    context.SaveChanges();
                }
                string postName=form["ApplyingForPost"];
                var user1 = context.Users.FirstOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
                user.CandidateDetails.MaritalStatus = cand.CandidateDetails.MaritalStatus;
                user.CandidateDetails.NumberOfGapYears = cand.CandidateDetails.NumberOfGapYears;
                user.CandidateDetails.ID = cand.CandidateDetails.ID;
                user.CandidateDetails.ApplyingForPost = context.Departments.FirstOrDefault(x => x.DepartmentName == postName);
                user.CandidateDetails.Phone = cand.CandidateDetails.Phone;
                user.CandidateDetails.PinCode = cand.CandidateDetails.PinCode;
                user.CandidateDetails.PreviousOrganization = cand.CandidateDetails.PreviousOrganization;
                user.CandidateDetails.StandingBackLogs = cand.CandidateDetails.StandingBackLogs;
                user.CandidateDetails.User = user1;
                user.CandidateDetails.YearOfPassing = cand.CandidateDetails.YearOfPassing;
                user.CandidateDetails.Gender = cand.CandidateDetails.Gender;
                user.CandidateDetails.GapInEducation = cand.CandidateDetails.GapInEducation;
                user.CandidateDetails.ExperienceInYears = cand.CandidateDetails.ExperienceInYears;
                user.CandidateDetails.DegreeAggregate = cand.CandidateDetails.DegreeAggregate;
                user.CandidateDetails.Degree = cand.CandidateDetails.Degree;
                user.CandidateDetails.DateOfBirth = cand.CandidateDetails.DateOfBirth;
                user.CandidateDetails.Address = cand.CandidateDetails.Address;
                context.CandidateDetails.Add(user.CandidateDetails);
                context.SaveChanges();
                user1.FirstName = cand.FirstName;
                user1.LastName = cand.LastName;
                context.Entry(user1).State = EntityState.Modified;
                context.SaveChanges();
            }

            return RedirectToAction("EditUserProfile");
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", new { area = "" });
         
        }

        public ActionResult DeleteUserProfile()
        {
            return View();
        }
        public ActionResult ViewResult()
        {
            CandidateResult cand = new CandidateResult();
            var user = context.Users.FirstOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            cand.FirstName = user.FirstName;
            cand.LastName = user.LastName;
            var candidate = context.InterviewDetail.Where(x => x.Candidate.UserName == HttpContext.User.Identity.Name);

            cand.AverageRating = "Not Rated.";
            cand.Result = "No Result Found.";
            if (candidate.Count() > 0)
            {
                var totalRating = Convert.ToDecimal(candidate.Sum(x => x.Rating));
                var counter = candidate.Count();
                var ratingCount = (totalRating / counter);
                cand.AverageRating = ratingCount.ToString();
                cand.Result = ratingCount >= 3 ? "Pass" : "Fail";
            }

            var allUsers = context.InterviewDetail.OrderBy(l => l.InterviewDate).GroupBy(x => x.Candidate.UserName).Select(g => new
               {
                   CurrentUser = g.Key,
                   TotalRatings = g.Sum(k => k.Rating)
               }).OrderByDescending(y => y.TotalRatings);

            cand.Rank = 0;
            foreach (var rateduser in allUsers)
            {
                if (rateduser.CurrentUser == HttpContext.User.Identity.Name)
                {
                    cand.Rank++;
                    break;
                }
                cand.Rank++;
            }

            return View(cand);
        }
    }
}