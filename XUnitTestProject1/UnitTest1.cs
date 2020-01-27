using System;
using TradingPlaces.WebApi.Dtos;
using TradingPlaces.WebApi.Controllers;
using TradingPlaces.WebApi.Filters;
using TradingPlaces;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void RegisterStrategyTest1()
        {
            StrategyDetailsDto strategyDetails = new StrategyDetailsDto();
            strategyDetails.Instruction = TradingPlaces.Resources.BuySell.Buy;
            strategyDetails.PriceMovement = 2;
            strategyDetails.Quantity = 10;
            strategyDetails.Ticker = "MSBT";
            StrategyController strategyController = new StrategyController();
            var result = strategyController.RegisterStrategy(strategyDetails);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public void RegisterStrategyTest2()
        {
            StrategyDetailsDto strategyDetails = new StrategyDetailsDto();
            strategyDetails.Instruction = TradingPlaces.Resources.BuySell.Sell;
            strategyDetails.PriceMovement = 2;
            strategyDetails.Quantity = 10;
            strategyDetails.Ticker = "MSBT";
            StrategyController strategyController = new StrategyController();
            var result = strategyController.RegisterStrategy(strategyDetails);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public void UnregisterStrategyTest2()
        {
            StrategyDetailsDto strategyDetails = new StrategyDetailsDto();
            strategyDetails.Instruction = TradingPlaces.Resources.BuySell.Sell;
            strategyDetails.PriceMovement = 2;
            strategyDetails.Quantity = 10;
            strategyDetails.Ticker = "MSBT";
            StrategyController strategyController = new StrategyController();
            var mockDependency = new Mock<StrategyController>();
            CreatedResult result = new CreatedResult("12345678", strategyDetails);
            mockDependency.Setup(x => x.RegisterStrategy(strategyDetails)).Returns(result);
            string id = "12345678";
            var result2 = strategyController.UnregisterStrategy(id);
            var okResult = result2 as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

    }
}
