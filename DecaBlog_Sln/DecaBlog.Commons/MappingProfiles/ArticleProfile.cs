using AutoMapper;
using DecaBlog.Models;
using DecaBlog.Models.DTO;

namespace DecaBlog.Commons.MappingProfiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleInfoDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Article, ArticleByKeywordDto>().ForMember("ArticleId", dest=>dest.MapFrom(src=>src.Id))
                .ForMember("KeyWord", src=>src.MapFrom(src=>src.Keywords));
            CreateMap<User, Contributor>().ForMember("FullName", dest => dest.MapFrom(src => string.Join(' ', src.FirstName, src.LastName)));
            CreateMap<Article,PublisUnPublishArticleResponseDto>().ForMember("Status", dest=> dest.MapFrom(src=>src.IsPublished?"Published":"UnPublished"));
            CreateMap<AddContributionDTO, Article>();
            CreateMap<AddArticleDTO, ArticleTopic>();
            CreateMap<AddArticleTopicDTO, ArticleTopic>();
            CreateMap<CreatedContributionDTO, Article>();
            CreateMap<CreatedArticleDTO, ArticleTopic>();
        }
    }
}