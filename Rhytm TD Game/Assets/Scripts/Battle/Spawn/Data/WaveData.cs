namespace RhytmTD.Battle.Spawn.Data
{
    public class WaveData
    {
        public int ID { get; }                      //ID волны
        public int EnemiesAmount { get; }           //Количество врагов в волне
        public int ChunksAmount { get; }            //Количество пачек в волне
        public int DurationRestTicks { get; }       //Длительность отдыха после создания всех врагов до начала следующей волны (в тиках)
        public int DelayBetweenChunksTicks { get; } //Задержка между спауном пачек (в тиках)

        public WaveData(int id, int enemiesAmount, int chunksAmount, int durationRestTicks, int delayBetweenChunks)
        {
            ID = id;
            EnemiesAmount = enemiesAmount;
            ChunksAmount = chunksAmount;
            DurationRestTicks = durationRestTicks;
            DelayBetweenChunksTicks = delayBetweenChunks;
        }

        public override string ToString()
        {
            return $"Wave {ID}. EnemiesAmount: {EnemiesAmount}. ChunksAmount: {ChunksAmount}. BetweenChunks: {DelayBetweenChunksTicks}. RestTicks: {DurationRestTicks}";
        }
    }
}
