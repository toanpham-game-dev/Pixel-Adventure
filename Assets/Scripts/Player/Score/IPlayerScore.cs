public interface IPlayerScore
{
    void IncreaseScore(int amount);
    int CurrentScore { get; }
}
