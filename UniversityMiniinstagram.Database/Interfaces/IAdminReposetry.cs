﻿using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface IAdminReposetry : IBaseReposetry<Report>
    {
        public Task<ICollection<Report>> GetPostReports();
        public Task<ICollection<Report>> GetCommentReports();
    }
}
