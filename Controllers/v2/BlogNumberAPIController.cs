using AutoMapper;
using DotNet_API_Example.Data;
using DotNet_API_Example.Models;
using DotNet_API_Example.Models.Dto;
using DotNet_API_Example.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DotNet_API_Example.Controllers.v2
{
    [Route("api/v{version:apiVersion}/BlogNumberAPI")]
    [ApiController]
    [ApiVersion("2.0")]
    public class BlogNumberAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ILogger<BlogAPIController> _logger;
        private readonly IBlogNumberRepository _dbBlogNumber;
        private readonly IBlogRepository _dbBlog;
        private readonly IMapper _mapper;
        public BlogNumberAPIController(IBlogNumberRepository dbBlogNumber, IBlogRepository dbBlog, ILogger<BlogAPIController> logger, IMapper mapper)
        {
            _dbBlogNumber = dbBlogNumber;
            _dbBlog = dbBlog;
            _logger = logger;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        //[MapToApiVersion("2.0")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



    }
}
