using System.Collections.Generic;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface IAdminRepository : IBaseRepository<Report>
    {
        public ICollection<Report> GetPostReports();
        public ICollection<Report> GetCommentReports();
    }
}
