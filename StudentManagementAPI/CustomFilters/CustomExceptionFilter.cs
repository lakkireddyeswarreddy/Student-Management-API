using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;

namespace StudentManagementAPI.CustomFilters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string message = "An error occured";

            int statusCode = 500;

            if(context.Exception is SqlException sqlException)
            {
                message = "Database exception occured, check your connection string or query.";
                statusCode = 504;
            }

            var response = new
            {
                message = message,
                details = context.Exception.Message
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;


        }
    }
}
