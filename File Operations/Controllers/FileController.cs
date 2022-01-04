using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FileOperations.Controllers
{
    [ApiController]
    public class FileController : ControllerBase
    {

        public FileController(ILogger<FileController> logger)
        {
       
        }

        [Route("API/SaveFile")]
        public async Task<string> SaveFileAsync ()
        {
            try
            {
                // Posted file taken 
                var files = HttpContext.Request.Form.Files;

                var uploads = (@"C:\Uploads\");  
                foreach (var _file in files)
                {
                    if (_file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(_file.ContentDisposition).FileName.Trim('"');
                        // Guid is used for making the file name unique
                        fileName = Guid.NewGuid() + "_" + fileName;
                        await _file.CopyToAsync(new FileStream(uploads + fileName, FileMode.Create));
                    }
                }               
                return JsonConvert.SerializeObject(new
                {
                    success = true,
                    message = "File is successfuly uploaded!"
                });
            }
            catch (Exception err)
            {
                return JsonConvert.SerializeObject(new
                {
                    success = false,
                    message = "An error occured while saving the file",
                    errMessage = err.Message,
                    innerExMessage = err.InnerException != null ? err.InnerException.Message : ""
                });
            }
        }
    }
}
