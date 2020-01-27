using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TradingPlaces.Resources;
using TradingPlaces.WebApi.Dtos;
using TradingPlaces.WebApi.Controllers;
using System.Collections.Generic;

namespace TradingPlaces.WebApi.Services
{
    internal class StrategyManagementService : TradingPlacesBackgroundServiceBase, IStrategyManagementService
    {
        private const int TickFrequencyMilliseconds = 1000;

        public StrategyManagementService(ILogger<StrategyManagementService> logger)
            : base(TimeSpan.FromMilliseconds(TickFrequencyMilliseconds), logger)
        {
        }

        protected override Task CheckStrategies()
        {
            // TODO: Check registered strategies.
            StrategyController strategyController = new StrategyController();
            Dictionary<string, double> prevPrice = new Dictionary<string, double>();
            var reutberg = new Reutberg.ReutbergService();
            foreach(var strategy in strategyController.getStrategies())
            {
                string ticker = strategy.Value.Ticker;
                double newQuote = (double)reutberg.GetQuote(ticker);
                if (!prevPrice.ContainsKey(ticker)) prevPrice[ticker] = newQuote;
                else
                    if ((newQuote / prevPrice[ticker]) == ((100.0 + (double)strategy.Value.PriceMovement) / 100.0))
                {
                    if (strategy.Value.Instruction == BuySell.Buy)
                    {
                        reutberg.Buy(ticker, strategy.Value.Quantity);
                        strategyController.UnregisterStrategy(strategy.Key);
                    }
                    else
                    if (strategy.Value.Instruction == BuySell.Sell)
                    {
                        reutberg.Sell(ticker, strategy.Value.Quantity);
                        strategyController.UnregisterStrategy(strategy.Key);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
