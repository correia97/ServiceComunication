using Infra.Service.Interfaces;
using Infra.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Micro2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly IQueueService QueueService;
        public QueueController(IQueueService queueService)
        {
            QueueService = queueService;
        }
        // POST: api/Queue
        [HttpPost]
        public void Post([FromBody] QueueMessage value)
        {
            QueueService.PublishMessage<QueueMessage>(value);
        }

    }
}
