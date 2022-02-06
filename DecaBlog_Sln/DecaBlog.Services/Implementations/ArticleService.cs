using AutoMapper;
using DecaBlog.Commons.Helpers;
using DecaBlog.Data.Repositories.Interfaces;
using DecaBlog.Models;
using DecaBlog.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DecaBlog.Models.DTO;
using DecaBlog.Services.Extensions;
using DecaBlog.Services.Interfaces;
using Microsoft.AspNetCore.Http;


namespace DecaBlog.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userMgr;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public ArticleService(IArticleRepository articleRepository, IMapper mapper,IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<PaginatedListDto<ArticlesToReturnByContributorIdDto>> GetArticleByContributorId(string contributorId, int pageNumber, int perPage)
        {
            var articles = await _articleRepository.GetArticleByContributorId(contributorId);

            var returnDto = new List<ArticlesToReturnByContributorIdDto>();
            foreach (var article in articles)
            {
                var publisher = await _userMgr.FindByIdAsync(article.PublisherId);
                returnDto.Add(new ArticlesToReturnByContributorIdDto
                {
                    ContributionId = article.Id,
                    ArticleText = article.ArticleText,
                    TopicId = article.ArticleTopicId,
                    Keywords = article.Keywords,
                    SubTopic = article.SubTopic,
                    CreatedAt = article.DateCreated,
                    UpdatedAt = article.DateUpdated,
                    Topic = article.ArticleTopic.Topic,
                    Publisher = new PublisherDto
                    {
                        PublisherId = article.PublisherId,
                        PublisherBy = $"{publisher.FirstName} {publisher.LastName}"
                    }
                });
            }

            var result = returnDto.GroupBy(c => c.TopicId).SelectMany(x => x.Select(x => x)).Select(x => x).ToList();
            var paginatedList = PagedList<ArticlesToReturnByContributorIdDto>.Paginate(result, pageNumber, perPage);

            return paginatedList;
        }

        public async Task<PaginatedListDto<ArticlesToReturnByPublisherIdDto>> GetArticleByPublisherId(string publisherId, int pageNumber, int perPage)
        {
            var articles = await _articleRepository.GetArticleByPublisherId(publisherId);
            if (articles is null) throw new Exception("Error occurred");

            var returnDto = new List<ArticlesToReturnByPublisherIdDto>();
            foreach (var article in articles)
            {
                returnDto.Add(new ArticlesToReturnByPublisherIdDto
                {
                    ContributionId = article.Id,
                    ArticleText = article.ArticleText,
                    TopicId = article.ArticleTopicId,
                    Keywords = article.Keywords,
                    SubTopic = article.SubTopic,
                    CreatedAt = article.DateCreated,
                    UpdatedAt = article.DateUpdated,
                    Topic = article.ArticleTopic.Topic,
                    Contributor = new ContributorDto
                    {
                        ContributorId = article.UserId,
                        ContributorName = $"{article.Contributor.FirstName} {article.Contributor.LastName}"
                    }

                });
            }

            var result = returnDto.GroupBy(c => c.TopicId).SelectMany(x => x.Select(x => x)).Select(x => x).ToList();
            var paginatedList = PagedList<ArticlesToReturnByPublisherIdDto>.Paginate(result, pageNumber, perPage);

            return paginatedList; 
        }

        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.GetLoggedInUserId<string>();
        }
        public async Task<CreatedContributionDTO> CreateContribution(AddContributionDTO model, string topicId, User user)
        {            
            //create the article topic since you are the owner of the article
            var article = _mapper.Map<Article>(model);
            article.Contributor = user;
            article.ArticleTopicId = topicId;
            var createContribution = await _articleRepository.CreateContribution(article);
            if (!createContribution)
            {
                return null;
            }
            return _mapper.Map<CreatedContributionDTO>(article);
        }
        public async Task<PublisUnPublishArticleResponseDto> PublishArticleAsync(string articleId)
        {
            var currentUserAsPublisherId = GetUserId();
            var articlePublishedResponse = await _articleRepository.PublishArticleAsync(articleId, currentUserAsPublisherId);
            if (articlePublishedResponse is null) return null;
            await _articleRepository.SaveChangesAsync();

            return _mapper.Map<PublisUnPublishArticleResponseDto>(articlePublishedResponse);
        }

        public async Task<PublisUnPublishArticleResponseDto> UnPublishArticleAsync(string articleId)
        {
            var currentUserAsPublisherId = GetUserId();
            var articlePublishedResponse = await _articleRepository.UnPublishArticleAsync(articleId, currentUserAsPublisherId);
            if (articlePublishedResponse is null) return null;
            await _articleRepository.SaveChangesAsync();

            return _mapper.Map<PublisUnPublishArticleResponseDto>(articlePublishedResponse);
        }

        public async Task<Tuple<bool, string, List<ArticleByKeywordDto>>> GetArticleByKeyword(string keyword)
        {
            var ListToReturn = new List<Article>();
            var articleToReturnResponseDto = new List<ArticleByKeywordDto>();
            var articleList = await _articleRepository.GetArticlesByKeyword(keyword);
            for (int i = 0; i < articleList.Count; i++)
            {
                var keywordSplit = articleList[i].Keywords.Split(",");
                var result = keywordSplit.Contains(keyword);
                if (result)
                {
                    ListToReturn.Add(articleList[i]);
                }
                articleToReturnResponseDto = _mapper.Map <List<ArticleByKeywordDto>>(ListToReturn);
            }
            return new Tuple<bool, string, List<ArticleByKeywordDto>>(true, "Article Found", articleToReturnResponseDto);
        }

    }
}