using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace WebSender
{
}

namespace WebSender.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public HomeController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [Route("/")]
        [HttpGet]
        public ActionResult Index()
        {
            return Ok();
        }
        
        [Route("/Post")]
        [HttpGet]
        public async Task<ActionResult> Post([FromQuery]string value)
        {
            try
            {
                var source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            
                await _publishEndpoint.Publish<ValueEntered>(new
                {
                    Value = value
                },  source.Token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return Ok();
        }
    }
}