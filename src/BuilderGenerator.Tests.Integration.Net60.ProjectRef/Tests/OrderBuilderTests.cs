using System;
using BuilderGenerator.Tests.Integration.Net60.ProjectRef.Builders;
using BuilderGenerator.Tests.Integration.Net60.ProjectRef.Models.Entities;
using BuilderGenerator.Tests.Integration.Net60.ProjectRef.Models.Enums;
using NUnit.Framework;
using Shouldly;

namespace BuilderGenerator.Tests.Integration.Net60.ProjectRef.Tests;

[TestFixture]
public class OrderBuilderTests
{
    private Guid _id;
    private string _internalString = null!;
    private DateTime _orderDate;
    private Order _result = null!;
    private OrderStatus _status;

    [Test]
    public void OrderBuilder_can_set_properties()
    {
        _result.Id.ShouldBe(_id);
        _result.InternalString.ShouldBe(_internalString);
        _result.OrderDate.ShouldBe(_orderDate);
        _result.Status.ShouldBe(_status);
    }

    [Test]
    public void Simple_does_not_populate_Items()
    {
        var actual = OrderBuilder.Simple().Build();
        actual.ShouldBeOfType<Order>();
        actual.Items.ShouldBeNull();
    }

    [Test]
    public void Simple_returns_an_OrderBuilder()
    {
        OrderBuilder.Simple().ShouldBeOfType<OrderBuilder>();
    }

    [Test]
    public void Typical_populates_Items()
    {
        var actual = OrderBuilder.Typical().Build();
        actual.ShouldBeOfType<Order>();
        actual.Items.ShouldNotBeNull();
    }

    [Test]
    public void Typical_returns_an_OrderBuilder()
    {
        OrderBuilder.Typical().ShouldBeOfType<OrderBuilder>();
    }

    [OneTimeSetUp]
    public void SetUp()
    {
        _id = Guid.NewGuid();
        _orderDate = DateTime.Now;
        _status = OrderStatus.Processing;
        _internalString = Guid.NewGuid().ToString();

        _result = OrderBuilder
            .Typical()
            .WithId(_id)
            .WithInternalString(_internalString)
            .WithOrderDate(_orderDate)
            .WithStatus(_status)
            .Build();
    }
}
