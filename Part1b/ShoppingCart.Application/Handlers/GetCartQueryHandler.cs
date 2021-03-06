﻿using System;
using System.Threading.Tasks;
using AzureFromTheTrenches.Commanding.Abstractions;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Repositories;
using ShoppingCart.Commands;

namespace ShoppingCart.Application.Handlers
{
    class GetCartQueryHandler : ICommandHandler<GetCartQuery, Model.ShoppingCart>
    {
        private readonly IShoppingCartRepository _repository;
        private readonly ILogger<GetCartQueryHandler> _logger;

        public GetCartQueryHandler(ILoggerFactory loggerFactory, IShoppingCartRepository repository)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<GetCartQueryHandler>();
        }

        public async Task<Model.ShoppingCart> ExecuteAsync(GetCartQuery command, Model.ShoppingCart previousResult)
        {
            _logger.LogInformation("Getting basket for user {0}", command.UserId);
            try
            {
                Model.ShoppingCart cart = await _repository.GetActualOrDefaultAsync(command.UserId);
                _logger.LogInformation("Retrieved cart for user {0} with {1} items", command.UserId, cart.Items.Count);
                return cart;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to get basket for user {0}", command.UserId);
                return null;
            }
        }
    }
}
