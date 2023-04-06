using System;
using BuilderGenerator.Test.Net60.Builders;
using BuilderGenerator.Test.Net60.Models;
using NUnit.Framework;
using Shouldly;

namespace BuilderGenerator.Test.Net60.Tests;

[TestFixture]
public class RecordExampleBuilderTests
{
    private int _parameterOne;
    private bool _parameterTwo;
    private string _parameterThree;
    private RecordExample _result;

    [Test]
    public void Simple_returns_a_RecordExampleBuilder() => RecordExampleBuilder.Typical().ShouldBeOfType<RecordExampleBuilder>();

    [Test]
    public void Typical_returns_a_RecordExampleBuilder() => RecordExampleBuilder.Typical().ShouldBeOfType<RecordExampleBuilder>();

    [Test]
    public void RecordExampleBuilder_can_set_properties()
    {
        _result.ParameterOne.ShouldBe(_parameterOne);
        _result.ParameterTwo.ShouldBe(_parameterTwo);
        _result.ParameterThree.ShouldBe(_parameterThree);
    }

    [OneTimeSetUp]
    public void SetUp()
    {
        _parameterOne = Random.Shared.Next();
        _parameterTwo = Random.Shared.Next(0, 5) == 3;
        _parameterThree = Guid.NewGuid().ToString();

        _result = RecordExampleBuilder
            .Typical()
            .WithParameterOne(_parameterOne)
            .WithParameterTwo(_parameterTwo)
            .WithParameterThree(_parameterThree)
            .Build();
    }
}
