using JiraProject.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using JiraProject.Business.Abstract;
using JiraProject.Business.Concrete;
using JiraProject.DataAccess.Concrete;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı bağlantısını projeye tanıtıyoruz
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<JiraProjectDbContext>(options =>
    options.UseSqlServer(connectionString));

// === BAĞIMLILIKLARI (DEPENDENCIES) SİSTEME TANITIYORUZ ===
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IIssueService, IssueManager>();
builder.Services.AddScoped<IProjectService, ProjectManager>();
builder.Services.AddScoped<IUserService, UserManager>();


// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Enum'ları metin olarak kabul etmesi (veya metne çevirmesi) için bu ayarı ekliyoruz.
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
