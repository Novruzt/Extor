# Extor 🚀

**Extor** is a powerful and flexible global exception handling library for .NET applications. It provides a fluent interface for configuring exception handling, complete with method chaining and customizable middleware. Whether you're building a small app or a large enterprise system, Extor is here to manage your exceptions gracefully.

## Features ✨

- **Global Exception Handling**: Handle all exceptions in a consistent manner across your application.
- **Fluent API**: Configure exception handling with a clean and readable API.
- **Customizable Middleware**: Easily plug Extor into your middleware pipeline.
- **Service Registration**: Seamlessly integrate with the .NET dependency injection system.
- **.NET Core 6.0+**

## Installation 🛠️

To install Extor, simply add the NuGet package to your project or use bash command:

```bash
dotnet add package Extor
```

## Usage 🎯

1. ### Register Extor Services 
  ```javascript
   builder.Services.AddExtor(typeof(BadRequestException));
  ```
  Registering exception assemblies is optional (You must send method parameterless to do this). It is required if you intend to use the [WithName](#withnamestring-name) method; otherwise, the [WithName](#withnamestring-name) method will default to the `Exception` type.

> [!NOTE]
> For same assembly members, only one `typeof(Exception)` is sufficient. The Method can take more `typeof(Exception)` if you have different assemblies.


2. ### Adding Extor middleware
```javascript
  app.UseExtor()
      .Handle(typeof(BadRequestException), 400, "GLOBAL_MESSAGE", true)
      .Handle(typeof(NullException), HttpStatusCode.BadRequest, "NULL_EXCEPTION", false);
```
 The `Handle` methods are optional. Since you can't alter [Extor middleware](https://github.com/Novruzt/Extor/blob/master/src/Extor/MIddlewares/ExtorMiddleware.cs), you can handle specific exception classes here.

 > [!NOTE]
>The `Handle` method takes parameter `typeof(Exception)`, `int` / `HttpStatusCode`, `globalMessage` and `messageOverriding`

* `typeof(Exception)`: Determines which class will be handled.
* `int` / `HttpStatusCode`: Determines the status code of the message. To see behavior with `builder` methods, click [here](#withstatuscodeint-statuscode--withstatuscodehttpstatuscode-statuscode)
* `globalMessage`: Sets global message for sepcific class. 
* `messageOverriding`: Enables/disables global message overriding. 

To see both `globalMessage` and `messageOverriding` behaviors with `builder` methods, click [here](#withmessagestring-message)

> [!TIP]
> The `app.UseExtor` method handles exceptions not only from IExtor but also from catch blocks, regular exception throwing, and system exceptions.


## Example 🎬

```javascript
public class ExampleController : ControllerBase
{
    private readonly IExtor _extor;

    public ExampleController(IExtor extor)
    {
        _extor = extor;
    }

    [HttpPost]
    public IActionResult Get()
    {
        
        if(true)
        {
            _extor
                .Create()
                .WithType(new TestException())
                .WithMessage("Test-case")
                .WithStatusCode(400)
                .Build()
                .Throw();
        }

        return Ok("Success!");
    }
}
```

## Method Overview 📚
- ### Create() 
  Creates a new exception builder. If no builder methods are used, then an `Exception` will be created with default values.
- ### WithName(string name) 
  Sets the name of the exception. Assembly must be registered [here](#register-extor-services) if `Exception` class is created by user
- ### WithType(Exception exception) 
  Sets the type of the exception. Assembly registration is not needed. Exception message can be passed with `constructor`

> [!NOTE]
> [WithMessage](#withmessagestring-message) method always ignores the `constructor`.

> [!CAUTION] 
> `WithName` and ``WithType`` cannot be used at the same time.
- ### WithStatusCode(int statusCode) / WithStatusCode(HttpStatusCode statusCode) 
  Sets the status code of the exception. Paramater can be passed as an `int` or enum from `System.Net.HttpStatusCode`. Default value is `500`, can be overwritten in the [Handle](#adding-extor-middleware). 

> [!NOTE]
> `WithStatusCode` method always ignores [Handle](#adding-extor-middleware)'s StatusCode.

- ### WithMessage(string message)
  Sets the message of the exception.


>[!NOTE]
> either `constructor` or `WithMessage` will be ignored if [messageOverriding](#adding-extor-middleware) is enabled.

> [!WARNING]
> If [messageOverrading](#adding-extor-middleware) is disabled and [globalMessage](#adding-extor-middleware) is provided, and
    if neither [WithMessage](#withmessagestring-message) nor `constructor` are used for the message then [globalMessage](#adding-extor-middleware) will be triggered.



- ### Build()
   Builds the exception. 
- ### Throw / ThrowIf(bool predicate)
   Throws the built exception. 
  The `Throw` method throws exception regardless of conditions. It’s better used with If-else statements or try-catch blocks. However, The`ThrowIf` method throws exception only if the condition is `true`

> [!TIP]
>More detailed explanations and behaviors of each method, including combinations and global handler methods, can be found in the [TestController](https://github.com/Novruzt/Extor/blob/master/test/Extor.Test/Controllers/TestController.cs)




