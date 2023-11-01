using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using netlib.controller;
using netlib.interfaces;
using netlib.records;
using netlib.services;
using netlib.udp;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole();

// Config files
builder.Services.Configure<UDPReceiverOptions>(builder.Configuration.GetSection("UDPReceiver"));
builder.Services.Configure<UDPSenderOptions>(builder.Configuration.GetSection("UDPSender"));
builder.Services.Configure<MainControllerOptions>(builder.Configuration.GetSection("MainController"));

// Injections
builder.Services.AddTransient<IMessageService, MessageService>();
builder.Services.AddTransient<ISender, UDPSender>();
builder.Services.AddTransient<IReceiver, UDPReceiver>();
builder.Services.AddSingleton<MainController>();

using var host = builder.Build();

MainController controller = host.Services.GetRequiredService<MainController>();
controller.Start();

Console.ReadLine();