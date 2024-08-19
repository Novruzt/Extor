using Extor.Interfaces;
using Extor.Test.Exceptions;
using Extor.Test.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Extor.Test.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IExtor _extor;

    public TestController(IExtor extor)
    {
        _extor = extor;
    }

    [HttpPost("Default")]
    public IActionResult DefaultCase()
    {
        // Exception will be thrown with default values.
        _extor.Create()
              .Build()
              .Throw();

        return Ok();
    }

    [HttpPost("ByName")]
    public IActionResult ExceptionByName()
    {
        // Exception Type can be called from class names.
        // If class is not found then type will be Exception.
        // Assembly must be registered in program.cs for using this method.
        // NOTICE: WithName cannot be used after WithType.
        _extor.Create()
            .WithName("TestException")
            .Build()
            .Throw();

        return Ok();
    }

    [HttpPost("ByType")]
    public IActionResult ExceptionByType()
    {
        // Exception Type can be called from
        /// <see cref="IExceptionBuilder.WithType"/>.
        // No need to registering assembly in program.cs
        // NOTICE: WithType cannot be used after WithName.
        _extor.Create()
            .WithType(new TestException())
            .Build()
            .Throw();

        return Ok();
    }

    [HttpPost("Message")]
    public IActionResult ExceptionMessage()
    {

        //Exception message can be provided either in Constructor or with
        ///<see cref="IExceptionBuilder.WithMessage(string)"/>.
        // NOTICE-1: WithMessage have priority meaning constructor will be ignored if method called.
        // NOTICE-2: Both usage will be ignored if MessageOverrading Enabled in
        ///<see cref="IExtorBuilder.Handle"/>.
        // NOTICE-3: If MessageOverrading is disabled where globalMessage is provided and
        // if WithMessage or constructor are not used for message then globalMessage will be triggered. Otherwise value is default.

        _extor.Create()
            .WithType(new TestException("FromConstructor"))
            .WithMessage("From Method")
            .Build()
            .Throw();

        return Ok();
    }

    [HttpPost("StatusCode")]
    public IActionResult ExceptionStatusCode()
    {

        //Status code can be setted in
        ///<see cref="IExceptionBuilder.WithStatusCode"/>.
        // with either int or with
        ///<see cref="System.Net.HttpStatusCode"/> .
        // Default value is 500. Can be overriten in progam.cs with
        ///<see cref="IExtorBuilder.Handle"/>.
        // NOTICE: If WithStatusCode is called, then GlobalStatusCode will be ignored.

        _extor.Create()
            .WithType(new TestException("Statuscode"))
            .WithStatusCode(401)
            .Build()
            .Throw();

        return Ok();
    }

    [HttpPost("ThrowIf")]
    public IActionResult ThrowIf()
    {
        User user = new()
        {
            Id = 1,
            Name = "userName",
            Age = 22
        };

        ///<see cref="IThrow.ThrowIf"/> method throw exception only when conditions are met. 
        ///<see cref="IThrow.Throw" /> method throw exception no matter what.
        //It means Throw method requires additional If statement for handling, but not ThrowIf since it has own.
        _extor.Create()
            .WithType(new TestException())
            .WithMessage("Exception with ThrowIf method")
            .Build()
            .ThrowIf(user.Age>18 || user.Id == 1);

        return Ok();
    }

}
