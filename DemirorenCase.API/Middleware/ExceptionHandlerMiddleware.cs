using System;
using System.Threading.Tasks;
using DemirorenCase.API.ValueObjects;
using DemirorenCase.Common.Services;
using DemirorenCase.Core.Abstractions;
using DemirorenCase.Infrastructure.Abstractions.Services;
using Microsoft.AspNetCore.Http;

namespace DemirorenCase.API.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRabbitQueue<ILog> _rabbitQueue;

        public ExceptionHandlerMiddleware(RequestDelegate next, IRabbitQueue<ILog> rabbitQueue)
        {
            _next = next;
            _rabbitQueue = rabbitQueue;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                throw new Exception("Hello");
                await _next(context);
            }
            catch (Exception e)
            {
                _rabbitQueue.Publish(new Log
                {
                    Message = e.Message
                });
                Console.WriteLine(e);
                throw;
            }
        }
    }
}