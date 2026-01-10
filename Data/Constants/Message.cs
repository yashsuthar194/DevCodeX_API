// <copyright file="Message.cs" company="DevCodeX">
// Copyright (c) 2025 All Rights Reserved
// </copyright>

namespace DevCodeX_API.Data.Constants
{
    /// <summary>
    /// Standard API Response Messages
    /// </summary>
    public static class Message
    {
        public const string AddSuccess = "Record created successfully";
        public const string UpdateSuccess = "Record updated successfully";
        public const string DeleteSuccess = "Record deleted successfully";
        public const string DeleteError = "Failed to delete record";
        public const string NotFound = "Record not found";
        public const string SomethingWentWrong = "Something went wrong";
        public const string Unauthorized = "Unauthorized access";
    }
}
