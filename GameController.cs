using System;
using System.Collections.Generic;
using Godot;

namespace projectgodot
{
    /// <summary>
    /// 게임 흐름 제어를 담당하는 비즈니스 로직 클래스 (테스트 가능)
    /// </summary>
    public class GameController
    {
        public event Action WaveCleared;
        
        private readonly WaveManager _waveManager;
        private readonly IZombieSpawner _zombieSpawner;
        private PackedScene _zombieScene;
        
        public int ZombiesRemainingInWave { get; private set; } = 0;

        public GameController(WaveManager waveManager, IZombieSpawner zombieSpawner)
        {
            _waveManager = waveManager ?? throw new ArgumentNullException(nameof(waveManager));
            _zombieSpawner = zombieSpawner ?? throw new ArgumentNullException(nameof(zombieSpawner));
        }

        public void SetZombieScene(PackedScene zombieScene)
        {
            _zombieScene = zombieScene;
        }

        public void StartWave(List<Vector2> spawnPositions)
        {
            if (spawnPositions == null || spawnPositions.Count == 0)
            {
                throw new ArgumentException("스폰 위치가 필요합니다", nameof(spawnPositions));
            }

            _waveManager.StartNextWave();
            int zombiesToSpawn = _waveManager.GetZombiesToSpawnThisWave();
            ZombiesRemainingInWave = zombiesToSpawn;

            // 좀비들 스폰하기
            Random random = new Random();
            for (int i = 0; i < zombiesToSpawn; i++)
            {
                var baseSpawnPosition = spawnPositions[i % spawnPositions.Count];
                
                // 랜덤 오프셋 적용 (반경 50픽셀 내에서)
                float offsetX = (float)(random.NextDouble() * 100 - 50); // -50 ~ 50
                float offsetY = (float)(random.NextDouble() * 100 - 50); // -50 ~ 50
                var spawnPosition = baseSpawnPosition + new Vector2(offsetX, offsetY);
                
                _zombieSpawner.SpawnZombie(_zombieScene, spawnPosition);
            }
        }

        public void OnZombieDied()
        {
            ZombiesRemainingInWave--;
            
            if (ZombiesRemainingInWave <= 0)
            {
                _waveManager.EndWave();
                WaveCleared?.Invoke();
            }
        }
    }
}