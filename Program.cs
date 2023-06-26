using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using Email.Models;
using Email.DataContext;

namespace Task
{
    class Program
    {
        static void Main(string[] args)
        {
            bttn_Send_Click();
            string path = @"C:\Users\krpat\OneDrive\Documents\GitHub\Email\Task.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine($"{DateTime.Now.ToString()} - This is running from the task");
            }
        }

        static void bttn_Send_Click()
        {
            DateTime todaydate = DateTime.Now;
            int currentmonth = DateTime.Now.Month;
            DateTime lastDayOfMonth = new DateTime(DateTime.Now.Year, currentmonth, DateTime.DaysInMonth(DateTime.Now.Year, currentmonth));

            List<SendEmail> sendEmails = new List<SendEmail>(); // Create a list to store SendEmail objects

            using (var result = new ApplicationDbContext())
            {
                var users = result.MIS_Users.ToList(); // Retrieve data from MIS_Users

                foreach (var entry in users)
                {
                    sendEmails.Add(new SendEmail() // Create a new SendEmail object and add it to the list
                    {
                        UserId = entry.UserId,
                        Email = entry.Email
                    });

                    // Rest of your code...
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                        mail.To.Add(new MailAddress(entry.Email));
                        mail.Subject = "Reminder";
                        string message = "This is a Reminder message!";
                        mail.Body = "<h1>Reminder</h1>"; /*+ message /*+ entry.Appreciation.Appericiation*//* + entry.Employee.EmpName;*/
                        mail.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential("noreplyehstesting78@gmail.com", "cntpraqgsaqjlxpi");
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }

                    }
                }
            }
        }

        class SendEmail
        {
            public int UserId { get; set; }
            public string Email { get; set; }
        }
    }
}
