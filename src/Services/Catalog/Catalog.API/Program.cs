using BuildingBlocks.Behaviors;

var builder = WebApplication.CreateBuilder(args);

// add services to the container
var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);
builder
    .Services.AddMarten(opts =>
    {
        opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    })
    .UseLightweightSessions();
builder.Services.AddCarter();

var app = builder.Build();

// configure the HTTP request pipeline
app.MapCarter();
app.UseExceptionHandler(exceptionHandlerApp => { });

app.Run();
