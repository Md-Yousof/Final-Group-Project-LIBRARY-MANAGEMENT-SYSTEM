using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Interfaces;
using LibraryAPI_R53_A.Core.Repositories;
using LibraryAPI_R53_A.Helpers;
using LibraryAPI_R53_A.Persistence;
using LibraryAPI_R53_A.Persistence.Repositories;
using LibraryAPI_R53_A.Persistence.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net.NetworkInformation;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHostedService<ApprovalExpirationService>();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Authentication", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authentication header using it",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
//CORS
builder.Services.AddCors(options =>
    options.AddPolicy(name: "AngularPolicy",
        cfg => {
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
            cfg.WithOrigins(builder.Configuration["AllowedCORS"]);
        }));


builder.Services.AddDbContext<ApplicationDbContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


#region Step 04. R-Step 03
// able to Inject JWTService class inside my Controller
builder.Services.AddScoped<JWTService>();
builder.Services.AddScoped<ContextSeedService>();
#endregion 

#region setp 01
// --------------- 01 ----------------
// defining identityCOre Service
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    // password Configuration
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddRoles<IdentityRole>() //  able to add roles
    .AddRoleManager<RoleManager<IdentityRole>>() // able to make use of RoleManager
    .AddEntityFrameworkStores<ApplicationDbContext>() // my context
    .AddSignInManager<SignInManager<ApplicationUser>>() // make use of signIn manager
    .AddUserManager<UserManager<ApplicationUser>>() // make use of userManager to create users
    .AddDefaultTokenProviders(); // able to create tokens for email confimation
// ------------ End 01 -------------------
#endregion

#region Step 05
// Authenticate users using this JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // validate the token based on the key we have provided inside appsetting.deleopment.json JWT:Key
            ValidateIssuerSigningKey = true,
            // the issuer singing key based on JWT:Key
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            // the issuer which in here is the api project url we r using
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            // validate the issuer (who ever is issiung the JWT) 
            ValidateIssuer = true,
            // (angular side )
            ValidateAudience = false
        };
    });
#endregion 

//DI
builder.Services.AddScoped<IPublisher, PublisherRepository>();
builder.Services.AddScoped<ISubscriptionPlan, SubsPlanRepository>();
builder.Services.AddScoped<ICategory, CategoryRepository>();
builder.Services.AddScoped<IBook, BookRepository>();
builder.Services.AddScoped<IAuthor, AuthorRepository>();
builder.Services.AddScoped<IBorrowBook, BorrowedBookRepository>();
builder.Services.AddScoped<IBookCopy, BookCopyRepository>();
builder.Services.AddScoped<IInvoice, InvoiceRepository>();
builder.Services.AddScoped<IRepository<BookFloor>, BookFloorRepository>();
builder.Services.AddScoped<IRepository<Shelf>, ShelfRepository>();
builder.Services.AddScoped<IRepository<Subcategory>, SubcategoryRepository>();
builder.Services.AddScoped<ISubscriptionUser, SubscriptionUserRepository>();

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    opt.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));
    opt.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});


builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//for files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "uploads")),
    RequestPath="/uploads"
});

// Step 06   -- app.UseAuthentication();
app.UseAuthentication();
app.UseAuthorization();
//Cors
app.UseCors("AngularPolicy");

app.MapControllers();

#region  Context Seed. R-step 04
using var scope = app.Services.CreateScope();
try
{
    var contextSeedService = scope.ServiceProvider.GetService<ContextSeedService>();
    await contextSeedService.InitializeContextAsync();
}
catch(Exception ex)
{
    var logger = scope.ServiceProvider.GetServices<ILogger<Program>>();
    //logger.logError(ex.Message, "Failed to initialilze and seed the database");
}

#endregion

app.Run();
