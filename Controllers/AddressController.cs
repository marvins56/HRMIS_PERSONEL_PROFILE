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
                //force id
                var forcenoid = db.PERSONAL_INFORMATION.Where(a=>a.ForceNo == users.FileNo).Select(a=>a.EmpID).FirstOrDefault();
                //empid
                var empid = db.PERSONAL_INFORMATION.Where(a => a.FileNo == users.FileNo).Select(a => a.EmpID).FirstOrDefault();
                var FileNo = db.PERSONAL_INFORMATION.Where(a => a.FileNo == users.FileNo && a.EmpID == empid).Select(a=>a.FileNo).FirstOrDefault();
                //fileno form force id
                var Filenoforceid = db.PERSONAL_INFORMATION.Where(a => a.ForceNo == users.FileNo && a.EmpID == forcenoid).FirstOrDefault();
                
                if(empid > 0)
                {

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
                    else if (FileNo == null )
                    {
                        TempData["FILENOERROR"] = "FILENO NOT FOUND";
                    }
                    else
                    {
                        if (empid == userninid)
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


                }
                else if(forcenoid > 0 )
                {


                    var NIN = db.PERSONAL_INFORMATION.Where(a => a.EmpID == forcenoid).Select(a => a.NIN).FirstOrDefault();
                    var userninid = db.PERSONAL_INFORMATION.Where(a => a.NIN == users.NIN).Select(a => a.EmpID).FirstOrDefault();

                    var email = db.PHYSICAL_ADDRESSES.Where(a => a.EmpID == forcenoid).Select(a => a.Email).FirstOrDefault();
                    var pass = db.PERSONAL_INFORMATION.Where(a => a.EmpID == forcenoid).Select(a => a.Passcode).FirstOrDefault();


                    if (NIN == null)
                    {
                        TempData["NINERROR"] = "NIN NOT FOUND";
                    }
                    else if (email == null)
                    {
                        TempData["EMAILERROR"] = "EMAIL NOT FOUND";
                    }
                    else if ( Filenoforceid == null)
                    {
                        TempData["FILENOERROR"] = "FILENO NOT FOUND";
                    }
                    else
                    {
                        if (forcenoid == userninid)
                        {
                            Session["userid1"] = Convert.ToInt32(forcenoid);
                            var passcode = Generate_Pass();

                            if (forcenoid > 0 && email != null && NIN != null)
                            {
                                try
                                {
                                    pass = passcode;

                                    int res = db.Database.ExecuteSqlCommand("Update PERSONAL_INFORMATION  set Passcode = '" + pass + "' where EmpID = '" + forcenoid + "'");

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
                }
                
                else
                {
                    TempData["errorid"] = "INVALID FILE / FORCE NUMBER ENTERED";
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
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                var Passcode = db.PERSONAL_INFORMATION.Where(a => a.EmpID == id).Select(a => a.Passcode).FirstOrDefault();
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
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;
                 
            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                
                //picking data from diffrent tables using ids (inner join
                //RANk
                var rankid = db.PERSONAL_INFORMATION.Where(a => a.EmpID == id).Select(a => a.RankID).FirstOrDefault();
                int id2 = Convert.ToInt32(rankid);             
                var rankname = db.Ranks.Where(a => a.RankID == id).Select(a => a.Rank1).FirstOrDefault();
                TempData["rankname"] = rankname;
                //NATIONALITY
                var Nationalityid = db.PERSONAL_INFORMATION.Where(a => a.EmpID == id).Select(a => a.NationalityID).FirstOrDefault();
                int id3 = Convert.ToInt32(Nationalityid);
              
             var nationalname = db.Nationalities.Where(a => a.NationalityID.Equals(id3)).Select(a => a.Nationality1).FirstOrDefault(); ;
                TempData["Nationalityname"] = nationalname;

                TempData["PERSONAL_INFORMATION"] = db.PERSONAL_INFORMATION.Where(a => a.EmpID ==id ).ToList().Count();
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
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                //home
                var district = db.HOME_ADDRESS.Where(a => a.EmpID == id).Select(a => a.DistrictID).FirstOrDefault();
                int id13 = Convert.ToInt32(district);
                var disctrictnames = db.Districts.Where(a => a.DistrictID.Equals(id13)).Select(a => a.District1).FirstOrDefault();
                TempData["disctrictnames"] = disctrictnames;

                TempData["HOME_ADDRESS"] = db.HOME_ADDRESS.Where(a => a.EmpID == id).ToList().Count();
                db.HOME_ADDRESS.Where(a => a.EmpID == id).ToList();
            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.HOME_ADDRESS.Where(a => a.EmpID == id).ToList());
        }
        public List<DETAILS_OF_EMPLOYMENT> Get_employmentdetails()
        {
            
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                TempData["DETAILS_OF_EMPLOYMENT"] = db.DETAILS_OF_EMPLOYMENT.Where(a => a.EmpID == id).ToList().Count();
                db.DETAILS_OF_EMPLOYMENT.Where(a => a.EmpID == id).ToList();

            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.DETAILS_OF_EMPLOYMENT.Where(a => a.EmpID == id).ToList());
        }
        public List<EDUCATION_AND_QUALIFICATIONS_Old> Get_educationQualifications()
        {
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                TempData["EDUCATION_AND_QUALIFICATIONS_Old"] = db.EDUCATION_AND_QUALIFICATIONS_Old.Where(a => a.EmpID == id).ToList().Count();
                db.EDUCATION_AND_QUALIFICATIONS_Old.Where(a => a.EmpID == id).ToList();

            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.EDUCATION_AND_QUALIFICATIONS_Old.Where(a => a.EmpID == id).ToList());
        }
        public List<AWARDS_AND_COMMENDATIONS> Get_Awards()
        {
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                TempData["AWARDS_AND_COMMENDATIONS"] = db.AWARDS_AND_COMMENDATIONS.Where(a => a.EmpID == id).ToList().Count();
                db.AWARDS_AND_COMMENDATIONS.Where(a => a.EmpID == id).ToList();

            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.AWARDS_AND_COMMENDATIONS.Where(a => a.EmpID == id).ToList());
        }
        public List<PHYSICAL_ADDRESSES> Get_physicalAddress()
        {
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                //picking data from diffrent tables using ids (inner join
                //DISTRICT
                var districtid = db.PHYSICAL_ADDRESSES.Where(a => a.EmpID == id).Select(a => a.DistrictID).FirstOrDefault();

                int id4 = Convert.ToInt32(districtid);

                var districtname = db.Districts.Where(a => a.DistrictID.Equals(id4)).Select(a => a.District1).FirstOrDefault();
                TempData["districtname"] = districtname;



                TempData["PHYSICAL_ADDRESSES"] = db.PHYSICAL_ADDRESSES.Where(a => a.EmpID == id).ToList().Count();
                db.PHYSICAL_ADDRESSES.Where(a => a.EmpID == id).ToList();

            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.PHYSICAL_ADDRESSES.Where(a => a.EmpID == id).ToList());
        }
        public List<PHYSICAL_FEATURES> Get_physicalfeatures()
        {
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                TempData["PHYSICAL_FEATURES"] = db.PHYSICAL_FEATURES.Where(a => a.EmpID == id).ToList().Count();
                db.PHYSICAL_FEATURES.Where(a => a.EmpID == id).ToList();

            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.PHYSICAL_FEATURES.Where(a => a.EmpID == id).ToList());
        }
        public List<DEPENDANT> Get_dependant()
        {
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                TempData["DEPENDANTS"] = db.DEPENDANTS.Where(a => a.EmpID == id).ToList().Count();
                db.DEPENDANTS.Where(a => a.EmpID == id).ToList();

            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.DEPENDANTS.Where(a => a.EmpID == id).ToList());
        }
        public List<ACCOMMODATION> accomodation()
        {
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                TempData["ACCOMMODATIONs"] = db.ACCOMMODATIONs.Where(a => a.EmpID == id).ToList().Count();
                db.ACCOMMODATIONs.Where(a => a.EmpID == id).ToList();

            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.ACCOMMODATIONs.Where(a => a.EmpID == id).ToList());
        }
        public List<LANGUAGES_PROFFECIENCY> Get_Language()
        {
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                //language
                var langid = db.LANGUAGES_PROFFECIENCY.Where(a => a.EmpID == id).Select(a => a.LangID).FirstOrDefault();
                int id12 = Convert.ToInt32(langid);
                var langname = db.Languages.Where(a => a.LangID.Equals(id12)).Select(a => a.Language1).FirstOrDefault();
                ViewBag.languagename = langname;
                //var result = db.Database.ExecuteSqlCommand("SELECT  Languages.Language  FROM LANGUAGES_PROFFECIENCY INNER JOIN Languages ON LANGUAGES_PROFFECIENCY.LangID = Languages.LangID where EmpID ='"+id+"' ");
                //TempData["result"] = result;
                TempData["LANGUAGES_PROFFECIENCY"] = db.LANGUAGES_PROFFECIENCY.Where(a => a.EmpID == id).ToList().Count();
                db.LANGUAGES_PROFFECIENCY.Where(a => a.EmpID == id).ToList();
            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.LANGUAGES_PROFFECIENCY.Where(a => a.EmpID == id).ToList());
        }
        public List<HEALTH> Get_health()
        {
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                TempData["HEALTHs"] = db.HEALTHs.Where(a => a.EmpID == id).ToList().Count();
                db.HEALTHs.Where(a => a.EmpID == id).ToList();
            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.HEALTHs.Where(a => a.EmpID == id).ToList());
        }
        public List<PARENT> Get_parents()
        {
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                TempData["PARENTS"] = db.PARENTS.Where(a => a.EmpID == id).ToList().Count();
                db.PARENTS.Where(a => a.EmpID == id).ToList();
            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.PARENTS.Where(a => a.EmpID == id).ToList());
        }
        public List<PAY_AND_WELFARE> Paywelfare()
        {
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            { //picking data from diffrent tables using ids (inner join
                //salaryscale
                var bankid = db.PAY_AND_WELFARE.Where(a => a.EmpID == id).Select(a => a.BankID).FirstOrDefault();
                int id5 = Convert.ToInt32(bankid);
                var bankname = db.Banks.Where(a => a.BankID.Equals(id5)).Select(a => a.Bank1).FirstOrDefault();
                TempData["bankname"] = bankname;

                //salaryscale
                var salaryid = db.PAY_AND_WELFARE.Where(a => a.EmpID == id).Select(a => a.SalaryScaleID).FirstOrDefault();
                int id6 = Convert.ToInt32(salaryid);
                var salaryscale = db.SalaryScales.Where(a => a.SalaryScaleID.Equals(id6)).Select(a => a.Scale).FirstOrDefault();
                TempData["salaryscale"] = salaryscale;

                //account type
                var accounttypeid = db.PAY_AND_WELFARE.Where(a => a.EmpID == id).Select(a => a.AccountTypeID).FirstOrDefault();
                int id7 = Convert.ToInt32(accounttypeid);
                var accounttypename = db.AccountTypes.Where(a => a.AccountTypeID.Equals(id7)).Select(a => a.AccountType1).FirstOrDefault();
                TempData["accounttypename"] = accounttypename;

                //account type
                var branchid = db.PAY_AND_WELFARE.Where(a => a.EmpID == id).Select(a => a.BranchID).FirstOrDefault();
                int id8 = Convert.ToInt32(branchid);
                var branchname = db.BankBranches.Where(a => a.BranchID.Equals(id8)).Select(a => a.Branch).FirstOrDefault();
                TempData["branchname"] = branchname;


                TempData["PAY_AND_WELFARE"] = db.PAY_AND_WELFARE.Where(a => a.EmpID == id).ToList().Count();
                db.PAY_AND_WELFARE.Where(a => a.EmpID == id).ToList();
            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.PAY_AND_WELFARE.Where(a => a.EmpID == id).ToList());
        }
        public List<NEXT_OF_KIN1> NOK1()
        {
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                //DISTRICT
                var districtid = db.NEXT_OF_KIN1.Where(a => a.EmpID == id).Select(a => a.DistrictID).FirstOrDefault();

                int id10 = Convert.ToInt32(districtid);

                var districtname2 = db.Districts.Where(a => a.DistrictID.Equals(id10)).Select(a => a.District1).FirstOrDefault();
                TempData["districtname2"] = districtname2;
                TempData["NEXT_OF_KIN1"] = db.NEXT_OF_KIN1.Where(a => a.EmpID == id).ToList().Count();
                db.NEXT_OF_KIN1.Where(a => a.EmpID == id).ToList();
            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.NEXT_OF_KIN1.Where(a => a.EmpID == id).ToList());
        }
        public List<NEXT_OF_KIN2> NOK2()
        {

            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                //DISTRICT
                var districtid = db.NEXT_OF_KIN2.Where(a => a.EmpID == id).Select(a => a.DistrictID).FirstOrDefault();

                int id11 = Convert.ToInt32(districtid);

                var districtname3 = db.Districts.Where(a => a.DistrictID.Equals(id11)).Select(a => a.District1).FirstOrDefault();
                TempData["districtname3"] = districtname3;
                TempData["NEXT_OF_KIN2"] = db.NEXT_OF_KIN2.Where(a => a.EmpID == id).ToList().Count();
                db.NEXT_OF_KIN2.Where(a => a.EmpID == id).ToList();
            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.NEXT_OF_KIN2.Where(a => a.EmpID == id).ToList());
        }
        public List<OTHER_RELATED_COURSES_ATTENDED> other_related_courses()
        {
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                TempData["OTHER_RELATED_COURSES_ATTENDED"] = db.OTHER_RELATED_COURSES_ATTENDED.Where(a => a.EmpID == id).ToList().Count();
                db.OTHER_RELATED_COURSES_ATTENDED.Where(a => a.EmpID == id).ToList();
            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

            return (db.OTHER_RELATED_COURSES_ATTENDED.Where(a => a.EmpID == id).ToList());
        }
        public List<FAMILY_INFORMATION> familyinfo()
        {
            int idforceno = Convert.ToInt32(Session["userid1"]);
            int idfilrno = Convert.ToInt32(Session["userid"]);
            if (Session["userid1"] != null)
            {
                Session["id"] = idforceno;

            }
            else
            {
                Session["id"] = idfilrno;
            }

            int id = Convert.ToInt32(Session["id"]);
            try
            {
                TempData["FAMILY_INFORMATION"] = db.FAMILY_INFORMATION.Where(a => a.EmpID == id).ToList().Count();
                db.FAMILY_INFORMATION.Where(a => a.EmpID == id).ToList();
            }
            catch (Exception e)
            {
                TempData["databaseerror"] = e.Message;
            }

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
