using System;
using System.Collections.Generic;
using Godot;
using projectgodot.Constants;

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
        
        /// <summary>
        /// 웨이브별 좀비 구성을 가져옵니다 (컴포지션 패턴의 힘!)
        /// </summary>
        /// <param name="normalScene">일반 좀비 씬</param>
        /// <param name="runnerScene">빠른 좀비 씬</param>
        /// <param name="tankScene">탱커 좀비 씬</param>
        /// <returns>좀비 씬과 수량의 딕셔너리</returns>
        public Dictionary<PackedScene, int> GetZombieCompositionForWave(PackedScene normalScene, PackedScene runnerScene, PackedScene tankScene)
        {
            var composition = new Dictionary<PackedScene, int>();
            
            // 기본 좀비는 항상 3마리
            if (normalScene != null)
                composition.Add(normalScene, 3);
            
            // 빠른 좀비는 웨이브 번호만큼 (1웨이브=1마리, 2웨이브=2마리...)
            if (runnerScene != null)
                composition.Add(runnerScene, CurrentWaveNumber);
            
            // 탱커 좀비는 5의 배수 웨이브마다 등장 (5웨이브=1마리, 10웨이브=2마리...)
            if (tankScene != null && CurrentWaveNumber % GameConstants.Wave.TANK_ZOMBIE_WAVE_INTERVAL == 0)
                composition.Add(tankScene, CurrentWaveNumber / GameConstants.Wave.TANK_ZOMBIE_WAVE_INTERVAL);
            
            return composition;
        }
    }
}