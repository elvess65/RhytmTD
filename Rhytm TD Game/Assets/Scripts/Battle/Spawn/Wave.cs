namespace RhytmTD.Battle.Spawn
{
    public class Wave
    {
        public int ID { get; }             //ID волны
        public int EnemiesAmount { get; }       //Количество врагов в волне
        public int DurationAttackTicks { get; } //Длительность волны (в тиках)
        public int DurationRestTicks { get; }   //Длительность отдыха после создания всех врагов до начала следующей волны (в тиках)

        public Wave(int id, int enemiesAmount, int durationAttackTicks, int durationRestTicks)
        {
            ID = id;
            EnemiesAmount = enemiesAmount;
            DurationAttackTicks = durationAttackTicks;
            DurationRestTicks = durationRestTicks;
        }

        public override string ToString()
        {
            return $"Wave {ID}. Enemies: {EnemiesAmount}. Attack Ticks: {DurationAttackTicks}. Rest Ticks: {DurationRestTicks}";
        }
    }
}
