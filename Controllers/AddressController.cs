using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using HRMIS_PERSONEL_PROFILE.documents;
using HRMIS_PERSONEL_PROFILE.Models;
using HRMIS_PERSONEL_PROFILE.viewmodel;
using Microsoft.Reporting.WebForms;

namespace HRMIS_PERSONEL_PROFILE.Controllers
{
    public class AddressController : Controller
    {
        private Live_HRMISEntities1 db = new Live_HRMISEntities1();

        // GET: Address
        public ActionResult Index()
        {
            return View(db.PHYSICAL_ADDRESSES.ToList());
        }

        // GET: Address/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHYSICAL_ADDRESSES pHYSICAL_ADDRESSES = db.PHYSICAL_ADDRESSES.Find(id);
            if (pHYSICAL_ADDRESSES == null)
            {
                return HttpNotFound();
            }
            return View(pHYSICAL_ADDRESSES);
        }

        // GET: Address/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Address/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PhysicalAddressID,EmpID,FNumber,Village,Parish,Subcounty,County,DistrictID,Town,PostalAddress,Email,FaxNo,MobilePhone,FixedTelephone,RecordedBy,DateRecorded")] PHYSICAL_ADDRESSES pHYSICAL_ADDRESSES)
        {
            if (ModelState.IsValid)
            {
                db.PHYSICAL_ADDRESSES.Add(pHYSICAL_ADDRESSES);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pHYSICAL_ADDRESSES);
        }

        // GET: Address/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHYSICAL_ADDRESSES pHYSICAL_ADDRESSES = db.PHYSICAL_ADDRESSES.Find(id);
            if (pHYSICAL_ADDRESSES == null)
            {
                return HttpNotFound();
            }
            return View(pHYSICAL_ADDRESSES);
        }

        // POST: Address/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhysicalAddressID,EmpID,FNumber,Village,Parish,Subcounty,County,DistrictID,Town,PostalAddress,Email,FaxNo,MobilePhone,FixedTelephone,RecordedBy,DateRecorded")] PHYSICAL_ADDRESSES pHYSICAL_ADDRESSES)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pHYSICAL_ADDRESSES).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pHYSICAL_ADDRESSES);
        }

        // GET: Address/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHYSICAL_ADDRESSES pHYSICAL_ADDRESSES = db.PHYSICAL_ADDRESSES.Find(id);
            if (pHYSICAL_ADDRESSES == null)
            {
                return HttpNotFound();
            }
            return View(pHYSICAL_ADDRESSES);
        }

        // POST: Address/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PHYSICAL_ADDRESSES pHYSICAL_ADDRESSES = db.PHYSICAL_ADDRESSES.Find(id);
            db.PHYSICAL_ADDRESSES.Remove(pHYSICAL_ADDRESSES);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult GetUserDetails()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetUserDetails(PERSONAL_INFORMATION users)
        {
            try
            {
                var empid = db.PERSONAL_INFORMATION.Where(a => a.FileNo == users.FileNo).Select(a => a.EmpID).FirstOrDefault();
                var FileNo = db.PERSONAL_INFORMATION.Where(a => a.FileNo == users.FileNo).FirstOrDefault();
                var NIN = db.PERSONAL_INFORMATION.Where(a => a.EmpID == empid).Select(a => a.NIN).FirstOrDefault();
                var userninid = db.PERSONAL_INFORMATION.Where(a => a.NIN == users.NIN).Select(a => a.EmpID).FirstOrDefault();

                var email = db.PHYSICAL_ADDRESSES.Where(a => a.EmpID == empid).Select(a => a.Email).FirstOrDefault();
                var pass = db.PERSONAL_INFORMATION.Where(a => a.EmpID == empid).Select(a => a.Passcode).FirstOrDefault();
                

                if (NIN == null)
                {
                    TempData["NINERROR"] = "NIN NOT FOUND";
                }
                else if (email == null)
                {
                    TempData["EMAILERROR"] = "EMAIL NOT FOUND";
                }
                else if (FileNo == null)
                {
                    TempData["FILENOERROR"] = "FILENO NOT FOUND";
                }
                else
                {
                   if(empid == userninid)
                    {
                        Session["userid"] = Convert.ToInt32(empid);
                        var passcode = Generate_Pass();

                        if (empid > 0 && email != null && NIN != null)
                        {
                            try
                            {
                                pass = passcode;

                                int res = db.Database.ExecuteSqlCommand("Update PERSONAL_INFORMATION  set Passcode = '" + pass + "' where EmpID = '" + empid + "'");

                                db.SaveChanges();

                                SendEmailVerificationLink(email, passcode);

                                TempData["verificationlink"] = "check email for verification code.";

                            }
                            catch (Exception e)
                            {
                                TempData["mailerror"] = e.Message;
                            }
                            return RedirectToAction("verify");
                        }
                        else
                        {
                            TempData["empid_email"] = "invalid email address";
                        }
                    }
                    else
                    {
                        TempData["idmissmatch"] = "USER NOT FOUND PLEASE CHECK FILENO OR NIN ";
                    }
                }
            }catch(Exception e)
            {
                TempData["usererror"] = e.Message;
            }

            return View(users);
           

        }

        public ActionResult Report()
        {
            int id = Convert.ToInt32(Session["userid"]);
            var user = db.PERSONAL_INFORMATION.Where(a => a.EmpID.Equals(id)).ToList();
           
            return View(user);
        }

        public void SendEmailVerificationLink(string emailId,string passcode)
        {
            
            //var scheme = Request.Url.Scheme;
            //var host = Request.Url.Host;
            //var port = Request.Url.Port;    
            //var verifyUrl = "/Users/" + emailFor + "/" + activationCode;
            //var link = verifyUrl;
            var fromEmail = new MailAddress("testmarvinug@gmail.com", "test asp");
            var toEmail = new MailAddress(emailId);
            //this password is generated by u in ur email account
            var fromEmailPassword = "kcywjucbmujbrycc";

            var subject = "ACCOUNT TESTING";
            var body = "TESING TESTING  " + passcode;/* + "<br /> <a href='" + link + "'> click here</a>";*/
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,

                Credentials = new System.Net.NetworkCredential(fromEmail.Address, fromEmailPassword),

            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            })
                try
                {
                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    TempData["emailerror"] = ex.Message;
                }
        }

        [HttpGet]
        public ActionResult verify()
        {
            return View();
        }
        [HttpPost]
        
        public ActionResult verify(PERSONAL_INFORMATION passcodes)
        {
            
         int userid = Convert.ToInt32(Session["userid"]);

            try
            {
                var Passcode = db.PERSONAL_INFORMATION.Where(a => a.EmpID.Equals(userid)).Select(a => a.Passcode).FirstOrDefault();
                if (Passcode == passcodes.Passcode)
                {
                    TempData["success"] = "verified";

                    return RedirectToAction("userdetails");
                }
                else
                {
                    TempData["error"] = "invalid code";
                }
            }catch(Exception e)
            {
                TempData["error"] = e.Message;
            }
           

            return View(passcodes);
        }

        DataSet1 ds = new DataSet1();
        public ActionResult viewDocument()
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"documents\profile.rdlc";
            //reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet", ds.PERSONAL_INFORMATION[0]));
            ViewBag.ReportViewer = reportViewer;

            return View();
        }      
        public ActionResult userdetails()
        {
            var personalinfo = Get_personal_info();
            var homeaddress = Get_HomeAddress();
            var employmentdetails = Get_employmentdetails();
            var education = Get_educationQualifications();
            var dependant = Get_dependant();
            var physicaladdress = Get_physicalAddress();
            var physicalfeatures = Get_physicalfeatures();
            var award = Get_Awards();
            var Accomodations = accomodation();
            var language = Get_Language();
            var health = Get_health();
            var getparents = Get_parents();
            var welfare = Paywelfare();
            var nextOfKin1 = NOK1();
            var nextOfKin2 = NOK2();
            var courses = other_related_courses();
            var familyinfos = familyinfo();

            var userDetails = new userdetails();

            userDetails.PERSONAL_INFORMATION = personalinfo;
            userDetails.HOME_ADDRESS = homeaddress;
            userDetails.DETAILS_OF_EMPLOYMENT = employmentdetails;
            userDetails.EDUCATION_AND_QUALIFICATIONS_Old = education;
            userDetails.DEPENDANT = dependant;
            userDetails.PHYSICAL_ADDRESSES = physicaladdress;
            userDetails.PHYSICAL_FEATURES = physicalfeatures;
            userDetails.AWARDS_AND_COMMENDATIONS = award;
            userDetails.Accomodation = Accomodations;
            userDetails.LANGUAGES_PROFFECIENCY = language;
            userDetails.HEALTH = health;
            userDetails.PARENT = getparents;
            userDetails.PAY_AND_WELFARE = welfare;
            userDetails.NEXT_OF_KIN1 = nextOfKin1;
            userDetails.NEXT_OF_KIN2 = nextOfKin2;
            userDetails.OTHER_RELATED_COURSES_ATTENDED = courses;
            userDetails.FAMILY_INFORMATION = familyinfos;
            //userDetails.FAMILY_INFORMATION = db.FAMILY_INFORMATION.Where(a => a.EmpID == id).ToList();

            return View(userDetails);
        }

        public List<PERSONAL_INFORMATION> Get_personal_info()
        {
            int id = Convert.ToInt32(Session["userid"]);
            try
            {
               
             db.PERSONAL_INFORMATION.Where(a => a.EmpID == id).ToList();
               
            }
            catch(Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.PERSONAL_INFORMATION.Where(a => a.EmpID == id).ToList());

           
        }
        public List<HOME_ADDRESS> Get_HomeAddress()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.HOME_ADDRESS.Where(a => a.EmpID == id).ToList());
        }
        public List<DETAILS_OF_EMPLOYMENT> Get_employmentdetails()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.DETAILS_OF_EMPLOYMENT.Where(a => a.EmpID == id).ToList());
        }
        public List<EDUCATION_AND_QUALIFICATIONS_Old> Get_educationQualifications()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.EDUCATION_AND_QUALIFICATIONS_Old.Where(a => a.EmpID == id).ToList());
        }
        public List<AWARDS_AND_COMMENDATIONS> Get_Awards()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.AWARDS_AND_COMMENDATIONS.Where(a => a.EmpID == id).ToList());
        }
        public List<PHYSICAL_ADDRESSES> Get_physicalAddress()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.PHYSICAL_ADDRESSES.Where(a => a.EmpID == id).ToList());
        }
        public List<PHYSICAL_FEATURES> Get_physicalfeatures()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.PHYSICAL_FEATURES.Where(a => a.EmpID == id).ToList());
        }
        public List<DEPENDANT> Get_dependant()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.DEPENDANTS.Where(a => a.EmpID == id).ToList());
        }
        public List<ACCOMMODATION> accomodation()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.ACCOMMODATIONs.Where(a => a.EmpID == id).ToList());
        }
        public List<LANGUAGES_PROFFECIENCY> Get_Language()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.LANGUAGES_PROFFECIENCY.Where(a => a.EmpID == id).ToList());
        }
        public List<HEALTH> Get_health()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.HEALTHs.Where(a => a.EmpID == id).ToList());
        }
        public List<PARENT> Get_parents()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.PARENTS.Where(a => a.EmpID == id).ToList());
        }
        public List<PAY_AND_WELFARE> Paywelfare()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.PAY_AND_WELFARE.Where(a => a.EmpID == id).ToList());
        }
        public List<NEXT_OF_KIN1> NOK1()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.NEXT_OF_KIN1.Where(a => a.EmpID == id).ToList());
        }
        public List<NEXT_OF_KIN2> NOK2()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.NEXT_OF_KIN2.Where(a => a.EmpID == id).ToList());
        }
        public List<OTHER_RELATED_COURSES_ATTENDED> other_related_courses()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.OTHER_RELATED_COURSES_ATTENDED.Where(a => a.EmpID == id).ToList());
        }
        public List<FAMILY_INFORMATION> familyinfo()
        {
            int id = Convert.ToInt32(Session["userid"]);
            return (db.FAMILY_INFORMATION.Where(a => a.EmpID == id).ToList());
        }
































        [NonAction]
        public string  Generate_Pass( )
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string finalString = new String(stringChars);

            return finalString;
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
