using System;
using DecaBlog.Helpers;
using System.Collections.Generic;
﻿using System.Linq;
using DecaBlog.Data.Repositories.Interfaces;
using System.Threading.Tasks;
using DecaBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace DecaBlog.Data.Repositories.Implementations
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly DecaBlogDbContext _context;
        public ArticleRepository(DecaBlogDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateContribution(Article model)
        {
            await _context.Articles.AddAsync(model);
            return await SaveChangesAsync();
        }

        public Task<List<Article>> GetArticles()
        {
            return _context.Articles.ToListAsync();
        }

        public Task<List<Article>> GetArticlesByKeyword(string keyword)
        {
            return _context.Articles.Where(x => x.Keywords.Contains(keyword)).Include(x => x.Contributor).Include(x => x.ArticleTopic).ToListAsync();
        }

        public async Task<Article> PublishArticleAsync(string articleId, string currentUserAsPublisherId)
        {
            var articleToBePublish = await _context.Articles.FirstOrDefaultAsync(x => x.Id == articleId);

            if (articleToBePublish == null) return null;
            articleToBePublish.PublisherId = currentUserAsPublisherId;
            articleToBePublish.IsPublished = true;

            return articleToBePublish;
        }

        public async Task<Article> UnPublishArticleAsync(string articleId, string currentUserAsPublisherId)
        {
            var articleToBeUnPublish = await _context.Articles.FirstOrDefaultAsync(x => x.Id == articleId);

            if (articleToBeUnPublish == null) return null;
            articleToBeUnPublish.PublisherId = currentUserAsPublisherId;
            articleToBeUnPublish.IsPublished = false;

            return articleToBeUnPublish;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<IQueryable<Article>> GetArticleByContributorId(string contributorId)
        {
            var articles = _context.Articles
                .Include(t => t.ArticleTopic)
                .Where(x => x.UserId == contributorId);
            return articles;
        }

        public async Task<IQueryable<Article>> GetArticleByPublisherId(string publisherId)
        {
            var articles = _context.Articles
                .Include(t => t.ArticleTopic)
                .Include(u => u.Contributor)
                .Where(x => x.PublisherId == publisherId);
            return articles;
        }
    }
}