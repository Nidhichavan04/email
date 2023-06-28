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

namespace Task
{
    class Program
    {
        static void Main(string[] args)
        {
            Report_submitted_01();
            Report_submitted_02();
            Report_reviewed_02();
            ReportReviewed_03();
            Report_approved_04();
            Report_Reminder_05();
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

                foreach (var reportData in reports)
                {
                    var report = reportData.ReportVertical.Report;
                    var vertical = reportData.ReportVertical.Vertical;
                    var project = reportData.Project;

                    var user = users.FirstOrDefault(u => u.UserId == report.EntryBy);

                    if (user != null && user.RoleId == 1)
                    {
                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                            mail.To.Add(new MailAddress(user.Email));
                            mail.Subject = "Report Submitted";

                            string body = "<h5>Dear Sir,</h5>";
                            body += $"<p>Thank you for submitting your report for the project '{project.ProjectName}' in the vertical '{vertical.VerticalName}'.</p>";
                            body += "<p>Your report has been successfully submitted to the project manager for review.</p>";
                          
                            body += "<p>Thank you once again, and have a productive day!</p>";

                            mail.Body = body;
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

                foreach (var reportData in reports)
                {
                    var report = reportData.ReportVertical.Report;
                    var vertical = reportData.ReportVertical.Vertical;
                    var project = reportData.Project;

                    var users = result.MIS_Users.Where(u => u.RoleId == 2).ToList();

                    foreach (var user in users)
                    {
                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                            mail.To.Add(new MailAddress(user.Email));
                            mail.Subject = "Report submitted for review";

                            // Constructing the mail body
                            var mailBody = new StringBuilder();
                            mailBody.AppendLine($"<h5>Dear sir,</h5>");
                            mailBody.AppendLine("<br/>");
                            mailBody.AppendLine("<p>Greetings!</p>");
                            mailBody.AppendLine("<br/>");
                            mailBody.AppendLine($"<p>This is to inform you that the report for the project '{project.ProjectName}' in the vertical '{vertical.VerticalName}' has been successfully submitted by the site manager for review.</p>");
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

                foreach (var reportData in reports)
                {
                    var report = reportData.ReportVertical.Report;
                    var vertical = reportData.ReportVertical.Vertical;
                    var project = reportData.Project;

                    var users = result.MIS_Users.Where(u => u.RoleId == 1).ToList();

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                        mail.To.Add(new MailAddress("hoteam8460@gmail.com"));
                        mail.Subject = "Report has been reviewed.";

                        var mailBody = $"Dear Sir/Madam,\n\n";
                        mailBody += $"I am writing to inform you that the report for the project '{project.ProjectName}' in the vertical '{vertical.VerticalName}' has been reviewed by the project manager. Kindly approve the report at your earliest convenience.\n\n";
                        mailBody += $"Thank you for your attention to this matter.\n\n";
                       
                        mail.Body = mailBody;
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

                foreach (var reportData in reports)
                {
                    var report = reportData.ReportVertical.Report;
                    var vertical = reportData.ReportVertical.Vertical;
                    var project = reportData.Project;
                    var users = result.MIS_Users.Where(u => u.RoleId == 3).ToList();

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                        mail.To.Add(new MailAddress("siteincharge01@gmail.com"));
                        mail.Subject = "Report has been sent back for corrections.";
                        mail.Body = $"<h5>Dear Sir/Madam,</h5>" +
                                    $"<p>This email is to inform you that the report for the project '{project.ProjectName}' in the vertical '{vertical.VerticalName}' has been reviewed by the EHS team at the Head Office. However, there are some corrections that need to be made in the report.</p>" +
                                    $"<p>We kindly request you to review the identified corrections and make the necessary updates to ensure the accuracy and completeness of the report.</p>" +
                                    $"<p>Thank you for your attention to this matter.</p>" +
                                    $"<p>Best regards,</p>" +
                                    $"<p>Your Name</p>"; // Replace 'Your Name' with your actual name or the sender's name
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

                foreach (var reportData in reports)
                {
                    var report = reportData.ReportVertical.Report;
                    var vertical = reportData.ReportVertical.Vertical;
                    var project = reportData.Project;
                    var users = result.MIS_Users.Where(u => u.RoleId == 2).ToList();

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                        mail.To.Add(new MailAddress("siteincharge01@gmail.com"));
                        mail.Subject = "Report has been Approved.";
                        mail.Body = $"<h5>Dear Team,</h5><p>The report for the project '{project.ProjectName}' in the vertical '{vertical.VerticalName}' has been approved.</p><p>Please review the report and take any necessary actions accordingly.</p><p>Thank you for your attention.</p>";

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
            int currentMonth = DateTime.Now.Month;
            DateTime lastDayOfPreviousMonth = new DateTime(todayDate.Year, currentMonth, 1).AddDays(-1);

            if (todayDate.Date == lastDayOfPreviousMonth.Date)
            {
                using (var result = new ApplicationDbContext())
                {
                    var existingProjectIds = result.MIS_MISReport
                        .Where(r => r.EntryDate.Month == currentMonth - 1) // Check for the previous month's report
                        .Select(r => r.ProjectId)
                        .Distinct()
                        .ToList();

                    var missingProjectIds = Enumerable.Range(1, 8)
                        .Except(existingProjectIds)
                        .ToList();

                    foreach (int projectId in missingProjectIds)
                    {
                        var project = result.MIS_ProjectMaster
                            .Join(
                                result.MIS_VerticalMaster,
                                p => p.VerticalId,
                                v => v.VerticalId,
                                (p, v) => new { Project = p, Vertical = v }
                            )
                            .FirstOrDefault(pv => pv.Project.ProjectId == projectId);

                        if (project != null)
                        {
                            using (MailMessage mail = new MailMessage())
                            {
                                mail.From = new MailAddress("noreplyehstesting78@gmail.com");
                                mail.To.Add(new MailAddress("siteincharge01@gmail.com"));
                                mail.Subject = "Reminder";
                                mail.Body = $"<h5>Report for the project '{project.Project.ProjectName}' in the vertical '{project.Vertical.VerticalName}' is missing for the previous month. Please submit it as soon as possible.</h5>";
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




    }
}
