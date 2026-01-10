// <copyright file="TechnologyController.cs" company="DevCodeX">
// Copyright (c) 2025 All Rights Reserved
// </copyright>

using DevCodeX_API.Data.Constants;
using DevCodeX_API.Data.Entities;
using DevCodeX_API.Data.Responses;
using DevCodeX_API.Data.Shared;
using DevCodeX_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevCodeX_API.Controllers
{
    /// <summary>
    /// Controller for managing Technology entities
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TechnologyController : ControllerBase
    {
        private readonly ITechnologyService _service;
        private readonly ILogger<TechnologyController> _logger;

        public TechnologyController(
            ITechnologyService service,
            ILogger<TechnologyController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get all technologies without filtering
        /// </summary>
        [HttpGet]
        public async Task<Response<List<Technology>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return new Response<List<Technology>>(Status.Succeeded, HttpStatusCode.OK, result)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all technologies");
                return new Response<List<Technology>>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Get paginated list of technologies
        /// </summary>
        [HttpPost("list")]
        public async Task<Response<List<Technology>>> GetListAsync([FromBody] Filter filter)
        {
            try
            {
                var result = await _service.GetListAsync(filter);
                return new Response<List<Technology>>(
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
                _logger.LogError(ex, "Error getting technology list");
                return new Response<List<Technology>>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Get technology by ID
        /// </summary>
        [HttpGet("{uid:guid}")]
        public async Task<Response<Technology>> GetByUidAsync(Guid uid)
        {
            try
            {
                var result = await _service.GetByIdAsync(uid);
                return new Response<Technology>(Status.Succeeded, HttpStatusCode.OK, result!)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting technology by ID");
                return new Response<Technology>(Status.Failed, HttpStatusCode.NotFound, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Create new technology
        /// </summary>
        [HttpPost]
        public async Task<Response<Technology>> CreateAsync([FromBody] Technology entity)
        {
            try
            {
                var result = await _service.CreateAsync(entity);
                return new Response<Technology>(Status.Succeeded, HttpStatusCode.OK, Message.AddSuccess, result)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating technology");
                return new Response<Technology>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Update existing technology
        /// </summary>
        [HttpPut("{uid:guid}")]
        public async Task<Response<Technology>> UpdateAsync([FromBody] Technology entity, Guid uid)
        {
            try
            {
                var result = await _service.UpdateAsync(uid, entity);
                return new Response<Technology>(Status.Succeeded, HttpStatusCode.OK, Message.UpdateSuccess, result!)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating technology");
                return new Response<Technology>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Delete existing technology
        /// </summary>
        [HttpDelete("{uid:guid}")]
        public async Task<Response<bool>> Delete(Guid uid)
        {
            try
            {
                var result = await _service.DeleteAsync(uid);
                var status = result ? Status.Succeeded : Status.Failed;
                var statusCode = result ? HttpStatusCode.OK : HttpStatusCode.NotFound;
                var message = result ? Message.DeleteSuccess : "Technology not found";
                return new Response<bool>(status, statusCode, message)
                {
                    IsSuccess = result,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting technology");
                return new Response<bool>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }
    }
}
