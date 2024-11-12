var builder = DistributedApplication.CreateBuilder(args);

// Dapr Components
var stateStore = builder.AddDaprStateStore("statestore");
var workflowChannel = builder.AddDaprPubSub("workflowchannel");
var warehouseChannel = builder.AddDaprPubSub("warehousechannel");
var paymentChannel = builder.AddDaprPubSub("paymentchannel");

// Adds API project for each service for Aspire orchestration
builder.AddProject<Projects.OrderService_Api>("orderservice-api")
    .WithDaprSidecar()
    .WithReference(stateStore)
    .WithReference(workflowChannel)
    .WithReference(warehouseChannel)
    .WithReference(paymentChannel);

builder.AddProject<Projects.WarehouseService_Api>("warehouseservice-api")
    .WithDaprSidecar()
    .WithReference(workflowChannel)
    .WithReference(warehouseChannel);

builder.AddProject<Projects.PaymentService_Api>("paymentservice-api")
    .WithDaprSidecar()
    .WithReference(workflowChannel)
    .WithReference(paymentChannel);

builder.Build().Run();
