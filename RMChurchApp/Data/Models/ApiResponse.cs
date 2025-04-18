﻿using RMChurchApp.Data.Models.Enums;
using System.Text.Json.Serialization;

namespace RMChurchApp.Data.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; }

        [JsonIgnore] // Prevent enum from being serialized
        public ResponseType ResponseType { get; set; }

        // This will be serialized as responseType in JSON
        [JsonPropertyName("responseType")]
        public string ResponseTypeString => ResponseType.ToString().ToLower();

        public ApiResponse() { }

        // Success response
        public ApiResponse(T data, string message = "Request successful", int statusCode = 200)
        {
            Success = true;
            Message = message;
            Data = data;
            StatusCode = statusCode;
            ResponseType = ResponseType.Success;
        }

        // Warning response
        public static ApiResponse<T> Warning(string message, T data = default, int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode,
                ResponseType = ResponseType.Warning
            };
        }

        // Error response
        public static ApiResponse<T> Fail(string message, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                StatusCode = statusCode,
                ResponseType = ResponseType.Error
            };
        }
    }
}
