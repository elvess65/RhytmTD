namespace RhytmTD.Battle.Spawn.Data
{
    public class WaveData
    {
        public int ID { get; }                   //ID волны
        public int EnemiesAmount { get; }        //Количество врагов в волне
        public int ChunksAmount { get; }         //Количество пачек в волне
        public int DurationRestTicks { get; }    //Длительность отдыха после создания всех врагов до начала следующей волны (в тиках)
        public int DelayBetweenChunksTicks => 2; //Задержка между спауном пачек (в тиках)

        public WaveData(int id, int enemiesAmount, int chunksAmount, int durationRestTicks)
        {
            ID = id;
            EnemiesAmount = enemiesAmount;
            ChunksAmount = chunksAmount;
            DurationRestTicks = durationRestTicks;
        }

        public override string ToString()
        {
            return $"Wave {ID}. Enemies: {EnemiesAmount}. Attack Ticks: {ChunksAmount}. Rest Ticks: {DurationRestTicks}";
        }
    }
}
