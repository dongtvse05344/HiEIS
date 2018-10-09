using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace HiEIS.Businesses
{
    public class EmailBusiness
    {
        public string ToHtml(string viewToRender, ViewDataDictionary viewData, ControllerContext controllerContext)
        {
            var result = ViewEngines.Engines.FindView(controllerContext, viewToRender, null);

            StringWriter output;
            using (output = new StringWriter())
            {
                var viewContext = new ViewContext(controllerContext, result.View, viewData, controllerContext.Controller.TempData, output);
                result.View.Render(viewContext, output);
                result.ViewEngine.ReleaseView(controllerContext, result.View);
            }

            return output.ToString();
        }

        public bool SendEmail(string receiver, string subject, string content)
        {
            bool result = false;

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com"); //Server mail

            mail.From = new MailAddress("hieis.automailer@gmail.com"); //Mail gửi
            mail.To.Add(receiver); //Mail nhận
            mail.IsBodyHtml = true; //Cho phép đọc HTML

            mail.Subject = subject;
            mail.Body = content;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("hieis.automailer@gmail.com", "cgxdemculgzfrror"); //Mail gửi + password
            SmtpServer.EnableSsl = true;

            try
            {
                SmtpServer.Send(mail);
                result = true;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}