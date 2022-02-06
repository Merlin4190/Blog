using DecaBlog.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DecaBlog.Models;


namespace DecaBlog.Data.Repositories.Interfaces
{
    public interface IArticleRepository
    {
        Task<bool> CreateContribution(Article model);
        Task<List<Article>> GetArticles();
        Task<List<Article>> GetArticlesByKeyword(string keyword);
        Task<IQueryable<Article>> GetArticleByPublisherId(string publisherId);
        Task<IQueryable<Article>> GetArticleByContributorId(string contributorId);
        Task<Article> PublishArticleAsync(string articleId,string currentUserAsPublisherId);
        Task<Article> UnPublishArticleAsync(string articleId,string currentUserAsPublisherId);
        Task<bool> SaveChangesAsync();
    }
}