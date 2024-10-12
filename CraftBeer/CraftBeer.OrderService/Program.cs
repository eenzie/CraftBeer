using CraftBeer.OrderService.Workflow;
using Dapr.Workflow;

var builder = WebApplication.CreateBuilder(args);

// Dapr ports
var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT");
if (string.IsNullOrEmpty(daprGrpcPort))
{
	Console.WriteLine("DAPR_GRPC_PORT not set. Assuming 50001.");
	daprGrpcPort = "50001";
	Environment.SetEnvironmentVariable("DAPR_GRPC_PORT", daprGrpcPort);
}

builder.Services.AddControllers()
	.AddDapr(config => config
	.UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));

builder.Services.AddDaprWorkflow(options =>
{
	options.RegisterWorkflow<OrderWorkflow>();

	// TODO: REGISTER ACTIVITY!!!
	//options.RegisterActivity<xxxxActivity>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// Dapr comms
app.UseCloudEvents();
app.MapSubscribeHandler();

// TODO: UseAuthorization needed?
//app.UseAuthorization();

app.MapControllers();

app.Run();
