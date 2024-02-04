using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sucre_Core.LoggerExternal;
using System.Text.RegularExpressions;

namespace Sucre_MVC.Filters
{
    public class ResourceFilterSimple : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_RESOURCE...SimpleIn");
            context.HttpContext.Response.Cookies.Append("LastVisit",DateTime.Now.ToString("dd/MM/yyyyy HH-mm-ss"));
        }
    }

    public class ResourceFilterAsyncSimple : Attribute, IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_RESOURCE...SimpleAsync");
            context.HttpContext.Response.Cookies.Append("AsyncLastVisit", DateTime.Now.ToString("dd/MM/yyyyy HH-mm-ss"));
            await next();
        }
    }

    public class ResourceFilterAsyncParam : Attribute, IAsyncResourceFilter
    {
        private int _id;
        private string _token;
        public ResourceFilterAsyncParam(int id, string token)
        {
            _id = id;
            _token = token;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_RESOURCE...ParamAsync");
            context.HttpContext.Response.Headers.Add("Id", _id.ToString());
            context.HttpContext.Response.Headers.Add("token", _token.ToString());
            await next();
        }
    }

    public class ResourceFilterAsyncLog : Attribute, IAsyncResourceFilter
    {
        private ILogger _log;        
        public ResourceFilterAsyncLog(ILoggerFactory log)
        {
            _log = log.CreateLogger(typeof(ResourceFilterAsyncLog));     
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_RESOURCE...LoggerAsync");
            _log.LogWarning($"!!!Filter SimpleAsyncLogResourceFilter-OnResourceExecutionAsync - {DateTime.Now} - {context.RouteData.Values} - {context.HttpContext.Request.QueryString.Value}");
            await next();
        }
    }

    public class ResourceFilterLog : Attribute, IResourceFilter
    {
        private ILogger _log;
        public ResourceFilterLog(ILoggerFactory log)
        {
            //_log = log;
            _log = log.CreateLogger(typeof(ResourceFilterLog));

        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_RESOURCE...LoggerOut");
            
            _log.LogWarning($"!!!Filter SimpleLogResourceFilter-OnResourceExecuted - {DateTime.Now} - {context.RouteData.Values} - ...");
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_RESOURCE...LoggerIn");
            _log.LogWarning($"!!!Filter SimpleLogResourceFilter-OnResourceExecution - {DateTime.Now} - {context.RouteData.Values} - {context.HttpContext.Request.QueryString.Value}");
        }
    }

    public class ResourceFilterGlobal : Attribute, IResourceFilter, IOrderedFilter
    {
        public ResourceFilterGlobal(int order) => Order = order;
        public int Order { get; }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_RESOURCE...GlobalOut");
            TestGlobalFilterResourceValue.GlobalResourceIn += "GlobalOut-";
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_RESOURCE...GlobalIn");
            TestGlobalFilterResourceValue.GlobalResourceIn += "GlobalIn-";
        }
    }

    public class ResourceFilterController : Attribute, IResourceFilter, IOrderedFilter
    {
        public ResourceFilterController(int order) => Order = order;
        public int Order { get; }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_RESOURCE...ControllerOut");
            TestGlobalFilterResourceValue.GlobalResourceIn += "ControllerOut-";
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_RESOURCE...ControllerIn");
            TestGlobalFilterResourceValue.GlobalResourceIn += "ControllerIn-";
        }
    }

    public class ResourceFilterAction : Attribute, IResourceFilter, IOrderedFilter
    {
        public ResourceFilterAction(int order) => Order = order;
        public int Order { get; }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_RESOURCE...ActionOut");
            TestGlobalFilterResourceValue.GlobalResourceIn += "ActionOut-";
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_RESOURCE...ActionIn");
            TestGlobalFilterResourceValue.GlobalResourceIn += "ActionIn-";
        }
    }

    public static class TestGlobalFilterResourceValue
    {
        public static string GlobalResourceIn { get; set; } = string.Empty;
        public static string GlobalResourceOut { get; set; } = string.Empty;
    }

    public class ResourceFilterAsyncIE : Attribute, IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            LoggerExternal.LoggerEx.Information($"*->FILTER_TEST_RESOURCE...IE - {context.HttpContext.Request.Headers["User-Agent"].ToString()}");
            string userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();
            string browser = context.HttpContext.Request.Headers["sec-ch-ua"].ToString();
            if (Regex.IsMatch(userAgent, "MSIE|Trident") || (Regex.IsMatch(userAgent, "Firefox")))
            {
                //"[User-Agent, Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/113.0]"

                context.Result = new ContentResult { Content = "Ваш браузер заблокирован"};
            }
            else { await next(); }
        }
    }
}

