using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ConsolidatedPlatformForRecruitmentAgencies.DAL;
using ConsolidatedPlatformForRecruitmentAgencies.Models;
using NLog;

namespace ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection
{
    public class ApplicantImplementation : IApplicant
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        private readonly RecruitmentContext _recruitmentContext;
        private readonly System.Web.HttpContext _httpContext = System.Web.HttpContext.Current;
        public ApplicantImplementation(RecruitmentContext recruitmentContext)
        {
            _recruitmentContext = recruitmentContext;
        }
        public void Register(Applicant applicant)
        {
           _recruitmentContext.Applicants.Add(applicant);
           _recruitmentContext.SaveChanges();
        }

        public List<Gender> ListOfGender()
        {
            var listofgender = _recruitmentContext.Genders.ToList();
            return listofgender;
        }

        public List<Country> ListOfCountry()
        {
            var listofcountry = _recruitmentContext.Countries.ToList();
            return listofcountry;
        }

        public List<State> ListOfState()
        {
            var listofstate = _recruitmentContext.States.ToList();
            return listofstate;
        }



        public List<State> GetStateByCountry(int id)
        {
            var getstatebycountryid = _recruitmentContext.States.Where(s => s.CountryId == id).ToList();
            return getstatebycountryid;
        }


        public void SendEmail(int id)
        {
            Applicant applicant = _recruitmentContext.Applicants.Where(a => a.ApplicantId == id).SingleOrDefault();

            string SenderEmail = "connectRecruiters@outlook.com";
            string RecieverEmail = applicant.Email;
            var password = ConfigurationManager.AppSettings["password"];
            string morning = "Good Morning";
            string afternoon = "Good Afternoon";
            string evening = "Good Evening";
            string goodday = "Good Day";
            string subject = "Congratulatons: Registration was Successful....";
            var getPresentTime = (DateTime.Now.Hour <= 12) && !(DateTime.Now.Hour > 12)
                ? morning : (DateTime.Now.Hour > 12) && (DateTime.Now.Hour <= 16)
                ? afternoon : (DateTime.Now.Hour > 16) ? evening : goodday;
            string body = "<div class='row' background-color= 'blue' ;font-family='Trebuchet MS'>" 
                +"<h2 style='text-align:center'>Registration Successful</h3>"+"<br />" 
                + getPresentTime+"," + " " + "<b>"+applicant.LastName+"</b>" + "<br />"+
                "Your Login Details are Provided Below " + "<br />" +
                "Registration Number : " + applicant.UserName + "<br />"+
                "Password : " + applicant.Password +
                "<br />" +
                "<a href ='http://localhost:49851/Applicant/Login'>Please Click Here To Login</a>"
                + "</div>";
            var sender = new MailAddress(SenderEmail);
            var reciever = new MailAddress(RecieverEmail);
            var smtp = new SmtpClient
            {
                Host = "smtp-mail.outlook.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sender.Address, password)
            };
            using (var message = new MailMessage(sender, reciever)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml=true
            })

            {
                try
                {
                    smtp.Send(message);
                    applicant.EmailSent = true;
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    for (int i = 0; i < ex.InnerExceptions.Length; i++)
                    {
                        SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                        if (status == SmtpStatusCode.MailboxBusy ||
                            status == SmtpStatusCode.MailboxUnavailable)
                        {

                            logger.Error("An Error Occurred", ex);
                            System.Threading.Thread.Sleep(5000);
                            
                            smtp.Send(message);
                        }
                        else
                        {
                           logger.Error("An Error Occurred", ex.InnerExceptions[i].FailedRecipient);
                        }
                    }
                }
            }
        }

        public string GenerateRegNo()
        {
            string registrationNo = "";
            var random = new Random();
            registrationNo = "APP" + random.Next(100000, 800000);
            return registrationNo;
        }
    }
}