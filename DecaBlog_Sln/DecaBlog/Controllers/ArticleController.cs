using DecaBlog.Commons.Helpers;
using DecaBlog.Models;
using System.Threading.Tasks;
using DecaBlog.Commons.Helpers;
using DecaBlog.Helpers;
using DecaBlog.Models.DTO;
using DecaBlog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DecaBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IArticleTopicService _articleTopicService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<User> _userManager;

        public ArticleController(IArticleService articleService, IArticleTopicService articleTopicService, ICategoryService categoryService, UserManager<User> userManager)
        {
            _articleService = articleService;
            _articleTopicService = articleTopicService;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        [HttpPost("add-article")]
        [Authorize(Roles = "Admin, Editor, Decadev")]
        public async Task<IActionResult> CreateArticle([FromForm] AddArticleDTO model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByIdAsync(userId);

            var photo = model.Photo;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _articleTopicService.CreateNewArticle(model, photo, user);
            if (response == null)
            {
                ModelState.AddModelError("Failed To Add Article", "Article Addition Failed, Please Try again");
                return BadRequest(ResponseHelper.BuildResponse<string>(false, "Article failed to Add.", ModelState, null));
            }
            return Ok(ResponseHelper.BuildResponse<object>(true, "Article Added successful", ResponseHelper.NoErrors, response));
        }

        [Authorize(Roles = "Admin, Editor, Decadev")]
        [HttpPost("add-contribution")]
        public async Task<IActionResult> AddContribution([FromBody] AddContributionDTO model, string topicId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _articleService.CreateContribution(model, topicId, user);
            if (response==null)
            {
                ModelState.AddModelError("Failed To Add Article", "Article Addition Failed, Please Try again");
                return BadRequest(ResponseHelper.BuildResponse<string>(false, "Article failed to Add.", ModelState, null));
            }
            return Ok(ResponseHelper.BuildResponse<object>(true, "Contribution Added successful", ResponseHelper.NoErrors, response));
        }

        [HttpPatch("publish-article/{articleId}")]
        public async Task<IActionResult> PublishArticle([FromRoute] string articleId)
        {
            if (string.IsNullOrWhiteSpace(articleId))
            {
                ModelState.AddModelError("", "Bad articleId format");
                return BadRequest(ResponseHelper.BuildResponse<object>(false, "Empty articleId", ModelState, null));
            }

            var articleResponse = await _articleService.PublishArticleAsync(articleId);
            if (articleResponse != null)
                return Ok(ResponseHelper.BuildResponse<object>(true, "Article successfully published", ResponseHelper.NoErrors, articleResponse));
            ModelState.AddModelError("", "Article not found");
            return NotFound(ResponseHelper.BuildResponse<object>(false, "An error occured", ModelState, null));
        }

        [HttpPatch("unpublish-article/{articleId}")]
        public async Task<IActionResult> UnPublishArticle([FromRoute] string articleId)
        {
            if (string.IsNullOrWhiteSpace(articleId))
            {
                ModelState.AddModelError("", "Bad articleId format");
                return BadRequest(ResponseHelper.BuildResponse<object>(false, "Empty articleId", ModelState, null));
            }
            var articleResponse = await _articleService.UnPublishArticleAsync(articleId);
            if (articleResponse != null)
                return Ok(ResponseHelper.BuildResponse<object>(true, "Article successfully unpublished", ResponseHelper.NoErrors, articleResponse));
            ModelState.AddModelError("", "Article not found");
            return NotFound(ResponseHelper.BuildResponse<object>(false, "An error occured", ModelState, null));
        }

        [HttpGet("get-articles-by-contributorid/{contributorId}")]
        public async Task<IActionResult> GetArticleByContributorId(string contributorId, int pageNumber, int perPage) =>
           Ok(ResponseHelper.BuildResponse<object>(
               true, "Successfully fetched all contributions by contributionId", ModelState,
               await _articleService.GetArticleByContributorId(contributorId, pageNumber, perPage)));


        [HttpGet("get-articles-by-publisherid/{publisherId}")]
        public async Task<IActionResult> GetArticleByPublisherId(string publisherId, int pageNumber, int perPage) =>
            Ok(ResponseHelper.BuildResponse<object>(
                true, "Successfully fetched all contributions by publisherId", ModelState,
                await _articleService.GetArticleByPublisherId(publisherId, pageNumber, perPage)));
    }

}