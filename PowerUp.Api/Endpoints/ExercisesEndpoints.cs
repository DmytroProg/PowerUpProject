using Microsoft.AspNetCore.Mvc;
using PowerUp.Application.Services.Exercises;
using PowerUp.Domain.Requests.Exercises;

namespace PowerUp.Api.Endpoints;

public static class ExercisesEndpoints
{
    public static IEndpointRouteBuilder UseExercisesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/exercises");
        
        group.MapGet("/", async ([FromBody] ExercisesRequest request, CancellationToken cancellationToken, ExercisesService service) =>
        {
            request.Limit = 10;
            var exercises = await service.GetExercises(request, cancellationToken);
            return Results.Ok(exercises);
        });

        group.MapPost("/", async ([FromBody] CreateExerciseRequest request, CancellationToken cancellationToken, ExercisesService service) =>
        {
            var exercise = await service.AddExercise(request, cancellationToken);
            return Results.Created($"api/v1/exercises/{exercise.Id}", exercise);
        });

        group.MapGet("{id}", async ([FromRoute] int id, CancellationToken cancellationToken, ExercisesService service) =>
        {
            var exercise = await service.GetById(id, cancellationToken);
            return Results.Ok(exercise);
        })
        .RequireAuthorization();
        
        return app;
    }
}