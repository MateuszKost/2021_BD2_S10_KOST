using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartCollection.DataAccess.Context;
using SmartCollection.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly ILogger<ImagesController> _logger;
        private readonly SmartCollectionDbContext _context;

        public ImagesController(ILogger<ImagesController> logger, SmartCollectionDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Database.CanConnect() ? "connected to db" : "error while connecting to db");
        }
    }
}
