using Extor.Extensions;
using Extor.Test.Exceptions;

namespace Extor.Test;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Registering Extor. Assemblies can be registered for method
        ///<see cref="Interfaces.IExceptionBuilder.WithName(string)"/>
        builder.Services.AddExtor(typeof(BadRequestException));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        //Adding ExtorMiddleware with Handling cases (optional)
        app.UseExtor()
            .Handle(typeof(BadRequestException), 400, "GLOBAL_MESSAGE", true)
            .Handle(typeof(NullException), 404, "NULL_EXCEPTION", false);

        app.MapControllers();

        app.Run();
    }
}
