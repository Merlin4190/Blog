using DecaBlog.Models;
using DecaBlog.Models.DTO;
using System;
using System.Collections.Generic;
﻿using System.Threading.Tasks;

namespace DecaBlog.Services.Interfaces
{
    public interface IArticleService
    {
        Task<CreatedContributionDTO> CreateContribution(AddContributionDTO model, string topicId,User user);
        Task<Tuple<bool, string, List<ArticleByKeywordDto>>> GetArticleByKeyword(string keyword);
        Task<PaginatedListDto<ArticlesToReturnByPublisherIdDto>> GetArticleByPublisherId(string publisherId, int pageNumber, int perPage);
        Task<PaginatedListDto<ArticlesToReturnByContributorIdDto>> GetArticleByContributorId(string contributorId, int pageNumber, int perPage);
        Task<PublisUnPublishArticleResponseDto> PublishArticleAsync(string articleId);
        Task<PublisUnPublishArticleResponseDto> UnPublishArticleAsync(string articleId);
    }
}