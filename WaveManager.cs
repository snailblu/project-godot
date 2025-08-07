using System;

namespace projectgodot
{
    /// <summary>
    /// 웨이브 진행과 관련된 상태를 관리하는 클래스 (비즈니스 로직)
    /// </summary>
    public class WaveManager
    {
        public event Action<int, int> WaveStarted; // (waveNumber, zombiesToSpawn)
        public event Action<int> WaveEnded; // (waveNumber)
        
        public int CurrentWaveNumber { get; private set; } = 0;
        public bool IsWaveActive { get; private set; } = false;

        /// <summary>
        /// 다음 웨이브를 시작합니다
        /// </summary>
        public void StartNextWave()
        {
            CurrentWaveNumber++;
            IsWaveActive = true;
            
            int zombiesToSpawn = GetZombiesToSpawnThisWave();
            WaveStarted?.Invoke(CurrentWaveNumber, zombiesToSpawn);
        }

        /// <summary>
        /// 현재 웨이브에 스폰할 좀비 수를 계산합니다
        /// </summary>
        /// <returns>스폰할 좀비 수</returns>
        public int GetZombiesToSpawnThisWave()
        {
            // 웨이브가 진행될수록 점점 더 많은 좀비가 나오도록 수식을 만듭니다
            // 1웨이브: 5마리, 2웨이브: 7마리, 3웨이브: 9마리...
            return 3 + (CurrentWaveNumber * 2);
        }
        
        /// <summary>
        /// 현재 웨이브를 종료합니다
        /// </summary>
        public void EndWave()
        {
            IsWaveActive = false;
            WaveEnded?.Invoke(CurrentWaveNumber);
        }
    }
}