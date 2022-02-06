using DecaBlog.Models;
using DecaBlog.Models.DTO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DecaBlog.Services.Interfaces
{
    public interface IArticleTopicService
    {
        Task<CreatedArticleDTO>CreateNewArticle(AddArticleDTO model, IFormFile photo, User user);
    }
}
