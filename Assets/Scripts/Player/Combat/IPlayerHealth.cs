public interface IPlayerHealth
{
    void IncreaseHealth(int amount);
    void DecreaseHealth(int amount);
    void RestoreHealth();
    int CurrentHealth {  get; }
}
