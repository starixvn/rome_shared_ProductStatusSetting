namespace Fanex.ProductStatus.Setting.Web
{
    using System;
    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    internal class ProductStatusValidateAntiForgeryTokenAttribute : Attribute, IAsyncActionFilter
    {
        private const string AntiForgeryFormTokenName = "__RequestVerificationToken";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var antiforgery = context.HttpContext.RequestServices.GetRequiredService<IAntiforgery>();
            
            try
            {
                await antiforgery.ValidateRequestAsync(context.HttpContext);
            }
            catch (AntiforgeryValidationException)
            {
                // TODO: Handle antiforgery validation failure
                // For now, let it propagate
                throw;
            }

            await next();
        }
    }
}
