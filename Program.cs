using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StopApi.DbContexto;
using StopApi.Services;
using StopApi.MiddlerwareAutenticacao;
using Microsoft.Extensions.Configuration;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EstacionamentoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLEXPRESS")));

builder.Services.AddScoped<CalculoEstacionamento>(provider => 
    new CalculoEstacionamento(10.0m)); 

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "StopApi", Version = "v1" });

    options.AddSecurityDefinition("basicAuth", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Scheme = "basic",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Description = "Informe suas credenciais usando o formato: Basic {base64(username:password)}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basicAuth"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StopApi v1"));
}

app.UseHttpsRedirection();
app.UseCors("PermitirTudo");
app.UseMiddleware<AutenticacaoMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
