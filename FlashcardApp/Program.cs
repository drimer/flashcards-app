
namespace FlashcardApp;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

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

        app.MapGet("/question/{topic}", Controller.QuestionsController.GetNewQuestion)
            .WithName("GetNewQuestion")
            .WithOpenApi();
        app.MapPost("/question/answer", Controller.QuestionsController.SubmitAnswer)
            .WithName("SubmitAnswer")
            .WithOpenApi()
            .Accepts<Dto.PostAnswerRequestDto>("application/json")
            .Produces<Dto.PostAnswerResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        app.Run();
    }
}
