using MessageService.Api.BL;
using MessageService.Api.DAL;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MessageService API", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddSingleton<IMessageDAL, MessageDAL>();
builder.Services.AddSingleton<IMessageBL, MessageBL>();
builder.Services.AddSingleton<IWebSocketBL, WebSocketBL>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MessageService API V1");
});

app.UseWebSockets();

app.UseMvc();


await app.RunAsync();
