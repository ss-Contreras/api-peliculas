using ApiPeliculas.Data;
using ApiPeliculas.PeliculasMapper;
using ApiPeliculas.Repositorio;
using ApiPeliculas.Repositorio.IRepositorio;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
                opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));

// Soporte para Autenticación con .NET Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

// Soporte para Cache
var apiVersioningBuilder = builder.Services.AddApiVersioning(opcion =>
{
    opcion.AssumeDefaultVersionWhenUnspecified = true;
    opcion.DefaultApiVersion = new ApiVersion(1, 0);
    opcion.ReportApiVersions = true;
    //opcion.ApiVersionReader = ApiVersionReader.Combine(
    //    new QueryStringApiVersionReader("api-version") //?api-version=1.0
    //    //new HeaderApiVersionReader("X-Version")
    //    //new MediaTypeApiVersionReader("Ver");
    //);
});

apiVersioningBuilder.AddApiExplorer(
    opciones =>
    {
        opciones.GroupNameFormat = "'v'VVV";
        opciones.SubstituteApiVersionInUrl = true;
    }
);

//Agregar los repositorios
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta");

builder.Services.AddApiVersioning();

//Agregar el AutoMapper
builder.Services.AddAutoMapper(typeof(PeliculasMapper));

//Configuar la Autenticacion
builder.Services.AddAuthentication(
        x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    ).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });



builder.Services.AddControllers(opcion =>
{
    //Cache profile, Cache Global
    opcion.CacheProfiles.Add("PorDefecto30Segundos", new CacheProfile() { Duration = 30 });
}); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition( "Bearer", new OpenApiSecurityScheme
        {
            Description = 
            "Autenticación JWT usando el esquema Bearer. \r\n\r\n " +
            "Ingresa la palabra 'Bearer' seguido en un [espacio] y después su token en el campo de abajo. \r\n\r\n" +
            "Ejemplo: \"Bearer tkljk125jhhk\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer"
        }); 
        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        }); 
        options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1.0",
                Title = "Peliculas Api Versión 1",
                Description = "Api de Peliculas",
                TermsOfService = new Uri("https://localhost:3000"),
                Contact = new OpenApiContact
                {
                    Name = "Sergio",
                    Url = new Uri("https://localhost:3000")
                },
                License = new OpenApiLicense
                {
                    Name = "Licencia Personal",
                    Url = new Uri("https://localhost:3000")
                }
            }
        );
        options.SwaggerDoc("v2", new OpenApiInfo
        {
            Version = "v2.0",
            Title = "Peliculas Api Versión 2",
            Description = "Api de Peliculas",
            TermsOfService = new Uri("https://localhost:3000"),
            Contact = new OpenApiContact
            {
                Name = "Sergio",
                Url = new Uri("https://localhost:3000")
            },
            License = new OpenApiLicense
            {
                Name = "Licencia Personal",
                Url = new Uri("https://localhost:3000")
            }
        }
        );
    }
);


//Soporte para CORS
//Se pueden habilitar: 1-Un dominio, 2-Multiples dominios,
//3-Cualquier dominio (Tener en cuenta seguridad)
//Usamos de ejemplo el dominio: https://localhost:3223, se debe cambiar por el correcto
//Se usa (*) Para todos los dominios
builder.Services.AddCors(p => p.AddPolicy("PoliticaCors", build =>
{
    build.WithOrigins("https://localhost:3223").AllowAnyMethod().AllowAnyHeader();
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opciones =>
    {
        opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiPeliculasV1");
        opciones.SwaggerEndpoint("/swagger/v2/swagger.json", "ApiPeliculasV2");
    });
}

app.UseHttpsRedirection();

//Soporte para CORS
app.UseCors("PoliticaCors");

//Soporte para Auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
