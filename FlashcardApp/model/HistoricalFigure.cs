namespace FlashcardApp.Model
{
    public sealed class HistoricalFigure : ITopic
    {
        public int Number { get; set; } // TODO: This should be ID
        public string Name { get; set; } = string.Empty;
        public string[] Conflicts { get; set; } = [];
        public string[] Occupation { get; set; } = [];
        public string CauseOfDeath { get; set; } = string.Empty;
    }
}
