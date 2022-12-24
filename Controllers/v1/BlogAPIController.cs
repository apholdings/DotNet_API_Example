using AutoMapper;
using Azure;
using DotNet_API_Example.Data;
using DotNet_API_Example.Models;
using DotNet_API_Example.Models.Dto;
using DotNet_API_Example.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DotNet_API_Example.Controllers.v1
{
    [Route("api/v{version:apiVersion}/BlogAPI")]
    [ApiController]
    [ApiVersion("1.0", Deprecated =true)]
    //[ApiVersionNeutral]
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
            _response = new();
        }


        // LIST
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default60")]
        //[MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetBlogs([FromQuery(Name ="filterByRate")] double? rate,
            [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {

            try
            {
                IEnumerable<Blog> blogList;


                if (rate > 0)
                {
                    blogList = await _dbBlog.GetAllAsync(u => u.Rate == rate, pageSize:pageSize, pageNumber:pageNumber);
                }
                else
                {
                    blogList = await _dbBlog.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    blogList = blogList.Where(u=> u.Title.ToLower().Contains(search.ToLower()) 
                    || u.Description.ToLower().Contains(search.ToLower()));
                }

                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));

                _response.Result = _mapper.Map<List<BlogDTO>>(blogList);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _logger.LogInformation("Started Fetch for BLOG LIST:");
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        // GET
        [HttpGet("{id:int}", Name = "GetBlog")]
        [ResponseCache(CacheProfileName = "Default60")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetBlog(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogInformation("Get blog error with Id: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "Blog doesn't exist" };
                    return BadRequest(_response);
                }

                _logger.LogInformation("Started Fetch for BLOG ID: " + id);

                var blog = await _dbBlog.GetAsync(u => u.Id == id);
                if (blog == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "Blog not found" };
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<BlogDTO>(blog);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        // CREATE
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateBlog([FromBody] BlogCreateDTO createDTO)
        {
            try
            {
                if (await _dbBlog.GetAsync(u => u.Title.ToLower() == createDTO.Title.ToLower()) != null)
                { // Check is title unique, this means title is not unique
                    ModelState.AddModelError("CustomError", "Blog with Title already exists!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                Blog blog = _mapper.Map<Blog>(createDTO);

                await _dbBlog.CreateAsync(blog);

                _response.Result = _mapper.Map<BlogDTO>(blog);
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                return CreatedAtRoute("GetBlog", new { id = blog.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }


        // DELETE
        [HttpDelete("{id:int}", Name = "DeleteBlog")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteBlog(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var blog = await _dbBlog.GetAsync(u => u.Id == id);
                if (blog == null)
                {
                    return NotFound();
                }
                await _dbBlog.RemoveAsync(blog);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        // UPDATE
        [HttpPut("{id:int}", Name = "UpdateBlog")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateBlog(int id, [FromBody] BlogUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                Blog model = _mapper.Map<Blog>(updateDTO);

                await _dbBlog.UpdateAsync(model);


                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        // PATCH
        [HttpPatch("{id:int}", Name = "UpdatePartialBlog")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialBlog(int id, JsonPatchDocument<BlogUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var blog = await _dbBlog.GetAsync(u => u.Id == id, tracked: false);

            BlogUpdateDTO blogDTO = _mapper.Map<BlogUpdateDTO>(blog);

            if (blog == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(blogDTO, ModelState);

            Blog model = _mapper.Map<Blog>(blogDTO);


            await _dbBlog.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
