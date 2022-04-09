using Application.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebApi.Controllers
{
    [Route("logs")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LogsController : ControllerBase
    {
        private readonly IApplicationConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public LogsController(IApplicationConfiguration configuration,
            IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public ActionResult Logs()
        {
            Dictionary<DateTime, string> logs = new Dictionary<DateTime, string>();
            var env = _env.EnvironmentName.ToLower();

            var pathToFolder = Path.Combine(_env.WebRootPath, "logs", env);

            if (!Directory.Exists(pathToFolder))
            {
                Directory.CreateDirectory(pathToFolder);
            }

            foreach (string file in Directory.EnumerateFiles(
            pathToFolder, "*", SearchOption.AllDirectories))
            {
                var fileName = file.Remove(0, file.Length - 19);
                var date = fileName.Split("_")[0];
                var dateParts = date.Split('-');
                var dateTime = new DateTime(int.Parse(dateParts[2]), int.Parse(dateParts[1]), int.Parse(dateParts[0]));
                var fileUrl = _configuration.GetAppSettings().Api_URL + $"/logs/{env}/" + fileName;
                logs.Add(dateTime, fileUrl);
            }

            return Ok(logs.OrderByDescending(k => k.Key).ToDictionary(x => x.Key.ToString("dd-MM-yyyy"), x => x.Value));
        }
    }
}
