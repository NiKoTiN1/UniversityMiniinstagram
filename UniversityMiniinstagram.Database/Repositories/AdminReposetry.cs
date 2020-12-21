using System;
using System.Collections.Generic;
using System.Linq;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Repositories
{
    public class AdminReposetry : IAdminReposetry
    {
        public AdminReposetry(DatabaseContext context)
        {
            this.Context = context;
        }

        private readonly DatabaseContext Context;
        public void AddReport(Report report)
        {
            report.Date = DateTime.Now;
            this.Context.Reports.Add(report);
            this.Context.SaveChanges();
        }

        public ICollection<Report> GetPostReports()
        {
            var result = this.Context.Reports.Where(report => report.PostId != null).OrderBy(report => report.Date).ToList();
            return result;
        }

        public ICollection<Report> GetCommentReports()
        {
            var result = this.Context.Reports.Where(report => report.CommentId != null).OrderBy(report => report.Date).ToList();
            return result;
        }

        public Report GetReport(Guid reportId)
        {
            Report report = this.Context.Reports.FirstOrDefault(report => report.Id == reportId);
            return report;
        }

        public void RemoveReport(Report report)
        {
            this.Context.Reports.Remove(report);
            this.Context.SaveChanges();
        }
    }
}
