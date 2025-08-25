using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoApi.Context;
using TodoApi.Model;

var builder = WebApplication.CreateBuilder(args);

// Add DI - AddSevice 
builder.Services.AddDbContext<TodoDb>(op => op.UseInMemoryDatabase("TodoList"));



// Adiciona suporte ao Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = " Minimal API",
        Version = "v1",
        Description = "Minimal API"
    });
});

var app = builder.Build();

// Configure pipiline - UsedMetho


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha Minimal API v1");
    c.RoutePrefix = string.Empty; // abre o swagger direto em "/"
});


app.MapGet("/todoitems", async (TodoDb db) =>
    await db.TodoItem.ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
    await db.TodoItem.FindAsync(id));


app.MapPost("/todoitems", async (TodoItem todo, TodoDb db) =>
{
    await db.TodoItem.AddAsync(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{todo.Id}",todo);
});

app.MapPut("/todoitems/{id}", async (int id,TodoItem inputTodo, TodoDb db) =>
{
    var todo = await db.TodoItem.FindAsync(id);

    if (todo == null)  return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;
    await db.SaveChangesAsync();
    return Results.NoContent();
});


app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    var todo = await db.TodoItem.FindAsync(id);

    if (todo != null)
    {
        db.Remove(todo); 
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});



app.Run();
   
