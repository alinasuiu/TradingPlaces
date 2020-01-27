using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using TradingPlaces.WebApi.Dtos;
using TradingPlaces.WebApi.Services;

namespace TradingPlaces.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class StrategyController : ControllerBase
    {
        private readonly IHostedServiceAccessor<IStrategyManagementService> _strategyManagementService;
        private readonly ILogger<StrategyController> _logger;
        public static Dictionary<string,StrategyDetailsDto> strategies = new Dictionary<string, StrategyDetailsDto>();
        public StrategyController()
        {

        }
        public StrategyController(IHostedServiceAccessor<IStrategyManagementService> strategyManagementService, ILogger<StrategyController> logger)
        {
            _strategyManagementService = strategyManagementService;
            _logger = logger;
        }
        private string generateID()
        {
            long i = 1;

            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }

            string number = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);

            return number;
        }
        [HttpGet]
        [SwaggerOperation(nameof(getStrategies))]
        [SwaggerResponse(StatusCodes.Status200OK, "OK")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        public Dictionary<string, StrategyDetailsDto> getStrategies()
        {
            return strategies;
        }

        [HttpPost]
        [SwaggerOperation(nameof(RegisterStrategy))]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(string))]
        public IActionResult RegisterStrategy(StrategyDetailsDto strategyDetails)
        {

            if (strategyDetails == null) return BadRequest();
            string ID = generateID();
            strategies.Add(ID,strategyDetails);
            return CreatedAtAction(nameof(RegisterStrategy), new {id =  ID},strategyDetails);

        }

        [HttpDelete("{id}")]
        [SwaggerOperation(nameof(UnregisterStrategy))]
        [SwaggerResponse(StatusCodes.Status200OK, "OK")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        public IActionResult UnregisterStrategy(string id)
        {
            if (id == null) return BadRequest();
            strategies.Remove(id);
            return CreatedAtAction(nameof(UnregisterStrategy), id);
            //throw new NotImplementedException();
        }
        
    }
}
