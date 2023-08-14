using HttpMultipartParser;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text;
using Newtonsoft.Json; // Paquete para handlear jsonconvert

using Test;
using Test.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", builder =>
    {
        builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("https://localhost:44426");
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("CORSPolicy");

Db.InitializeDatabase();

app.MapPost("/file", async  (HttpContext context) => {
    var content = await MultipartFormDataParser.ParseAsync(context.Request.Body, context.Request.GetMultipartBoundary());

    var file = content.Files.First(); // Traigo el archivo de la peticion
    string fileName = file.FileName;

    Stream data = file.Data; // Vector con la data
    byte[] arrayBytes = new byte[data.Length];
    await data.ReadAsync(arrayBytes); // Llena el array de bytes
    var dataTxt = Encoding.Default.GetString(arrayBytes); // Devuelve el array como el texto

    var splitted = dataTxt.Split("\n"); // Divido por lineas

    List<Row> arrayRows = new List<Row>();
    
    foreach (var line in splitted)
    {
        var spllitedLine = line.Split('#'); // Divido por elementos siguientes por #
        var cleanedSplittedLine = spllitedLine.Select(s => s.Trim('\r')); // Eliminar \r de cada elemento

        Row row = new Row(cleanedSplittedLine.ElementAt(0), Int32.Parse(cleanedSplittedLine.ElementAt(1)) , cleanedSplittedLine.ElementAt(2));
        
        arrayRows.Add(row);
    }
    
    List<Row> singleRowList = new List<Row>();

    foreach(var row in arrayRows)
    {
        singleRowList.Add(row);
    }

    Entry newEntrie = new Entry(count: singleRowList.Count, timestamp: DateTime.Now, rows: singleRowList);

    var insertedId = Db.InsertInfo(newEntrie);

    newEntrie.setId(insertedId);

    return JsonConvert.SerializeObject(newEntrie);

}).WithTags("Post .txt File");

app.MapGet("/entries/{id}", (HttpRequest request) =>
{
    int id = Int32.Parse((string)request.RouteValues["id"]);

    Entry entrie = Db.GetInfoById(id);

    // Manejo de posibles errores

    if (entrie.Count > 0)
    {
        var responseObj = new
        {
            Status = 200,
            Message = "The Entry was successfully found",
            Entrie = entrie
        };
        return Results.Json(responseObj);
    }
    else
    {
        var responseObj = new
        {
            Status = 404,
            Message = "The Entry has not been found"
        };

        return Results.Json(responseObj);
    }

}).WithTags("Get Entrie by id");

app.MapGet("/entries", (HttpRequest request) =>
{

    List<EntryResume> entries = Db.GetAll();

    var serializedEntries = JsonConvert.SerializeObject(entries);

    return serializedEntries;

}).WithTags("Get all Entries");

app.MapDelete("/entries/{id}", (HttpRequest request) =>
{
    int id = int.Parse((string)request.RouteValues["id"]);

    object deletedEntry = Db.DeleteInfoById(id);

    //Manejo de posibles errores

    if (deletedEntry != null)
    {
        var responseObj = new
        {
            Status = 200,
            Message = "The Entry was successfully deleted",
            DeletedEntrie = deletedEntry
        };
        return Results.Json(responseObj);
    }
    else
    {
        var responseObj = new
        {
            Status = 404,
            Message = "The Entry has not been found"
        };

        return Results.Json(responseObj);
    }

}).WithTags("Delete Entrie");

app.Run();