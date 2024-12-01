﻿using Microsoft.EntityFrameworkCore;
using Tutorial.Services.AccountService;

namespace Tutorial.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext, IAccountService accountService)
        {
            await this.CheckTokenAsync(httpContext, accountService, "JWT");

            await _next(httpContext);
        }

        private async Task CheckTokenAsync(HttpContext httpContext, IAccountService accountService, string headerField)
        {
            string? token = httpContext.Request.Headers[headerField].FirstOrDefault()?.Split(" ").Last();

            if (token != null && token != String.Empty)
                await accountService.CheckJwtToken(httpContext, token!);
        }

    }
}
