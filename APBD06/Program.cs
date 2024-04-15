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
    using (var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default")))
    {
        string sqlText;
        var allowedOrderBy = new List<string?> { "IdAnimal", "Name", "Description", "Category", "Area" };
        if (allowedOrderBy.Contains(orderBy))
        {
            sqlText = $"SELECT * FROM Animal ORDER BY {orderBy}";
        }
        else
        {
            sqlText = "SELECT * FROM Animal ORDER BY Name";
        }
        

        var sqlCommand = new SqlCommand(sqlText, sqlConnection);
        sqlCommand.Parameters.AddWithValue("@orderBY", orderBy);
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
    }
});
app.Run();