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

namespace DotNet_API_Example.Controllers.v1
{
    [Route("api/v{version:apiVersion}/BlogNumberAPI")]
    [ApiController]
    [ApiVersion("1.0",Deprecated = true)]

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

        // LIST
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default60")]
        //[ResponseCache(Location =ResponseCacheLocation.None, NoStore = true)]
        //[MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetBlogNumbers()
        {

            try
            {
                _logger.LogInformation("Started Fetch for BLOG Number LIST:");
                IEnumerable<BlogNumber> blogNumberList = await _dbBlogNumber.GetAllAsync();
                _response.Result = _mapper.Map<List<BlogNumberDTO>>(blogNumberList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        // GET
        [HttpGet("{id:int}", Name = "GetBlogNumber")]
        [ResponseCache(CacheProfileName = "Default60")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetBlogNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogInformation("Get blog Number error with Id: " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                _logger.LogInformation("Started Fetch for BLOG ID: " + id);

                var blogNumber = await _dbBlogNumber.GetAsync(u => u.BlogNo == id);
                if (blogNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<BlogNumberDTO>(blogNumber);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        // CREATE
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateBlogNumber([FromBody] BlogNumberCreateDTO createDTO)
        {
            try
            {
                if (await _dbBlogNumber.GetAsync(u => u.BlogNo == createDTO.BlogNo) != null)
                { // Check is title unique, this means title is not unique
                    ModelState.AddModelError("CustomError", "Blog Number already exists!");
                    return BadRequest(ModelState);
                }

                if (await _dbBlog.GetAllAsync(u => u.Id == createDTO.BlogID) == null)
                {
                    ModelState.AddModelError("Custom Error", "Blog ID is invalid");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                BlogNumber blogNumber = _mapper.Map<BlogNumber>(createDTO);

                await _dbBlogNumber.CreateAsync(blogNumber);

                _response.Result = _mapper.Map<BlogNumberDTO>(blogNumber);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetBlogNumber", new { id = blogNumber.BlogNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }


        // DELETE
        [HttpDelete("{id:int}", Name = "DeleteBlogNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteBlogNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var blogNumber = await _dbBlogNumber.GetAsync(u => u.BlogNo == id);
                if (blogNumber == null)
                {
                    return NotFound();
                }
                await _dbBlogNumber.RemoveAsync(blogNumber);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        // UPDATE
        [HttpPut("{id:int}", Name = "UpdateBlogNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateBlogNumber(int id, [FromBody] BlogNumberUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.BlogNo)
                {
                    return BadRequest();
                }

                if (await _dbBlog.GetAllAsync(u => u.Id == updateDTO.BlogID) == null)
                {
                    ModelState.AddModelError("Custom Error", "Blog ID is invalid");
                    return BadRequest(ModelState);
                }

                BlogNumber model = _mapper.Map<BlogNumber>(updateDTO);

                await _dbBlogNumber.UpdateAsync(model);


                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        // PATCH
        [HttpPatch("{id:int}", Name = "UpdatePartialBlogNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialBlogNumber(int id, JsonPatchDocument<BlogNumberUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var blogNumber = await _dbBlogNumber.GetAsync(u => u.BlogNo == id, tracked: false);

            BlogNumberUpdateDTO blogNumberDTO = _mapper.Map<BlogNumberUpdateDTO>(blogNumber);

            if (blogNumber == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(blogNumberDTO, ModelState);

            BlogNumber model = _mapper.Map<BlogNumber>(blogNumberDTO);


            await _dbBlogNumber.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
