using Microsoft.AspNetCore.Mvc.Controllers;
using Shyjus.BrowserDetection;
using System.Net;
using System.Text;
using System.Text.Json;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.API.MiddleWares
{

    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        private readonly IEmailSenderService _emailSenderService;
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger, IEmailSenderService emailSenderService)
        {
            _next = next;
            _logger = logger;
            _emailSenderService = emailSenderService;
        }

        public async Task Invoke(HttpContext context, IBrowserDetector detector)
         {
            var browser = detector.Browser;
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                //Log Error
                var controllerName = "";
                var actionName = "";
                var endpoint = context.GetEndpoint();
                if (endpoint != null)
                {
                    var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                    if (controllerActionDescriptor != null)
                    {
                        controllerName = controllerActionDescriptor.ControllerName;
                        actionName = controllerActionDescriptor.ActionName;
                    }
                }

                var request = await FormatRequest(context.Request);
                string txtFileName = Constants.StringValues.COMPANY_NAME + Constants.StringValues.DASH + $"{controllerName}-{actionName}" + Constants.StringValues.UNDERSCORE + DateTime.Now.ToString(Constants.DateFormats.DD_MMM_YY_HH_MM_SS) + Constants.FileType.TEXT;
                CreatErrorText(txtFileName, error, "", browser, request);
                await _emailSenderService.SendEmail(txtFileName);
                _logger.LogError(error, "Application Request: Exception for Request {Name} {@Request}", $"{controllerName}-{actionName}", request);

                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new ErrorDetails() { StatusCode = (int)HttpStatusCode.InternalServerError, Message = error?.Message };


                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                await response.WriteAsync(result);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            // Leave the body open so the next middleware can read it.
            using var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            // Do some processing with body…

            var formattedRequest = $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {body}";

            // Reset the request body stream position so the next middleware can read it
            //request.Body.Position = 0;

            return formattedRequest;
        }

        private static void CreatErrorText(string txtFileName, Exception ex, string userName, IBrowser browser, string request)
        {
            string strNameOfScreen = txtFileName.Substring(0, txtFileName.IndexOf('_'));    //Screen Name
            string tempDirectory = Path.GetTempPath();


            File.WriteAllText(Path.Combine(tempDirectory, txtFileName), "HSKCore Exception Log" + Environment.NewLine);
            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "====================" + Environment.NewLine + Environment.NewLine);

            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "Screen Name: " + strNameOfScreen + Environment.NewLine);
            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "TimeStamp: " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss") + Environment.NewLine);
            string SystemDomainName = Environment.UserDomainName;
            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "AppDomain Name: " + SystemDomainName + Environment.NewLine);  //
            //File.AppendAllText(Path.Combine(tempDirectory, txtFileName),"Machine Name: "+ Environment.MachineName + Environment.NewLine);
            //string path = HttpContext.Current.Request.PhysicalApplicationPath;
            //File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "Base Directory: " + path + Environment.NewLine);
            string WebBrowserVersion = string.Empty;
            WebBrowserVersion = browser.Name;
            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "Browser Name: " + WebBrowserVersion + Environment.NewLine);
            WebBrowserVersion =browser.Version;
            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "Browser Version: " + WebBrowserVersion + Environment.NewLine);

            WebBrowserVersion = browser.OS;
            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "OS Version: " + WebBrowserVersion + Environment.NewLine);

            string systemIdentity = Environment.UserName + "\\" + Environment.MachineName;

            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "System Identity: " + systemIdentity + Environment.NewLine);

            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "====================================================================================================" + Environment.NewLine);


            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "User ID: " + userName + Environment.NewLine);
            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "TimeStamp: " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss") + Environment.NewLine);
            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "Exception Type:" + ex.Message + Environment.NewLine);   //

            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "Request : " + Environment.NewLine);
            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), request + Environment.NewLine);
            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), Environment.NewLine);

            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "SQL : " + Environment.NewLine);
            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), Convert.ToString(ex.Data["sql"]) + Environment.NewLine);
            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), Environment.NewLine);

            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), "Details: " + Environment.NewLine);



            if (ex.InnerException != null)
                File.AppendAllText(Path.Combine(tempDirectory, txtFileName), ex.InnerException.Message + Environment.NewLine);
            File.AppendAllText(Path.Combine(tempDirectory, txtFileName), ex.StackTrace + Environment.NewLine);



            #region "Delete one month privies file"

            string[] files = Directory.GetFiles(tempDirectory, "HSK*.txt");     // Start with "HSK" txt file find 



            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file); ;
                if (fi.CreationTime < DateTime.Now.AddDays(-30))   //Delete text file befor 30 days. 
                    fi.Delete();

            }


            string[] imgNames = Directory.GetFiles(tempDirectory, "*.JPEG");    //For Img Delete
            foreach (string img in imgNames)
            {
                FileInfo imgFi = new FileInfo(img);
                if (imgFi.CreationTime < DateTime.Now.AddDays(-30))
                    imgFi.Delete();

            }

            #endregion


        }

    }
}

