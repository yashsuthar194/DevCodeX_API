// <copyright file="BaseController.cs" company="DevCodeX">
// Copyright (c) 2025 All Rights Reserved
// </copyright>

using DevCodeX_API.Data.Constants;
using DevCodeX_API.Data.Responses;
using DevCodeX_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevCodeX_API.Controllers.Base
{
    /// <summary>
    /// Base controller providing common CRUD operations for all entity controllers
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="TFilter">Filter type for querying</typeparam>
    public abstract class BaseController<T, TFilter> : BaseActionController 
        where T : class 
        where TFilter : class
    {
        #region 'Initialization'

        /// <summary>
        /// Service Instance
        /// </summary>
        protected readonly IBaseService<T, TFilter> _service;

        /// <summary>
        /// Logger Instance
        /// </summary>
        protected readonly ILogger? _logger;
        #endregion

        #region 'Constructor'

        /// <summary>
        /// BaseController Constructor
        /// </summary>
        protected BaseController(IBaseService<T, TFilter> service)
        {
            _service = service;
        }

        /// <summary>
        /// BaseController Constructor with logger
        /// </summary>
        protected BaseController(IBaseService<T, TFilter> service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        #endregion

        #region 'CRUD API'
        /// <summary>
        /// Get all records without filtering
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<Response<List<T>>> GetAll()
        {
            return Succeeded(await _service.GetAllAsync());
        }

        /// <summary>
        /// Get all records with paged result and filtering
        /// </summary>
        [HttpPost("list")]
        public virtual async Task<Response<List<T>>> GetListAsync([FromBody] TFilter filter)
        {
                return Succeeded(await _service.GetListAsync(filter));
        }

        /// <summary>
        /// Get record by ID
        /// </summary>
        [HttpGet("{uid:guid}")]
        public virtual async Task<Response<T>> GetByUidAsync(Guid uid)
        {
            return Succeeded(await _service.GetByIdAsync(uid));
        }

        /// <summary>
        /// Create new record
        /// </summary>
        [HttpPost]
        public virtual async Task<Response<T>> CreateAsync([FromBody] T entity)
        {
            return Succeeded(await _service.CreateAsync(entity), Message.AddSuccess);
        }

        /// <summary>
        /// Update existing record
        /// </summary>
        [HttpPut("{uid:guid}")]
        public virtual async Task<Response<T>> UpdateAsync([FromBody] T entity, Guid uid)
        {
            return Succeeded(await _service.UpdateAsync(uid, entity), Message.UpdateSuccess);
        }

        /// <summary>
        /// Delete existing record
        /// </summary>
        [HttpDelete("{uid:guid}")]
        public virtual async Task<Response<T>> Delete(Guid uid)
        {
            var result = await _service.DeleteAsync(uid);
            return Succeeded<T>(Message.DeleteSuccess);
        }

        #endregion
    }
}
