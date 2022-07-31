using HRMIS_PERSONEL_PROFILE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HRMIS_PERSONEL_PROFILE.viewmodel;
namespace HRMIS_PERSONEL_PROFILE.viewmodel
{
    public class userdetails
    {
        public List<ACCOMMODATION> Accomodation { get; set; }
        public List<AWARDS_AND_COMMENDATIONS> AWARDS_AND_COMMENDATIONS { set; get; }
        public List<DEPENDANT> DEPENDANT { set; get; }
        public List<EDUCATION_AND_QUALIFICATIONS_Old> EDUCATION_AND_QUALIFICATIONS_Old { set; get; }
        public List<DETAILS_OF_EMPLOYMENT> DETAILS_OF_EMPLOYMENT { set; get; }
        public List<FAMILY_INFORMATION> FAMILY_INFORMATION { set; get; }
        public List<HEALTH> HEALTH { set; get; }
        public List<HOME_ADDRESS> HOME_ADDRESS { set; get; }
        public List<NEXT_OF_KIN1> NEXT_OF_KIN1 { get; set; }
        public List<NEXT_OF_KIN2> NEXT_OF_KIN2 { set; get; }
        public List<OTHER_RELATED_COURSES_ATTENDED> OTHER_RELATED_COURSES_ATTENDED { set; get; }
        public List<PARENT> PARENT { set; get; }
        public List<PAY_AND_WELFARE> PAY_AND_WELFARE { set; get; }
        public List<PHYSICAL_ADDRESSES> PHYSICAL_ADDRESSES { set; get; }
        public List<PHYSICAL_FEATURES> PHYSICAL_FEATURES { set; get; }
        public List<PERSONAL_INFORMATION> PERSONAL_INFORMATION { set; get; }
        public List<LANGUAGES_PROFFECIENCY> LANGUAGES_PROFFECIENCY { set; get; }


    }
}