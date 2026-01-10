// <copyright file="PaginatedList.cs" company="DevCodeX">
// Copyright (c) 2025 All Rights Reserved
// </copyright>

namespace DevCodeX_API.Data.Shared
{
    /// <summary>
    /// Wrapper for paginated list results with metadata
    /// </summary>
    /// <typeparam name="T">Type of items in the list</typeparam>
    public class PaginatedList<T>
    {
        /// <summary>
        /// The items on the current page
        /// </summary>
        public List<T> Items { get; set; } = [];

        /// <summary>
        /// Total count of all items (not just this page)
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Current page index (1-indexed)
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

        public PaginatedList() { }

        public PaginatedList(List<T> items, int totalCount, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
