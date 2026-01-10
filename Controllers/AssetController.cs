// <copyright file="AssetController.cs" company="DevCodeX">
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
    /// Controller for managing Asset entities
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService _service;
        private readonly ILogger<AssetController> _logger;

        public AssetController(
            IAssetService service,
            ILogger<AssetController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get all assets without filtering
        /// </summary>
        [HttpGet]
        public async Task<Response<List<Asset>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return new Response<List<Asset>>(Status.Succeeded, HttpStatusCode.OK, result)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all assets");
                return new Response<List<Asset>>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Get paginated list of assets
        /// </summary>
        [HttpPost("list")]
        public async Task<Response<List<Asset>>> GetListAsync([FromBody] Filter filter)
        {
            try
            {
                var result = await _service.GetListAsync(filter);
                return new Response<List<Asset>>(
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
                _logger.LogError(ex, "Error getting asset list");
                return new Response<List<Asset>>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Get asset by ID
        /// </summary>
        [HttpGet("{uid:guid}")]
        public async Task<Response<Asset>> GetByUidAsync(Guid uid)
        {
            try
            {
                var result = await _service.GetByIdAsync(uid);
                return new Response<Asset>(Status.Succeeded, HttpStatusCode.OK, result!)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting asset by ID");
                return new Response<Asset>(Status.Failed, HttpStatusCode.NotFound, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Create new asset
        /// </summary>
        [HttpPost]
        public async Task<Response<Asset>> CreateAsync([FromBody] Asset entity)
        {
            try
            {
                var result = await _service.CreateAsync(entity);
                return new Response<Asset>(Status.Succeeded, HttpStatusCode.OK, Message.AddSuccess, result)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating asset");
                return new Response<Asset>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Update existing asset
        /// </summary>
        [HttpPut("{uid:guid}")]
        public async Task<Response<Asset>> UpdateAsync([FromBody] Asset entity, Guid uid)
        {
            try
            {
                var result = await _service.UpdateAsync(uid, entity);
                return new Response<Asset>(Status.Succeeded, HttpStatusCode.OK, Message.UpdateSuccess, result!)
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating asset");
                return new Response<Asset>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Delete existing asset
        /// </summary>
        [HttpDelete("{uid:guid}")]
        public async Task<Response<bool>> Delete(Guid uid)
        {
            try
            {
                var result = await _service.DeleteAsync(uid);
                var status = result ? Status.Succeeded : Status.Failed;
                var statusCode = result ? HttpStatusCode.OK : HttpStatusCode.NotFound;
                var message = result ? Message.DeleteSuccess : "Asset not found";
                return new Response<bool>(status, statusCode, message)
                {
                    IsSuccess = result,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting asset");
                return new Response<bool>(Status.Failed, HttpStatusCode.InternalServerError, ex.Message)
                {
                    IsSuccess = false
                };
            }
        }
    }
}
