using DecaBlog.Data.Repositories.Interfaces;
using DecaBlog.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecaBlog.Data.Repositories.Implementations
{
    public class ArticleTopicRepository : IArticleTopicRepository
    {
        private readonly DecaBlogDbContext _context;
        public ArticleTopicRepository(DecaBlogDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddArticleTopic(ArticleTopic model)
        {
            await _context.ArticleTopics.AddAsync(model);
            return await SaveChanges();
        }
        public async Task<List<ArticleTopic>> GetArticleByCategory(string id)
        {
            var articleToReturn = await _context.ArticleTopics.Where(x => x.CategoryId == id).ToListAsync();

            return articleToReturn;
        }
        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
