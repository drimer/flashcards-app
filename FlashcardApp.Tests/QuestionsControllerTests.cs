using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;
using FlashcardApp.Controller;
using FlashcardApp.Model;
using FlashcardApp.Service;
using FlashcardApp.Client;
using Moq;

namespace FlashcardApp.Tests;

public class QuestionsControllerNewTests
{
    private readonly Mock<IPokemonApiService> _pokemonApiServiceMock;
    private readonly Mock<IHistoricalFigureApiClient> _historicalFigureApiClientMock;

    public QuestionsControllerNewTests()
    {
        _pokemonApiServiceMock = new Mock<IPokemonApiService>();
        _historicalFigureApiClientMock = new Mock<IHistoricalFigureApiClient>();
    }

    [Fact]
    public async Task GetNewQuestion_WithInvalidCount_ReturnsError()
    {
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString("?count=0");
        string topic = "pokemon";

        var controller = CreateController();
        controller.ControllerContext.HttpContext = context;

        var result = await controller.GetNewQuestion(topic);

        Assert.NotNull(result);
        Assert.Equal("Count must be at least 1.", result.Error);
    }

    [Fact]
    public async Task GetNewQuestion_WithValidCount_Pokemon_ReturnsQuestions()
    {
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString("?count=1");
        string topic = "pokemon";

        var pokemon = new Pokemon { Number = 1, Name = "bulbasaur" };
        _pokemonApiServiceMock
            .Setup(service => service.GetRandomPokemonAsync())
            .ReturnsAsync(pokemon);

        var controller = CreateController();
        controller.ControllerContext.HttpContext = context;
        var result = await controller.GetNewQuestion(topic);

        Assert.NotNull(result);
        Assert.Null(result.Error);
        Assert.NotNull(result.Questions);
        Assert.Single(result.Questions);
        Assert.Equal("PokemonQuestion", result.Questions[0].Type);
    }

    [Fact]
    public async Task GetNewQuestion_WithValidCount_HistoricalFigure_ReturnsQuestions()
    {
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString("?count=1");
        string topic = "historicalFigure";

        var figure = new HistoricalFigure { Number = 1, Name = "albert einstein" };
        _historicalFigureApiClientMock
            .Setup(client => client.GetRandomHistoricalFigureAsync())
            .ReturnsAsync(figure);

        var controller = CreateController();
        controller.ControllerContext.HttpContext = context;
        var result = await controller.GetNewQuestion(topic);

        Assert.NotNull(result);
        Assert.Null(result.Error);
        Assert.NotNull(result.Questions);
        Assert.Single(result.Questions);
        Assert.Equal("HistoricalFigureQuestion", result.Questions[0].Type);
    }

    [Fact]
    public async Task GetNewQuestion_WithInvalidTopic_ReturnsError()
    {
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString("?count=1");
        string topic = "invalidTopic";

        var controller = CreateController();
        controller.ControllerContext.HttpContext = context;
        var result = await controller.GetNewQuestion(topic);

        Assert.NotNull(result);
        Assert.Equal("Invalid topic.", result.Error);
    }

    [Fact]
    public async Task SubmitAnswer_WithInvalidType_ReturnsError()
    {
        var context = new DefaultHttpContext();
        var request = new Dto.PostAnswerRequestDto
        {
            Question = new Dto.QuestionDto { Type = "UnknownType" },
            Answer = "test",
        };

        var controller = CreateController();
        controller.ControllerContext.HttpContext = context;
        var result = await controller.SubmitAnswer(request);

        Assert.False(result.IsCorrect);
        Assert.Equal("Invalid question type.", result.Message);
    }

    [Fact]
    public async Task SubmitAnswer_PokemonQuestion_CorrectAnswer_ReturnsCorrect()
    {
        var context = new DefaultHttpContext();
        var pokemon = new Pokemon { Number = 1, Name = "bulbasaur", Hp = 1 };
        var request = new Dto.PostAnswerRequestDto
        {
            Question = new Dto.QuestionDto
            {
                Type = "PokemonQuestion",
                Topic = new Dto.TopicDto { Id = 1 },
                Field = "hp",
            },
            Answer = "1",
        };

        _pokemonApiServiceMock
            .Setup(service => service.GetPokemonByNumberAsync(1))
            .ReturnsAsync(pokemon);

        var controller = CreateController();
        controller.ControllerContext.HttpContext = context;
        var result = await controller.SubmitAnswer(request);

        Assert.True(result.IsCorrect);
        Assert.Equal("Correct!", result.Message);
    }

    [Fact]
    public async Task SubmitAnswer_PokemonQuestion_IncorrectAnswer_ReturnsIncorrect()
    {
        var context = new DefaultHttpContext();
        var pokemon = new Pokemon { Number = 1, Name = "bulbasaur" };
        var request = new Dto.PostAnswerRequestDto
        {
            Question = new Dto.QuestionDto
            {
                Type = "PokemonQuestion",
                Topic = new Dto.TopicDto { Id = 1 },
                Field = "hp",
            },
            Answer = "100",
        };

        _pokemonApiServiceMock
            .Setup(service => service.GetPokemonByNumberAsync(1))
            .ReturnsAsync(pokemon);

        var controller = CreateController();
        controller.ControllerContext.HttpContext = context;
        var result = await controller.SubmitAnswer(request);

        Assert.False(result.IsCorrect);
        Assert.Equal("Incorrect.", result.Message);
    }

    [Fact]
    public async Task SubmitAnswer_HistoricalFigureQuestion_CorrectAnswer_ReturnsCorrect()
    {
        var context = new DefaultHttpContext();
        var figure = new HistoricalFigure { Number = 1, Name = "albert einstein", Conflicts = new[] { "World War II" } };
        var request = new Dto.PostAnswerRequestDto
        {
            Question = new Dto.QuestionDto
            {
                Type = "HistoricalFigureQuestion",
                Topic = new Dto.TopicDto { Id = 1 },
                Field = "Conflicts",
            },
            Answer = "World War II",
        };

        _historicalFigureApiClientMock
            .Setup(client => client.GetHistoricalFigureByNumberAsync(1))
            .ReturnsAsync(figure);

        var controller = CreateController();
        controller.ControllerContext.HttpContext = context;
        var result = await controller.SubmitAnswer(request);

        Assert.True(result.IsCorrect);
        Assert.Equal("Correct!", result.Message);
    }

    [Fact]
    public async Task SubmitAnswer_HistoricalFigureQuestion_IncorrectAnswer_ReturnsIncorrect()
    {
        var context = new DefaultHttpContext();
        var figure = new HistoricalFigure { Number = 1, Name = "albert einstein", Conflicts = new[] { "World War II" } };
        var request = new Dto.PostAnswerRequestDto
        {
            Question = new Dto.QuestionDto
            {
                Type = "HistoricalFigureQuestion",
                Topic = new Dto.TopicDto { Id = 1 },
                Field = "Conflicts",
            },
            Answer = "wronganswer",
        };

        _historicalFigureApiClientMock
            .Setup(client => client.GetHistoricalFigureByNumberAsync(1))
            .ReturnsAsync(figure);

        var controller = CreateController();
        controller.ControllerContext.HttpContext = context;
        var result = await controller.SubmitAnswer(request);

        Assert.False(result.IsCorrect);
        Assert.Equal("Incorrect.", result.Message);
    }

    private QuestionsController CreateController()
    {
        return new QuestionsController(_pokemonApiServiceMock.Object, _historicalFigureApiClientMock.Object);
    }
}
