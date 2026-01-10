// <copyright file="BaseActionController.cs" company="DevCodeX">
// Copyright (c) 2025 All Rights Reserved
// </copyright>

using DevCodeX_API.Data.Constants;
using DevCodeX_API.Data.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace DevCodeX_API.Controllers.Base
{
    /// <summary>
    /// Base Action Controller providing standardized response methods
    /// </summary>
    public abstract class BaseActionController : ControllerBase
    {
        #region 'Response Helpers'

        /// <summary>
        /// Returns a successful response with data
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <param name="result">Entity result class instance</param>
        /// <returns></returns>
        [NonAction]
        protected Response<TEntity> Succeeded<TEntity>(TEntity result)
        {
            return new Response<TEntity>(Status.Succeeded, HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Returns a successful response with message only
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        protected Response<TEntity> Succeeded<TEntity>(string message)
        {
            return new Response<TEntity>(Status.Succeeded, HttpStatusCode.OK, message);
        }

        /// <summary>
        /// Returns a successful response with data and message
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        protected Response<TEntity> Succeeded<TEntity>(TEntity result, string message)
        {
            return new Response<TEntity>(Status.Succeeded, HttpStatusCode.OK, message, result);
        }

        /// <summary>
        /// Returns a failed response
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <returns></returns>
        [NonAction]
        protected Response<TEntity> Failed<TEntity>()
        {
            return new Response<TEntity>(Status.Failed, HttpStatusCode.InternalServerError, Message.SomethingWentWrong);
        }

        /// <summary>
        /// Returns a failed response with custom message
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        protected Response<TEntity> Failed<TEntity>(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = Message.SomethingWentWrong;

            return new Response<TEntity>(Status.Failed, HttpStatusCode.InternalServerError, message);
        }

        /// <summary>
        /// Returns a bad request response
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        protected Response<TEntity> BadRequest<TEntity>(string message)
        {
            return new Response<TEntity>(Status.Failed, HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// Returns an unauthorized response
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <returns></returns>
        [NonAction]
        protected Response<TEntity> Unauthorized<TEntity>()
        {
            return new Response<TEntity>(Status.Failed, HttpStatusCode.Unauthorized, Message.Unauthorized);
        }

        /// <summary>
        /// Returns a not found response
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <returns></returns>
        [NonAction]
        protected Response<TEntity> NotFound<TEntity>()
        {
            return new Response<TEntity>(Status.Failed, HttpStatusCode.NotFound, Message.NotFound);
        }

        /// <summary>
        /// Returns a not found response with custom message
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        protected Response<TEntity> NotFound<TEntity>(string message)
        {
            return new Response<TEntity>(Status.Failed, HttpStatusCode.NotFound, message);
        }

        /// <summary>
        /// Fetch model errors
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        [NonAction]
        protected string FetchModelError(ModelStateDictionary modelState)
        {
            return string.Join(Environment.NewLine, modelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));
        }

        #endregion
    }
}
