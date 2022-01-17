using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDB>(options =>
{
    options.UseInMemoryDatabase("Todo");
});

builder.Services.AddSingleton<ITokenService>(new TokenService());
builder.Services.AddSingleton<IUserRepositoryService>(new UserRepositoryService());

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT 인증",
        Description = "JWT Bearer token을 입력하세요",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });
});

builder.Services.AddMvc();

var app = builder.Build();

app.MapGet("/todo", async (TodoDB db) => await db.TodoList.ToListAsync())
    .Produces<List<Todo>>(StatusCodes.Status200OK)
    .WithName("GetAllTodoList").WithTags("Getters");

app.MapPost("/todo", async ([FromBody] Todo todo, [FromServices] TodoDB db, HttpResponse response) =>
{
    db.TodoList.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"todo/{todo.Id}", todo);
}).Accepts<Todo>("application/json")
  .Produces<Todo>(StatusCodes.Status201Created).RequireAuthorization();

app.MapPut("/todo", async (int todoId, string title, [FromServices] TodoDB db, HttpResponse response) =>
{
    var todo = db.TodoList.SingleOrDefault(s => s.Id == todoId);

    if (todo == null) return Results.NotFound();

    todo.Title = title;
    await db.SaveChangesAsync();
    return Results.Created("/todo", todo);
});

app.MapGet("/todo/{id}", async (TodoDB db, int id) =>
    await db.TodoList.SingleOrDefaultAsync(s => s.Id == id) is Todo todo ? Results.Ok(todo) : Results.NotFound());

app.MapGet("/todo/search/{query}", (string query, TodoDB db) =>
{
    var todolist = db.TodoList.Where(x => x.Title.ToLower().Contains(query.ToLower())).ToList();
    return todolist.Count > 0 ? Results.Ok(todolist) : Results.NotFound();
}).Produces<List<Todo>>(StatusCodes.Status200OK)
   .Produces(StatusCodes.Status404NotFound); ;

app.MapPost("/login", [AllowAnonymous] async ([FromBody] UserModel userModel, ITokenService tokenService, IUserRepositoryService userRepositoryService, HttpResponse response) =>
{
    var userDto = userRepositoryService.GetUser(userModel);
    if (userDto == null)
    {
        response.StatusCode = 401;
        return;
    }

    var issuer = builder.Configuration["Jwt:Issuer"];
    var audience = builder.Configuration["Jwt:Audience"];
    var key = builder.Configuration["Jwt:Key"];

    var token = tokenService.BuildToken(key, issuer, audience, userDto);

    await response.WriteAsJsonAsync(new { token = token });
    return;

}).Produces(StatusCodes.Status200OK).WithName("Login").WithTags("Accounts");

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        "Root",
        "{action}",
        new { controller = "Home", action = "Index" });
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

public interface ITokenService
{
    string BuildToken(string key, string issuer, string audience, User user);
}

public class TokenService : ITokenService
{
    private TimeSpan ExpiryDuration = new TimeSpan(0, 30, 0);

    public string BuildToken(string key, string issuer, string audience, User user)
    {
        var claims = new[]
        {
             new Claim(ClaimTypes.Name, user.UserName),
             new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
        };

        var mySecret = Encoding.UTF8.GetBytes(key);
        var securityKey = new SymmetricSecurityKey(mySecret);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, audience, claims,
        expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}


public record User(string UserName, string Password);

public record UserModel
{
    public UserModel(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}

public interface IUserRepositoryService
{
    User? GetUser(UserModel model);
}

public class UserRepositoryService : IUserRepositoryService
{
    public List<User> _users = new List<User>()
    {
        new("admin", "1234"),
    };

    public User? GetUser(UserModel model)
      => _users.FirstOrDefault(x => string.Equals(x.UserName, model.UserName) && string.Equals(x.Password, model.Password));
}




public class Todo
{
    [Key]
    public int Id { get; set; }

    public string? Title { get; set; }

    public bool Checked { get; set; }
}

class TodoDB : DbContext
{
    public TodoDB(DbContextOptions<TodoDB> options) : base(options) { }

    public DbSet<Todo> TodoList => Set<Todo>();
}