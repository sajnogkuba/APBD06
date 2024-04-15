using System.Data.SqlClient;
using APBD06.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.MapGet("animals/", (IConfiguration configuration, string? orderBy = "") =>
{
    var animals = new List<GetAllAnimalsResponse>();
    using var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
    var allowedOrderBy = new List<string?> { "IdAnimal", "Name", "Description", "Category", "Area" };
    var sqlText = allowedOrderBy.Contains(orderBy) ? $"SELECT * FROM Animal ORDER BY {orderBy}" :
        "SELECT * FROM Animal ORDER BY Name";
        

    var sqlCommand = new SqlCommand(sqlText, sqlConnection);
    sqlCommand.Connection.Open();
    var sqlDataReader = sqlCommand.ExecuteReader();
    while (sqlDataReader.Read())
    {
        animals.Add(new GetAllAnimalsResponse(
            sqlDataReader.GetInt32(0),
            sqlDataReader.GetString(1),
            sqlDataReader.IsDBNull(2) ? null : sqlDataReader.GetString(2),
            sqlDataReader.GetString(3),
            sqlDataReader.GetString(4)));

    }
    return Results.Ok(animals);
});

app.MapPost("animals/", (IConfiguration configuration, CreateAnimalRequest request) =>
{
    using var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
    var sqlCommand = new SqlCommand(
        "INSERT INTO Animal (Name, Description, Category, Area) VALUES (@name, @desc, @category, @area)",
        sqlConnection);
    sqlCommand.Parameters.AddWithValue("@name", request.Name);
    sqlCommand.Parameters.AddWithValue("@desc", request.Description);
    sqlCommand.Parameters.AddWithValue("@category", request.Category);
    sqlCommand.Parameters.AddWithValue("@area", request.Area);
    sqlCommand.Connection.Open();
    sqlCommand.ExecuteNonQuery();

    return Results.Created("", null);
});
app.Run();