using DecaBlog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecaBlog.Data.Repositories.Interfaces
{
    public interface IArticleTopicRepository
    {
        Task<bool> AddArticleTopic(ArticleTopic model);
        Task<List<ArticleTopic>> GetArticleByCategory(string id);
        Task<bool> SaveChanges();
    }
}
