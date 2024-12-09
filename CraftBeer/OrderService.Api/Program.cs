using Dapr.Workflow;
using OrderService.Application.Activities;
using OrderService.Application.Workflows;

var builder = WebApplication.CreateBuilder(args);

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger _logger = factory.CreateLogger("Program");

builder.AddServiceDefaults();

#region Dapr config (!!! Superceded by Aspire !!!)
// Set Dapr gRPC Port (or assume default if not set)
//var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT");
//if (string.IsNullOrEmpty(daprGrpcPort))
//{
//    Console.WriteLine("DAPR_GRPC_PORT not set. Assuming 50001.");
//    daprGrpcPort = "50001";
//    Environment.SetEnvironmentVariable("DAPR_GRPC_PORT", daprGrpcPort);
//}

// Configure Dapr for service communication
//builder.Services.AddControllers()
//    .AddDapr(config => config
//    .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));
#endregion

// Register Dapr Workflow & Activities
builder.Services.AddDaprWorkflow(options =>
{
    Console.WriteLine("Registering workflows and activities...");
    _logger.LogInformation("Registering workflows and activities...");
    options.RegisterWorkflow<OrderWorkflow>();

    options.RegisterActivity<NotificationActivity>();
    options.RegisterActivity<OrderCreationActivity>();
    options.RegisterActivity<ReservationActivity>();
    options.RegisterActivity<PaymentActivity>();
    options.RegisterActivity<ShippingActivity>();

    _logger.LogInformation("All workflows and activities registered successfully...");
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Map endpoints and use Dapr middleware for handling Cloud Events and pub/sub
app.MapDefaultEndpoints();
app.UseCloudEvents();
app.MapSubscribeHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();