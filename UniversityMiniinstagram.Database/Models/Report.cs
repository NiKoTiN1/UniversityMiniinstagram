using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Index = Microsoft.EntityFrameworkCore.Metadata.Internal.Index;

namespace UniversityMiniinstagram.Database.Models
{
    public class Report
    {
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public Comment Comment { get; set; }
        public Guid? CommentId { get; set; }
        public Post Post { get; set; }
        public Guid? PostId { get; set; }
    }
}
