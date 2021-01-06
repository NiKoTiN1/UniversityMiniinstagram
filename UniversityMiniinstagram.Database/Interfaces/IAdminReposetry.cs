using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface IAdminReposetry
    {
        public Task AddReport(Report report);
        public ICollection<Report> GetPostReports();
        public ICollection<Report> GetCommentReports();
        public Report GetReport(Guid reportId);
        public Task RemoveReport(Report report);
    }
}
