using AutoMapper;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Comment, CommentViewModel>()
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src));

            CreateMap<PostReport, AdminPostReportsVeiwModel>()
                .ForMember(dest => dest.Report, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.CommentViewModel, opt => opt.MapFrom(src => src.Post.Comments));

            CreateMap<CommentReport, AdminCommentReportsVeiwModel>()
                .ForMember(dest => dest.Report, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.CommentViewModel, opt => opt.MapFrom(src => src.Comment.Post.Comments));

            CreateMap<ApplicationUser, UserRolesViewModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));

            CreateMap<Post, PostsViewModel>()
                .ForMember(dest => dest.Post, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.CommentVM, opt => opt.MapFrom(src => src.Comments));
            CreateMap<ApplicationUser, EditProfileViewModel>()
                .ForMember(dest => dest.HasPassword, opt => opt.MapFrom(src => src.PasswordHash != null));
        }
    }
}
