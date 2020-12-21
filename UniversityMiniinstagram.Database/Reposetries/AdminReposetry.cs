using System;
using System.Collections.Generic;
using System.Linq;
using UniversityMiniinstagram.Database.Interfaces;

namespace UniversityMiniinstagram.Database.Models
{
    public class AdminReposetry : IAdminReposetry
    {
        public AdminReposetry(DatabaseContext context)
        {
            Context = context;
        }

        private readonly DatabaseContext Context;
        public void AddReport(Report report)
        {
            report.Date = DateTime.Now;
            Context.Reports.Add(report);
            Context.SaveChanges();
        }

        public ICollection<Report> GetPostReports()
        {
            var result = Context.Reports.Where(report => report.PostId != null).OrderBy(report => report.Date).ToList();
            return result;
        }

        public ICollection<Report> GetCommentReports()
        {
            var result = Context.Reports.Where(report => report.CommentId != null).OrderBy(report => report.Date).ToList();
            return result;
        }

        public Report GetReport(Guid reportId)
        {
            Report report = Context.Reports.FirstOrDefault(report => report.Id == reportId);
            return report;
        }

        public void RemoveReport(Report report)
        {
            Context.Reports.Remove(report);
            Context.SaveChanges();
        }
    }
}
