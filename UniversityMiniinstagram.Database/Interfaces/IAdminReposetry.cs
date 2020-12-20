using System;
using System.Collections.Generic;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface IAdminReposetry
    {
        public void AddReport(Report report);
        public ICollection<Report> GetPostReports();
        public ICollection<Report> GetCommentReports();
        public Report GetReport(Guid reportId);
        public void RemoveReport(Report report);
    }
}
