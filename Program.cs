using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using Email.Models;
using Email.DataContext;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Globalization;

namespace Task
{
    class Program
    {
        static void Main(string[] args)
        {
            //Report_submitted_01();
            //Report_submitted_02();
            //Report_reviewed_02();
            //ReportReviewed_03();
            //Report_approved_04();
            //Report_Reminder_05();
            //Report_Reminder_06();
            //Report_Reminder_07();
            //Report_pending_08();
            //Report_Reminder_09();
            Report_autoapprove_10();
            //Report_approved_11();
            string path = @"C:\Users\krpat\OneDrive\Documents\GitHub\Email\Task.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine($"{DateTime.Now.ToString()} - This is running from the task");
            }
        }

        static void Report_submitted_01()
        {
            using (var result = new ApplicationDbContext())
            {
                var reports = result.MIS_MISReport
                    .Where(r => r.StatusId == 1)
                    .Join(
                        result.MIS_VerticalMaster,
                        report => report.VerticalId,
                        vertical => vertical.VerticalId,
                        (report, vertical) => new { Report = report, Vertical = vertical }
                    )
                    .Join(
                        result.MIS_ProjectMaster,
                        reportVertical => reportVertical.Report.ProjectId,
                        project => project.ProjectId,
                        (reportVertical, project) => new { ReportVertical = reportVertical, Project = project }
                    )
                    .ToList();

                var users = result.MIS_Users.ToList();

                if (reports.Count > 0)
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("noreplyehstesting78@gmail.com");

                        var emailRecipients = new List<string>();
                        var mailBody = new StringBuilder();

                        mail.Subject = "Submitted Reports";

                        // Constructing the mail body
                        mailBody.AppendLine("<h5>Dear Sir/Madam,</h5>");
                        mailBody.AppendLine("<br/>");
                        mailBody.AppendLine("<p>Greetings!</p>");
                        mailBody.AppendLine("<br/>");
                        mailBody.AppendLine("<p>The following reports have been submitted by you:</p>");
                        mailBody.AppendLine("<ul>");

                        foreach (var reportData in reports)
                        {
                            var report = reportData.ReportVertical.Report;
                            var vertical = reportData.ReportVertical.Vertical;
                            var project = reportData.Project;

                            var user = users.FirstOrDefault(u => u.UserId == report.EntryBy);

                            if (user != null && user.RoleId == 1)
                            {
                                emailRecipients.Add(user.Email);
                                mailBody.AppendLine($"<li>Report for the project '{project.ProjectName}' in the vertical '{vertical.VerticalName}'</li>");
                            }
                        }

                        mailBody.AppendLine("</ul>");
                        mailBody.AppendLine("<br/>");
                        mailBody.AppendLine("<p>Thank you.</p>");
                        mailBody.AppendLine("<br/>");

                        mail.Body = mailBody.ToString();
                        mail.IsBodyHtml = true;

                        foreach (string recipient in emailRecipients)
                        {
                            mail.To.Add(new MailAddress(recipient));
                        }

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




        static void Report_submitted_02()
        {
            using (var result = new ApplicationDbContext())
            {
                var reports = result.MIS_MISReport
                    .Where(r => r.StatusId == 1)
                    .Join(
                        result.MIS_VerticalMaster,
                        report => report.VerticalId,
                        vertical => vertical.VerticalId,
                        (report, vertical) => new { Report = report, Vertical = vertical }
                    )
                    .Join(
                        result.MIS_ProjectMaster,
                        reportVertical => reportVertical.Report.ProjectId,
                        project => project.ProjectId,
                        (reportVertical, project) => new { ReportVertical = reportVertical, Project = project }
                    )
                    .ToList();

                if (reports.Count > 0)
                {
                    var users = result.MIS_Users.Where(u => u.RoleId == 2).ToList();

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                        mail.Subject = "Reports submitted for review";

                        // Constructing the mail body
                        var mailBody = new StringBuilder();
                        mailBody.AppendLine("<h5>Dear Sir/Madam,</h5>");
                        mailBody.AppendLine("<br/>");
                        mailBody.AppendLine("<p>Greetings!</p>");
                        mailBody.AppendLine("<br/>");
                        mailBody.AppendLine("<p>The following reports have been submitted for review:</p>");
                        mailBody.AppendLine("<ul>");

                        foreach (var reportData in reports)
                        {
                            var report = reportData.ReportVertical.Report;
                            var vertical = reportData.ReportVertical.Vertical;
                            var project = reportData.Project;

                            mailBody.AppendLine($"<li>Report for the project '{project.ProjectName}' in the vertical '{vertical.VerticalName}'</li>");
                        }

                        mailBody.AppendLine("</ul>");
                        mailBody.AppendLine("<br/>");
                        mailBody.AppendLine("<p>Thank you for your attention.</p>");
                        mailBody.AppendLine("<br/>");

                        mail.Body = mailBody.ToString();
                        mail.IsBodyHtml = true;

                        foreach (var user in users)
                        {
                            mail.To.Add(new MailAddress(user.Email));
                        }

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



        static void Report_reviewed_02()
        {
            using (var result = new ApplicationDbContext())
            {
                var reports = result.MIS_MISReport.Where(r => r.StatusId == 3)
                    .Join(
                        result.MIS_VerticalMaster,
                        report => report.VerticalId,
                        vertical => vertical.VerticalId,
                        (report, vertical) => new { Report = report, Vertical = vertical }
                    )
                    .Join(
                        result.MIS_ProjectMaster,
                        reportVertical => reportVertical.Report.ProjectId,
                        project => project.ProjectId,
                        (reportVertical, project) => new { ReportVertical = reportVertical, Project = project }
                    ).ToList();

                if (reports.Count > 0)
                {
                    var users = result.MIS_Users.Where(u => u.RoleId == 1).ToList();

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                        mail.To.Add(new MailAddress("hoteam8460@gmail.com"));
                        mail.Subject = "Reports reviewed and awaiting approval";

                        // Constructing the mail body
                        var mailBody = new StringBuilder();
                        mailBody.AppendLine("Dear Sir/Madam,");
                        mailBody.AppendLine();
                        mailBody.AppendLine("I am writing to inform you that the following reports have been reviewed by the project manager and are awaiting your approval:");
                        mailBody.AppendLine();

                        foreach (var reportData in reports)
                        {
                            var report = reportData.ReportVertical.Report;
                            var vertical = reportData.ReportVertical.Vertical;
                            var project = reportData.Project;

                            mailBody.AppendLine($"- Report for the project '{project.ProjectName}' in the vertical '{vertical.VerticalName}'");
                        }

                        mailBody.AppendLine();
                        mailBody.AppendLine("Please review and approve these reports at your earliest convenience.");
                        mailBody.AppendLine();
                        mailBody.AppendLine("Thank you for your attention to this matter.");
                        mailBody.AppendLine();

                        mail.Body = mailBody.ToString();
                        mail.IsBodyHtml = false;

                        foreach (var user in users)
                        {
                            mail.CC.Add(new MailAddress(user.Email));
                        }

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

        static void ReportReviewed_03()
        {
            using (var result = new ApplicationDbContext())
            {
                var reports = result.MIS_MISReport.Where(r => r.StatusId == 2)
                    .Join(
                        result.MIS_VerticalMaster,
                        report => report.VerticalId,
                        vertical => vertical.VerticalId,
                        (report, vertical) => new { Report = report, Vertical = vertical }
                    )
                    .Join(
                        result.MIS_ProjectMaster,
                        reportVertical => reportVertical.Report.ProjectId,
                        project => project.ProjectId,
                        (reportVertical, project) => new { ReportVertical = reportVertical, Project = project }
                    ).ToList();

                if (reports.Count > 0)
                {
                    var users = result.MIS_Users.Where(u => u.RoleId == 3).ToList();

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                        mail.To.Add(new MailAddress("siteincharge01@gmail.com"));
                        mail.Subject = "Reports sent back for corrections";

                        // Constructing the mail body
                        var mailBody = new StringBuilder();
                        mailBody.AppendLine("<h5>Dear Sir/Madam,</h5>");
                        mailBody.AppendLine();
                        mailBody.AppendLine("This email is to inform you that the following reports have been reviewed by the EHS team at the Head Office, but corrections are required:");
                        mailBody.AppendLine();

                        foreach (var reportData in reports)
                        {
                            var report = reportData.ReportVertical.Report;
                            var vertical = reportData.ReportVertical.Vertical;
                            var project = reportData.Project;
                            mailBody.AppendLine("<br/>");
                            mailBody.AppendLine($"- Report for the project '{project.ProjectName}' in the vertical '{vertical.VerticalName}'");
                            mailBody.AppendLine("<br/>"); // Add a new line after each project and vertical
                        }
                        mailBody.AppendLine("<br/>");
                        mailBody.AppendLine();
                        mailBody.AppendLine("We kindly request you to review the identified corrections and make the necessary updates to ensure the accuracy and completeness of the reports.");
                        mailBody.AppendLine();
                        mailBody.AppendLine("Thank you for your attention to this matter.");
                        mailBody.AppendLine();
                        mailBody.AppendLine("Best regards,");
                        mailBody.AppendLine("Your Name"); // Replace 'Your Name' with your actual name or the sender's name

                        mail.Body = mailBody.ToString();
                        mail.IsBodyHtml = true;

                        foreach (var user in users)
                        {
                            mail.CC.Add(new MailAddress(user.Email));
                        }

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


        static void Report_approved_04()
        {
            using (var result = new ApplicationDbContext())
            {
                var reports = result.MIS_MISReport.Where(r => r.StatusId == 4)
                    .Join(
                        result.MIS_VerticalMaster,
                        report => report.VerticalId,
                        vertical => vertical.VerticalId,
                        (report, vertical) => new { Report = report, Vertical = vertical }
                    )
                    .Join(
                        result.MIS_ProjectMaster,
                        reportVertical => reportVertical.Report.ProjectId,
                        project => project.ProjectId,
                        (reportVertical, project) => new { ReportVertical = reportVertical, Project = project }
                    ).ToList();

                if (reports.Count > 0)
                {
                    var projectVerticalList = new StringBuilder();

                    foreach (var reportData in reports)
                    {
                        var project = reportData.Project.ProjectName;
                        var vertical = reportData.ReportVertical.Vertical.VerticalName;

                        projectVerticalList.AppendLine($"- Project: {project}, Vertical: {vertical}");
                    }

                    var users = result.MIS_Users.Where(u => u.RoleId == 2).ToList();

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                        mail.To.Add(new MailAddress("siteincharge01@gmail.com"));
                        mail.Subject = "Reports Approved";

                        // Constructing the mail body
                        var mailBody = new StringBuilder();
                        mailBody.AppendLine("<h5>Dear Team,</h5>");
                        mailBody.AppendLine("<p>The following reports have been approved:</p>");
                        mailBody.AppendLine();
                        mailBody.AppendLine(projectVerticalList.ToString());
                        mailBody.AppendLine();
                        mailBody.AppendLine("<p>Please review the reports and take any necessary actions accordingly.</p>");
                        mailBody.AppendLine("<p>Thank you for your attention.</p>");

                        mail.Body = mailBody.ToString();
                        mail.IsBodyHtml = true;

                        foreach (var user in users)
                        {
                            mail.CC.Add(new MailAddress(user.Email));
                        }

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

        static void Report_Reminder_05()
        {
            DateTime todayDate = DateTime.Now;
            int currentMonth = todayDate.Month;
            int nextMonth = currentMonth % 12 + 1; // Get the next month's number (wraps around to 1 after December)
            string nextMonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(nextMonth); // Get the name of the next month

            DateTime lastDayOfCurrentMonth = new DateTime(todayDate.Year, currentMonth, DateTime.DaysInMonth(todayDate.Year, currentMonth));

            if (todayDate.Date != lastDayOfCurrentMonth.Date)
            {
                using (var result = new ApplicationDbContext())
                {
                    var projectList = result.MIS_ProjectMaster
                        .Join(
                            result.MIS_VerticalMaster,
                            p => p.VerticalId,
                            v => v.VerticalId,
                            (p, v) => new { Project = p, Vertical = v }
                        )
                        .ToList();

                    var reportReminderList = new StringBuilder();

                    foreach (var project in projectList)
                    {
                        reportReminderList.AppendLine($"- Project: {project.Project.ProjectName}, Vertical: {project.Vertical.VerticalName}<br/>");
                    }

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                        mail.To.Add(new MailAddress("siteincharge01@gmail.com"));
                        mail.Subject = "Report Submission Reminder";
                        mail.Body = $"<h5>Kind Reminder: Please submit the following reports on or before the 4th of {nextMonthName}:</h5><br/>{reportReminderList.ToString()}";
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


        static void Report_Reminder_06()
        {
            DateTime todayDate = DateTime.Now;
            int currentMonth = todayDate.Month;
            DateTime currentDate = todayDate.Date;

            if (currentDate.Day >= 1 && currentDate.Day <= 4)
            {
                using (var result = new ApplicationDbContext())
                {
                    var existingProjectIds = result.MIS_MISReport
                        .Where(r => r.EntryDate.Month == currentMonth && r.EntryDate.Day >= 1 && r.EntryDate.Day <= 31) // Check for the current month's report from 1st to 4th
                        .Select(r => r.ProjectId)
                        .Distinct()
                        .ToList();

                    var missingProjectIds = Enumerable.Range(1, 8)
                        .Except(existingProjectIds)
                        .ToList();

                    var missingProjects = result.MIS_ProjectMaster
                        .Join(
                            result.MIS_VerticalMaster,
                            p => p.VerticalId,
                            v => v.VerticalId,
                            (p, v) => new { Project = p, Vertical = v }
                        )
                        .Where(pv => missingProjectIds.Contains(pv.Project.ProjectId))
                        .ToList();

                    var reportReminderList = new StringBuilder();

                    foreach (var project in missingProjects)
                    {
                        reportReminderList.AppendLine($"- Project: {project.Project.ProjectName}, Vertical: {project.Vertical.VerticalName}<br/>");
                    }

                    if (reportReminderList.Length > 0)
                    {
                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                            mail.To.Add(new MailAddress("siteincharge01@gmail.com"));
                            mail.Subject = "Report Submission Reminder";
                            mail.Body = $"<h5>Kind Reminder: The following reports has not been submitted yet.Please submit it on or before 4rth {DateTimeFormatInfo.CurrentInfo.GetMonthName(currentMonth)}</h5><br/>{reportReminderList.ToString()}";
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
        }


        static void Report_Reminder_07()
        {
            DateTime todayDate = DateTime.Now;
            int currentMonth = DateTime.Now.Month;
            DateTime currentDate = DateTime.Now.Date;

            if (currentDate.Day == 29)
            {
                using (var result = new ApplicationDbContext())
                {
                    var existingProjectIds = result.MIS_MISReport
                        .Where(r => r.EntryDate.Month == currentMonth && r.EntryDate.Day >= 1 && r.EntryDate.Day <= 31) // Check for the current month's report from 1st to 31st
                        .Select(r => r.ProjectId)
                        .Distinct()
                        .ToList();

                    var missingProjectIds = Enumerable.Range(1, 8)
                        .Except(existingProjectIds)
                        .ToList();

                    var missingProjects = result.MIS_ProjectMaster
                        .Join(
                            result.MIS_VerticalMaster,
                            p => p.VerticalId,
                            v => v.VerticalId,
                            (p, v) => new { Project = p, Vertical = v }
                        )
                        .Where(pv => missingProjectIds.Contains(pv.Project.ProjectId))
                        .ToList();

                    var projectList = new StringBuilder();

                    foreach (var project in missingProjects)
                    {
                        projectList.AppendLine($"- Project: {project.Project.ProjectName}, Vertical: {project.Vertical.VerticalName}<br/>");
                    }

                    if (projectList.Length > 0)
                    {
                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                            mail.To.Add(new MailAddress("siteincharge01@gmail.com"));
                            mail.CC.Add(new MailAddress("projectmanager8460@gmail.com"));
                            mail.Subject = "Report Submission Reminder";
                            mail.Body = $"<h5>Kind Reminder: The following projects have not yet submitted their reports:</h5><br/>{projectList.ToString()}";
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
        }


        static void Report_pending_08()
        {
            using (var result = new ApplicationDbContext())
            {
                var reports = result.MIS_MISReport
                    .Where(r => r.StatusId == 2)
                    .Join(
                        result.MIS_VerticalMaster,
                        report => report.VerticalId,
                        vertical => vertical.VerticalId,
                        (report, vertical) => new { Report = report, Vertical = vertical }
                    )
                    .Join(
                        result.MIS_ProjectMaster,
                        reportVertical => reportVertical.Report.ProjectId,
                        project => project.ProjectId,
                        (reportVertical, project) => new { ReportVertical = reportVertical, Project = project }
                    )
                    .ToList();

                if (reports.Count > 0)
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                        mail.To.Add(new MailAddress("hoteam8460@gmail.com"));
                        mail.To.Add(new MailAddress("projectmanager8460@gmail.com"));

                        mail.Subject = "Pending Reports for Review";

                        // Constructing the mail body
                        var mailBody = new StringBuilder();
                        mailBody.AppendLine($"<h5>Dear Sir/Madam,</h5>");
                        mailBody.AppendLine("<br/>");
                        mailBody.AppendLine("<p>Greetings!</p>");
                        mailBody.AppendLine("<br/>");
                        mailBody.AppendLine("<p>The following reports are pending for review:</p>");
                        mailBody.AppendLine("<ul>");

                        foreach (var reportData in reports)
                        {
                            var report = reportData.ReportVertical.Report;
                            var vertical = reportData.ReportVertical.Vertical;
                            var project = reportData.Project;

                            mailBody.AppendLine($"<li>Report for the project '{project.ProjectName}' in the vertical '{vertical.VerticalName}'</li>");
                        }

                        mailBody.AppendLine("</ul>");
                        mailBody.AppendLine("<br/>");
                        mailBody.AppendLine("<p>Thank you for your attention.</p>");
                        mailBody.AppendLine("<br/>");

                        mail.Body = mailBody.ToString();
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


        static void Report_Reminder_09()
        {
            DateTime currentDate = DateTime.Now.Date;
            int currentDay = currentDate.Day;

            if (currentDay <= 6)
            {
                using (var result = new ApplicationDbContext())
                {
                    var reports = result.MIS_MISReport
                        .Where(r => r.StatusId == 2)
                        .Join(
                            result.MIS_VerticalMaster,
                            report => report.VerticalId,
                            vertical => vertical.VerticalId,
                            (report, vertical) => new { Report = report, Vertical = vertical }
                        )
                        .Join(
                            result.MIS_ProjectMaster,
                            reportVertical => reportVertical.Report.ProjectId,
                            project => project.ProjectId,
                            (reportVertical, project) => new { ReportVertical = reportVertical, Project = project }
                        )
                        .ToList();

                    if (reports.Count > 0)
                    {
                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                            mail.To.Add(new MailAddress("projectmanager8460@gmail.com"));
                            mail.CC.Add(new MailAddress("hoteam8460@gmail.com"));
                            mail.Subject = "Reminder to approve reported MIS";

                            // Constructing the mail body
                            var mailBody = new StringBuilder();
                            mailBody.AppendLine($"<h5>Dear Sir/Madam,</h5>");
                            mailBody.AppendLine("<br/>");
                            mailBody.AppendLine("<p>Greetings!</p>");
                            mailBody.AppendLine("<br/>");
                            mailBody.AppendLine("<p>Reminder to approve the following reports and send them to the HOD Team for approval:</p>");
                            mailBody.AppendLine("<ul>");

                            foreach (var reportData in reports)
                            {
                                var report = reportData.ReportVertical.Report;
                                var vertical = reportData.ReportVertical.Vertical;
                                var project = reportData.Project;

                                mailBody.AppendLine($"<li>Report for the project '{project.ProjectName}' in the vertical '{vertical.VerticalName}'</li>");
                            }

                            mailBody.AppendLine("</ul>");
                            mailBody.AppendLine("<br/>");
                            mailBody.AppendLine("<p>Thank you for your attention.</p>");
                            mailBody.AppendLine("<br/>");

                            mail.Body = mailBody.ToString();
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
        }

        static void Report_autoapprove_10()
        {
            DateTime currentDate = DateTime.Now.Date;
            int currentDay = currentDate.Day;

            if (currentDay == 7)
            {
                using (var result = new ApplicationDbContext())
                {
                    var reports = result.MIS_MISReport
                        .Where(r => r.StatusId == 1)
                        .Join(
                            result.MIS_VerticalMaster,
                            report => report.VerticalId,
                            vertical => vertical.VerticalId,
                            (report, vertical) => new { Report = report, Vertical = vertical }
                        )
                        .Join(
                            result.MIS_ProjectMaster,
                            reportVertical => reportVertical.Report.ProjectId,
                            project => project.ProjectId,
                            (reportVertical, project) => new { ReportVertical = reportVertical, Project = project }
                        )
                        .ToList();

                    if (reports.Count > 0)
                    {
                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                            mail.To.Add(new MailAddress("projectmanager8460@gmail.com"));
                            mail.CC.Add(new MailAddress("hoteam8460@gmail.com"));
                            mail.Subject = "Auto-approval of reported MIS";

                            // Constructing the mail body
                            var mailBody = new StringBuilder();
                            mailBody.AppendLine($"<h5>Dear Sir/Madam,</h5>");
                            mailBody.AppendLine("<br/>");
                            mailBody.AppendLine("<p>Warm Greetings!</p>");
                            mailBody.AppendLine("<br/>");
                            mailBody.AppendLine("<p>We would like to inform you that the following reports have been auto-approved:</p>");
                            mailBody.AppendLine("<ul>");

                            foreach (var reportData in reports)
                            {
                                var report = reportData.ReportVertical.Report;
                                var vertical = reportData.ReportVertical.Vertical;
                                var project = reportData.Project;

                                mailBody.AppendLine($"<li>Report for the project '{project.ProjectName}' in the vertical '{vertical.VerticalName}'</li>");

                                // Update the StatusId to 3 for the project in the MIS_Report table
                                report.StatusId = 3;
                            }

                            mailBody.AppendLine("</ul>");
                            mailBody.AppendLine("<br/>");
                            mailBody.AppendLine("<p>Thank you for your attention.</p>");
                            mailBody.AppendLine("<br/>");

                            mail.Body = mailBody.ToString();
                            mail.IsBodyHtml = true;

                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.Credentials = new NetworkCredential("noreplyehstesting78@gmail.com", "cntpraqgsaqjlxpi");
                                smtp.EnableSsl = true;
                                smtp.Send(mail);
                            }

                            // Save the changes to the database
                            result.SaveChanges();
                        }
                    }
                }
            }
        }


        static void Report_approved_11()
        {
            using (var result = new ApplicationDbContext())
            {
                var reports = result.MIS_MISReport
                    .Where(r => r.StatusId == 4)
                    .Join(
                        result.MIS_VerticalMaster,
                        report => report.VerticalId,
                        vertical => vertical.VerticalId,
                        (report, vertical) => new { Report = report, Vertical = vertical }
                    )
                    .Join(
                        result.MIS_ProjectMaster,
                        reportVertical => reportVertical.Report.ProjectId,
                        project => project.ProjectId,
                        (reportVertical, project) => new { ReportVertical = reportVertical, Project = project }
                    )
                    .ToList();

                if (reports.Count > 0)
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                        mail.To.Add(new MailAddress("siteincharge@gmail.com"));
                        mail.To.Add(new MailAddress("projectmanager8460@gmail.com"));
                        mail.CC.Add(new MailAddress("hoteam8460@gmail.com"));


                        mail.Subject = "Pending Reports for Review";

                        // Constructing the mail body
                        var mailBody = new StringBuilder();
                        mailBody.AppendLine($"<h5>Dear Sir/Madam,</h5>");
                        mailBody.AppendLine("<br/>");
                        mailBody.AppendLine("<p>Greetings!</p>");
                        mailBody.AppendLine("<br/>");
                        mailBody.AppendLine("<p>The following reports are approved:</p>");
                        mailBody.AppendLine("<ul>");

                        foreach (var reportData in reports)
                        {
                            var report = reportData.ReportVertical.Report;
                            var vertical = reportData.ReportVertical.Vertical;
                            var project = reportData.Project;

                            mailBody.AppendLine($"<li>Report for the project '{project.ProjectName}' in the vertical '{vertical.VerticalName}'</li>");
                        }

                        mailBody.AppendLine("</ul>");
                        mailBody.AppendLine("<br/>");
                        mailBody.AppendLine("<p>Thank you for your attention.</p>");
                        mailBody.AppendLine("<br/>");

                        mail.Body = mailBody.ToString();
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





    }

}
