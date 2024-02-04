using Microsoft.AspNetCore.Mvc.Filters;
using Sucre_Core.LoggerExternal;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Sucre_MVC.Filters
{
    public class ActionFilterFirst : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_ACTION...FirstOut");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_ACTION...FirstIn");
        }
    }

    public class ActionFiltesSpaceCleaner : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_ACTION...SpaceCleanerOut");
            var gg = context;

            var regex = new Regex(@"(?<=\s)\s+(?![^<>]*</pre>)");
            var response = context.HttpContext.Response;
            var ee = new SpaceCleaner(response.Body);
            response.Body = new SpaceCleaner(response.Body);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_ACTION...SpaceCleanerIn");
            
        }
    }

    public class SpaceCleaner: Stream
    {
        private readonly Stream outputStream;

        public SpaceCleaner(Stream filterStream) => outputStream = filterStream;
        
        public override bool CanRead { get { return false; } }

        public override bool CanSeek { get { return false; } }

        public override bool CanWrite { get { return false; } }

        public override long Length { get { throw new NotSupportedException(); } }

        public override long Position
        {
            get { throw new NotSupportedException(); } 
            set { throw new NotSupportedException(); }
        }


        public override void Flush() => outputStream.Flush();
        
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            var html = Encoding.UTF8.GetString(buffer, offset, count) ;
            var regex = new Regex(@"(?<=\s)\s+(?![^<>]*</pre>)");
            html = regex.Replace(html, string.Empty);
            buffer = Encoding.UTF8.GetBytes(html);
            await outputStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
        }
    }

    public class ActionFiltesAsyncCheck : Attribute, IAsyncActionFilter
    {
        

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            LoggerExternal.LoggerEx.Information("*->FILTER_TEST_ACTION...CheckAsync");
            var ee = context.ActionArguments;
            
            context.ActionArguments["Id"] = 43;
            await next();
        }
    }

    public class ModelForActionFilter
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        public string PasswordConfirmation { get; set; }
    }
}
