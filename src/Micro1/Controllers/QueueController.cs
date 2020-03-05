using Infra.Service.Interfaces;
using Infra.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Micro1.Controllers
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
        // GET: api/Queue
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Queue/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Queue
        [HttpPost]
        public void Post([FromBody] QueueMessage value)
        {
            QueueService.PublishMessage<QueueMessage>(value);
        }

        // PUT: api/Queue/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
