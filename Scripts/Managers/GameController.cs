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
        private PackedScene _runnerZombieScene;
        private PackedScene _tankZombieScene;
        
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

        public void SetRunnerZombieScene(PackedScene runnerZombieScene)
        {
            _runnerZombieScene = runnerZombieScene;
        }

        public void SetTankZombieScene(PackedScene tankZombieScene)
        {
            _tankZombieScene = tankZombieScene;
        }

        public void StartWave(List<Vector2> spawnPositions)
        {
            if (spawnPositions == null || spawnPositions.Count == 0)
            {
                throw new ArgumentException("스폰 위치가 필요합니다", nameof(spawnPositions));
            }

            _waveManager.StartNextWave();
            
            // 새로운 좀비 구성 시스템이 활성화된 경우 (모든 씬이 설정된 경우)
            if (_zombieScene != null && _runnerZombieScene != null && _tankZombieScene != null)
            {
                // 웨이브별 좀비 구성 가져오기 (컴포지션 패턴!)
                var zombieComposition = _waveManager.GetZombieCompositionForWave(_zombieScene, _runnerZombieScene, _tankZombieScene);
                
                // 총 스폰할 좀비 수 계산
                ZombiesRemainingInWave = 0;
                foreach (var count in zombieComposition.Values)
                {
                    ZombiesRemainingInWave += count;
                }

                // 각 종류의 좀비들을 스폰하기
                Random random = new Random();
                int spawnIndex = 0;
                
                foreach (var (zombieScene, count) in zombieComposition)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var baseSpawnPosition = spawnPositions[spawnIndex % spawnPositions.Count];
                        
                        // 랜덤 오프셋 적용 (반경 50픽셀 내에서)
                        float offsetX = (float)(random.NextDouble() * 100 - 50); // -50 ~ 50
                        float offsetY = (float)(random.NextDouble() * 100 - 50); // -50 ~ 50
                        var spawnPosition = baseSpawnPosition + new Vector2(offsetX, offsetY);
                        
                        _zombieSpawner.SpawnZombie(zombieScene, spawnPosition);
                        spawnIndex++;
                    }
                }
            }
            else
            {
                // 기존 단순 시스템 (테스트 호환성)
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