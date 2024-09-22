using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace StopApi.MiddlerwareAutenticacao
{
    public class AutenticacaoMiddleware
    {
        private readonly RequestDelegate _next;

        public AutenticacaoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var username = "admin";
            var password = "senha123";

            // Permitir acesso público para GET /api/Veiculos e POST /api/Veiculos
            if (context.Request.Path.StartsWithSegments("/api/Veiculos") && 
                (context.Request.Method == "GET" || context.Request.Method == "POST"))
            {
                await _next(context);
                return;
            }

            // Para DELETE, requer autenticação
            if (context.Request.Path.StartsWithSegments("/api/Veiculos") && context.Request.Method == "DELETE")
            {
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

                if (authHeader != null)
                {
                    var authHeaderValue = authHeader.ToString().Split(' ');

                    if (authHeaderValue.Length == 2 && authHeaderValue[0] == "Basic")
                    {
                        var encodedCredentials = authHeaderValue[1];
                        var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                        var credentials = decodedCredentials.Split(':');

                        if (credentials.Length == 2 && credentials[0] == username && credentials[1] == password)
                        {
                            // Usuário autenticado
                            await _next(context);
                            return;
                        }
                    }
                }

                
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Credenciais inválidas");
                return;
            }

            
            await _next(context);
        }
    }
}
