using AutoMapper;
using DotNet_API_Example.Data;
using DotNet_API_Example.Models;
using DotNet_API_Example.Models.Dto;
using DotNet_API_Example.Repository.IRepository;
using DotNet_API_Example.Controllers.v1;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DotNet_API_Example.Controllers.v2
{
    [Route("api/v{version:apiVersion}/BlogAPI")]
    [ApiController]
    [ApiVersion("2.0")]
    public class BlogAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ILogger<BlogAPIController> _logger;
        private readonly IBlogRepository _dbBlog;
        private readonly IMapper _mapper;

        public BlogAPIController(IBlogRepository dbBlog, ILogger<BlogAPIController> logger, IMapper mapper)
        {
            _dbBlog = dbBlog;
            _logger = logger;
            _mapper = mapper;
            this._response = new();
        }


        // LIST
        [HttpGet]
        //[MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetBlogsV2()
        {

            try
            {
                _logger.LogInformation("Started Fetch for BLOG LIST:");
                IEnumerable<Blog> blogList = await _dbBlog.GetAllAsync();
                _response.Result = _mapper.Map<List<BlogDTO>>(blogList);
                _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess= false;
                _response.ErrorMessages=new List<string>() { ex.ToString() };
            }
            return _response;
        }

    }
}
