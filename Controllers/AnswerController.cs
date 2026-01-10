// <copyright file="AnswerController.cs" company="DevCodeX">
// Copyright (c) 2025 All Rights Reserved
// </copyright>

using DevCodeX_API.Data.Constants;
using DevCodeX_API.Data.DTO_s;
using DevCodeX_API.Data.Entities;
using DevCodeX_API.Data.Responses;
using DevCodeX_API.Data.Shared;
using DevCodeX_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevCodeX_API.Controllers
{
    /// <summary>
    /// Controller for managing Answer entities
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _service;
        private readonly ILogger<AnswerController> _logger;

        public AnswerController(
            IAnswerService service,
            ILogger<AnswerController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get all answers without filtering
        /// </summary>
        [HttpGet]
        public async Task<Response<List<Answer>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return new Response<List<Answer>>(Status.Succeeded, HttpStatusCode.OK, result)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all answers");
                return new Response<List<Answer>>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Get paginated list of answers
        /// </summary>
        [HttpPost("list")]
        public async Task<Response<List<Answer>>> GetListAsync([FromBody] Filter filter)
        {
            try
            {
                var result = await _service.GetListAsync(filter);
                return new Response<List<Answer>>(
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
                _logger.LogError(ex, "Error getting answer list");
                return new Response<List<Answer>>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Get answer by ID
        /// </summary>
        [HttpGet("{uid:guid}")]
        public async Task<Response<AnswerDetailDto>> GetByUidAsync(Guid uid)
        {
            try
            {
                var result = await _service.GetByIdAsync(uid);
                return new Response<AnswerDetailDto>(Status.Succeeded, HttpStatusCode.OK, result!)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting answer by ID");
                return new Response<AnswerDetailDto>(Status.Failed, HttpStatusCode.NotFound, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Create new answer
        /// </summary>
        [HttpPost]
        public async Task<Response<Answer>> CreateAsync([FromBody] Answer entity)
        {
            try
            {
                var result = await _service.CreateAsync(entity);
                return new Response<Answer>(Status.Succeeded, HttpStatusCode.OK, Message.AddSuccess, result)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating answer");
                return new Response<Answer>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Update existing answer
        /// </summary>
        [HttpPut("{uid:guid}")]
        public async Task<Response<Answer>> UpdateAsync([FromBody] Answer entity, Guid uid)
        {
            try
            {
                var result = await _service.UpdateAsync(uid, entity);
                return new Response<Answer>(Status.Succeeded, HttpStatusCode.OK, Message.UpdateSuccess, result!)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating answer");
                return new Response<Answer>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Delete existing answer
        /// </summary>
        [HttpDelete("{uid:guid}")]
        public async Task<Response<bool>> Delete(Guid uid)
        {
            try
            {
                var result = await _service.DeleteAsync(uid);
                var status = result ? Status.Succeeded : Status.Failed;
                var statusCode = result ? HttpStatusCode.OK : HttpStatusCode.NotFound;
                var message = result ? Message.DeleteSuccess : "Answer not found";
                return new Response<bool>(status, statusCode, message)
                {
                    IsSuccess = result,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting answer");
                return new Response<bool>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }
    }
}
