using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HRMIS_PERSONEL_PROFILE.Models;
using Microsoft.Reporting.WebForms;

namespace HRMIS_PERSONEL_PROFILE.Controllers
{
    public class BioDataController : Controller
    {
        private Live_HRMISEntities1 db = new Live_HRMISEntities1();

        // GET: BioData
        public ActionResult Index()
        {
            var v = db.PERSONAL_INFORMATION.ToList();
            return View(v);
        }

        public ActionResult StateArea()
        {

            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
