using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

using System.Threading.Tasks;
using System;
using RMotownFestival.Api.Data;
using RMotownFestival.Api.Domain;
using System.Net;

namespace RMotownFestival.Api.Controllers
{
    [Route("api/[controller]")]
    public class ArticlesController : Controller
    {
        private CosmosClient _cosmosClient { get; set; }

        private Container _websiteArticlesContainer { get; set; }


        public ArticlesController(IConfiguration configuration)
        {
            _cosmosClient = new CosmosClient(configuration.GetConnectionString("CosmosConnection"));
            _websiteArticlesContainer = _cosmosClient.GetContainer("RMotownArticles", "WebsiteArticles");   
         }

        [HttpPost("Add")]
        public async Task<ActionResult> AddArticle()
        {
            var dummyArticle = new Article
            {
                Id = Guid.NewGuid().ToString(),
                Title = "test2",
                Date = DateTime.Now,
                Status = "Unpublished",
                Tag = "Tag2",
                ImagePath = "",
                Message = "hey2"

            };
            await _websiteArticlesContainer.CreateItemAsync( dummyArticle);
            return Ok();
        }

        

        //    public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
