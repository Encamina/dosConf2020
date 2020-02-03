namespace AzureCognitiveSearch.Shared.Model.Enums
{
    public enum ExpirationTime
    {
        Minute = 1,
        Hour = Minute * 60,
        Day = Hour * 24,
        Week = Day * 7,
        Month = Day * 30,
        Year = Day * 365,
        None = -1
    }
}