// <copyright file="QuestionController.cs" company="DevCodeX">
// Copyright (c) 2025 All Rights Reserved
// </copyright>

using DevCodeX_API.Data.Constants;
using DevCodeX_API.Data.DTO_s;
using DevCodeX_API.Data.Entities;
using DevCodeX_API.Data.Filters;
using DevCodeX_API.Data.Responses;
using DevCodeX_API.Data.Shared;
using DevCodeX_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevCodeX_API.Controllers
{
    /// <summary>
    /// Controller for managing Question entities
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _service;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(
            IQuestionService service,
            ILogger<QuestionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get all questions without filtering
        /// </summary>
        [HttpGet]
        public async Task<Response<List<Question>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return new Response<List<Question>>(Status.Succeeded, HttpStatusCode.OK, result)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all questions");
                return new Response<List<Question>>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Get paginated list of questions with filters
        /// </summary>
        [HttpPost("list")]
        public async Task<Response<List<QuestionListDto>>> GetListAsync([FromBody] QuestionFilter filter)
        {
            try
            {
                var result = await _service.GetListAsync(filter);
                return new Response<List<QuestionListDto>>(
                    Status.Succeeded, 
                    HttpStatusCode.OK, 
                    result.Items,
                    new PaginationMeta(result.PageIndex, result.PageSize, result.TotalCount))
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting question list");
                return new Response<List<QuestionListDto>>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Get question by ID
        /// </summary>
        [HttpGet("{uid:guid}")]
        public async Task<Response<QuestionDetailDto>> GetByUidAsync(Guid uid)
        {
            try
            {
                var result = await _service.GetByIdAsync(uid);
                return new Response<QuestionDetailDto>(Status.Succeeded, HttpStatusCode.OK, result!)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting question by ID");
                return new Response<QuestionDetailDto>(Status.Failed, HttpStatusCode.NotFound, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Create new question
        /// </summary>
        [HttpPost]
        public async Task<Response<Question>> CreateAsync([FromBody] Question entity)
        {
            try
            {
                var result = await _service.CreateAsync(entity);
                return new Response<Question>(Status.Succeeded, HttpStatusCode.OK, Message.AddSuccess, result)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating question");
                return new Response<Question>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Update existing question
        /// </summary>
        [HttpPut("{uid:guid}")]
        public async Task<Response<Question>> UpdateAsync([FromBody] Question entity, Guid uid)
        {
            try
            {
                var result = await _service.UpdateAsync(uid, entity);
                return new Response<Question>(Status.Succeeded, HttpStatusCode.OK, Message.UpdateSuccess, result!)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating question");
                return new Response<Question>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Delete existing question
        /// </summary>
        [HttpDelete("{uid:guid}")]
        public async Task<Response<bool>> Delete(Guid uid)
        {
            try
            {
                var result = await _service.DeleteAsync(uid);
                var status = result ? Status.Succeeded : Status.Failed;
                var statusCode = result ? HttpStatusCode.OK : HttpStatusCode.NotFound;
                var message = result ? Message.DeleteSuccess : "Question not found";
                return new Response<bool>(status, statusCode, message)
                {
                    IsSuccess = result,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting question");
                return new Response<bool>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }
    }
}
