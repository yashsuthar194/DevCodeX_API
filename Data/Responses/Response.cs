// <copyright file="Response.cs" company="DevCodeX">
// Copyright (c) 2025 All Rights Reserved
// </copyright>

using System.Net;

namespace DevCodeX_API.Data.Responses
{
    /// <summary>
    /// Generic API Response wrapper
    /// </summary>
    /// <typeparam name="T">Response data type</typeparam>
    public class Response<T>
    {
        /// <summary>
        /// Response status (Succeeded/Failed)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Indicates if the operation was successful
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// HTTP status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Response message
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Response data
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Pagination metadata (populated for list endpoints)
        /// </summary>
        public PaginationMeta? Pagination { get; set; }

        /// <summary>
        /// Default parameterless constructor
        /// </summary>
        public Response()
        {
            Status = string.Empty;
            StatusCode = 200;
        }

        /// <summary>
        /// Constructor with status and status code
        /// </summary>
        public Response(string status, HttpStatusCode statusCode)
        {
            Status = status;
            StatusCode = (int)statusCode;
        }

        /// <summary>
        /// Constructor with status, status code, and data
        /// </summary>
        public Response(string status, HttpStatusCode statusCode, T data)
        {
            Status = status;
            StatusCode = (int)statusCode;
            Data = data;
        }

        /// <summary>
        /// Constructor with status, status code, and message
        /// </summary>
        public Response(string status, HttpStatusCode statusCode, string message)
        {
            Status = status;
            StatusCode = (int)statusCode;
            Message = message;
        }

        /// <summary>
        /// Constructor with all parameters
        /// </summary>
        public Response(string status, HttpStatusCode statusCode, string message, T data)
        {
            Status = status;
            StatusCode = (int)statusCode;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// Constructor with data and pagination
        /// </summary>
        public Response(string status, HttpStatusCode statusCode, T data, PaginationMeta pagination)
        {
            Status = status;
            StatusCode = (int)statusCode;
            Data = data;
            Pagination = pagination;
        }
    }

    /// <summary>
    /// Pagination metadata for list responses
    /// </summary>
    public class PaginationMeta
    {
        /// <summary>
        /// Current page number (1-indexed)
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of items
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

        /// <summary>
        /// Whether there is a next page
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;

        /// <summary>
        /// Whether there is a previous page
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        public PaginationMeta() { }

        public PaginationMeta(int pageIndex, int pageSize, int totalCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }
}
