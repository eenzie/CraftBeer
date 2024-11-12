var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

#region Dapr config

// Set Dapr gRPC Port (or assume default if not set)
var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT");
if (string.IsNullOrEmpty(daprGrpcPort))
{
    Console.WriteLine("DAPR_GRPC_PORT not set. Assuming 50001.");
    daprGrpcPort = "50001";
    Environment.SetEnvironmentVariable("DAPR_GRPC_PORT", daprGrpcPort);
}

// Configure Dapr for service communication
builder.Services.AddControllers()
    .AddDapr(config => config
    .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));

#endregion

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

app.MapControllers();

app.Run();
