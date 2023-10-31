using ChatApp;
using ChatApp.Business.ServiceInterfaces;
using ChatApp.Context;
using ChatApp.Infrastructure.ServiceImplementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

// Added DbContext file
builder.Services.AddDbContext<ChatAppContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDbContext<ArgusChatContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Add Dependency injection 
builder.Services.AddSignalR(options => { options.EnableDetailedErrors = true; });

// Added Authentication 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = builder.Configuration["Jwt:Issuer"],
                     ValidAudience = builder.Configuration["Jwt:Audience"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                 };

                // options.SecurityTokenValidators.Clear();
                 
             }
             
);

// Adding Cors Policy to allow any origin to access our api's
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    },
                });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Please insert Subscription Reference with Bearer into field",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Chat App API", Version = "v1" });
});

// Service Registration
builder.Services.AddScoped<IProfileService,ProfileService>();
builder.Services.AddScoped<IChatService, ChatServices>();
builder.Services.AddScoped<IGroupService,GroupServices>();
// In production, the Angular files will be served from this directory

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "MainApp/dist";
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


// app.UseSwagger();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseDefaultFiles();
if (!app.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();
}
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("Default", "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapHub<Chathub>("/Chathub", o => o.Transports = HttpTransportType.WebSockets);
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseWebSockets();

//app.UseSpa(spa =>
//{
//    spa.Options.SourcePath = "MainApp";

//    if (env.IsDevelopment())
//    {
//        spa.UseAngularCliServer(npmScript: "start");
//    }
//});

app.Run();
