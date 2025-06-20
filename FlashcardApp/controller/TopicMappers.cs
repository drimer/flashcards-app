namespace FlashcardApp.Controller;


public static class PokemonTopicMapper
{
    public static Model.Pokemon ToPokemon(this Dto.TopicDto topic)
    {
        return new Model.Pokemon
        {
            Number = topic.Id,
        };
    }

    public static Dto.TopicDto ToTopicDto(this Model.Pokemon pokemon)
    {
        return new Dto.TopicDto
        {
            Id = pokemon.Number,
        };
    }
}


public static class HistoricalFigureTopicMapper
{
    public static Model.HistoricalFigure ToHistoricalFigure(this Dto.TopicDto topic)
    {
        return new Model.HistoricalFigure
        {
            Number = topic.Id,
        };
    }

    public static Dto.TopicDto ToTopicDto(this Model.HistoricalFigure historicalFigure)
    {
        return new Dto.TopicDto
        {
            Id = historicalFigure.Number,
        };
    }
}

