using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolGradingSystem.Models.EntityFramework;

namespace SchoolGradingSystem.Controllers
{
    public class ClubsController : Controller
    {
        // GET: Clubs
        DbSchoolEntities dbClubs = new DbSchoolEntities();
        public ActionResult Clubs()
        {
            var clubList = dbClubs.Clubs.ToList();
            return View(clubList);
        }

        [HttpGet]
        public ActionResult AddClub()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var clubsUpdate = dbClubs.Clubs.Find(id);
            return View("Update", clubsUpdate);
        }

        [HttpPost]
        public ActionResult AddClub(Clubs club)
        {
            if (!String.IsNullOrEmpty(club.ClubName))
            {
                if (ValidateUniqueClub(club))
                {
                    dbClubs.Clubs.Add(club);
                    dbClubs.SaveChanges();
                    return View();
                }
            }

            return View();
        }

        public ActionResult Remove(int id) 
        {
            var club = dbClubs.Clubs.Find(id);
            dbClubs.Clubs.Remove(club);
            dbClubs.SaveChanges();
            return RedirectToAction("Clubs");
        }

        [HttpPost]
        public ActionResult UpdateClubs(Clubs club)
        {
            if (ModelState.IsValid) 
            {
                if (ValidateUniqueClub(club))
                {
                    var clubsUpdate = dbClubs.Clubs.Find(club.ClubID);
                    clubsUpdate.ClubName = club.ClubName;
                    clubsUpdate.ClubLimit = club.ClubLimit;
                    dbClubs.SaveChanges();
                    return RedirectToAction("Clubs", "Clubs");
                }

            }
            return RedirectToAction("Clubs", "Clubs");
        }

        private bool ValidateUniqueClub(Clubs club)
        {
            var existingClub = dbClubs.Clubs.FirstOrDefault(i => i.ClubName == club.ClubName);
            if(existingClub == null)
            {
                if (club.ClubLimit > 10 && club.ClubLimit < 100)
                {
                    return true;
                }
            }
            return false;
        }
    }
}