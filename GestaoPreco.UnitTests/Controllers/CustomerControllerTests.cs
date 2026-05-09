using Domain;
using GestaoPreco.Application.Queries.Customer;
using GestaoPreco.UI.Server.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace GestaoPreco.UnitTests.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<CustomerController>> _loggerMock;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<CustomerController>>();

            _controller = new CustomerController(
                _mediatorMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task GetAll_Should_Return_Ok()
        {
            // Arrange
            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<ListCustomersQuery>(),
                    default))
                .ReturnsAsync(new List<Customer>());

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
