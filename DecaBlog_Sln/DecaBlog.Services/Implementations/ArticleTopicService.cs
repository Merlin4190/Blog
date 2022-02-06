
using AutoMapper;
using DecaBlog.Commons.Helpers;
using DecaBlog.Data.Repositories.Implementations;
using DecaBlog.Data.Repositories.Interfaces;
using DecaBlog.Models;
using DecaBlog.Models.DTO;
using DecaBlog.Services.Extensions;
using DecaBlog.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace DecaBlog.Services.Implementations
{
    public class ArticleTopicService : IArticleTopicService
    {
        private readonly IArticleTopicRepository _articleTopicRepository;
        private readonly IMapper _mapper;
        private readonly IArticleService _articleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly ICloudinaryService _cloudinaryService;
        const int PHOTO_MAX_ALLOWABLE_SIZE = 3000000;

        public ArticleTopicService(ICloudinaryService cloudinaryService,IArticleTopicRepository articleTopicRepository, IArticleService articleService, IMapper mapper, IHttpContextAccessor httpContextAccessor,UserManager<User> userManager)
        {
            _articleTopicRepository = articleTopicRepository;
            _mapper = mapper;
            _articleService = articleService;
            _httpContextAccessor=httpContextAccessor;
            _userManager = userManager;
            _cloudinaryService = cloudinaryService;
        }
        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.GetLoggedInUserId<string>();
        }
        public async Task<CreatedArticleDTO> CreateNewArticle(AddArticleDTO model, IFormFile photo, User user)
        {
            PhotoUploadResult uploadResult = new PhotoUploadResult() ;
            if (photo != null)
            {
                uploadResult = await _cloudinaryService.UploadPhoto(photo);
                if (uploadResult == null)
                    return null;
            }

            //create the article topic since you are the owner of the article
            var articleTopic = _mapper.Map<ArticleTopic>(model);
            articleTopic.Author = user;
            articleTopic.CategoryId = model.CategoryId;
            if (photo != null)
            {
                articleTopic.PublicId = uploadResult.PublicId;
                articleTopic.PhotoUrl = uploadResult.Url;
            }
            var createTheArticleTopic = await _articleTopicRepository.AddArticleTopic(articleTopic);

            if (createTheArticleTopic == false)
            {
               return null;
            }

            //create the article with the topic id
            var theArticle = new AddContributionDTO
            {
                SubTopic = model.SubTopic,
                ArtlcleText = model.ArtlcleText,
                Keywords = model.Keywords                
            };
            var createContribution = await _articleService.CreateContribution(theArticle, articleTopic.Id,user);
            if (createContribution == null)
            {               
                return null;
            }
            return _mapper.Map<CreatedArticleDTO>(createTheArticleTopic); ;
        }
    }
}
