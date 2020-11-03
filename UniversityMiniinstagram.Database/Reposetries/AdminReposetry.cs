using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityMiniinstagram.Database.Interfaces;

namespace UniversityMiniinstagram.Database.Models
{
    public class AdminReposetry : IAdminReposetry
    {
        public AdminReposetry(DatabaseContext context)
        {
            _context = context;
        }

        DatabaseContext _context;
        public void AddReport(Report report)
        {
            report.Date = DateTime.Now;
            _context.Reports.Add(report);
            _context.SaveChanges();
        }

        public ICollection<Report> GetPostReports()
        {
            var result = _context.Reports.Where(report => report.PostId != null).OrderBy(report => report.Date).ToList();
            return result;
        }

        public Report GetReport(Guid reportId)
        {
            var report = _context.Reports.FirstOrDefault(report => report.Id == reportId);
            return report;
        }

        public void RemoveReport(Report report)
        {
            _context.Reports.Remove(report);
            _context.SaveChanges();
        }
    }
}
