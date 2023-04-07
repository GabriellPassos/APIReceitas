using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReitasAPI.Data;
using ReitasAPI.Models;
using ReitasAPI.Services;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<BuidRecipeService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<RecipeService>();
builder.Services.AddTransient<Ingredient_UnitMesureService>();
builder.Services.AddTransient<UnitMesureService>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddTransient<FilterTagService>();
builder.Services.AddTransient<Recipe_FilterTagsService>();
builder.Services.AddTransient<PictureService>();


//builder.Services.AddTransient<CarregarImagemService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddCors(policy => policy.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("https://gabriellpassos.github.io", "http://127.0.0.1:5500").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));

builder.Services.AddDbContext<RecipeContext>(options => options.UseSqlServer("Data Source=mysql-receitasapi;Initial Catalog=ReceitasDB;Persist Security Info=True;User Id=sa; Password=S#arada123;TrustServerCertificate=True"));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    var key = Encoding.ASCII.GetBytes("8YDpQbJLIS0ea9Bpb1VulQsOppUSuk10BRh7cceyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9eyJzd");
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>

        {
            /*A cada requisicao feita por um cliente, o valor do cookie "token" recebido ser�
            inserido de forma alternativa (context.Token) para a valida��o*/
            context.Token = context.Request.Cookies["token"];
            return Task.CompletedTask;
        }
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
app.UseCors("corspolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
