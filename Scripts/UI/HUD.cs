using Godot;

namespace projectgodot
{
	// 테스트 가능한 순수 로직 클래스
	public class HUDLogic
	{
		public double CalculateHealthPercentage(int currentHealth, int maxHealth)
		{
			if (maxHealth <= 0 || currentHealth < 0) return 0.0;
			return (double)currentHealth / maxHealth * 100.0;
		}

		public string FormatScoreText(int score)
		{
			return $"Score: {score}";
		}

		public string FormatWaveText(int waveNumber)
		{
			return $"Wave: {waveNumber}";
		}

		public double CalculateHungerPercentage(int currentHunger, int maxHunger)
		{
			if (maxHunger <= 0 || currentHunger < 0) return 0.0;
			return (double)currentHunger / maxHunger * 100.0;
		}

		public string FormatHungerText(int currentHunger, int maxHunger)
		{
			return $"Hunger: {currentHunger}/{maxHunger}";
		}

		public bool IsStarving(int currentHunger)
		{
			return currentHunger <= 0;
		}
	}

	public partial class HUD : CanvasLayer
	{
		// UI 노드들에 대한 참조
		private HeartHealthBar _heartHealthBar;
		private Label _scoreLabel;
		private Label _waveLabel;
		private TextureProgressBar _hungerBar;
		private Label _hungerLabel;
		private HUDLogic _logic;

		public override void _Ready()
		{
			_logic = new HUDLogic();

			// 씬의 노드들을 찾아서 참조 저장
			_heartHealthBar = GetNode<HeartHealthBar>("UIContainer/HeartHealthBar");
			_scoreLabel = GetNode<Label>("UIContainer/ScoreLabel");
			_waveLabel = GetNode<Label>("UIContainer/WaveLabel");
			
			// 허기 UI 노드들 (존재하지 않으면 null로 남겨둠)
			_hungerBar = GetNodeOrNull<TextureProgressBar>("UIContainer/HungerBar");
			_hungerLabel = GetNodeOrNull<Label>("UIContainer/HungerLabel");

			// 이벤트 버스 구독
			var events = EventsHelper.GetEventsNode(this);
			if (events != null)
			{
				events.PlayerHealthChanged += OnPlayerHealthChanged;
				events.ScoreChanged += OnScoreChanged;
				events.WaveChanged += OnWaveChanged;
				events.HungerChanged += OnHungerChanged;
				events.StarvationStarted += OnStarvationStarted;
			}
			else
			{
				GD.PrintErr("[HUD] ERROR: Events 노드를 찾을 수 없습니다!");
			}
		}

		// 이벤트 핸들러들
		private void OnPlayerHealthChanged(int currentHealth, int maxHealth)
		{
			UpdateHealth(currentHealth, maxHealth);
		}

		private void OnScoreChanged(int newScore)
		{
			UpdateScore(newScore);
		}

		private void OnWaveChanged(int waveNumber)
		{
			UpdateWave(waveNumber);
		}

		private void OnHungerChanged(int currentHunger, int maxHunger)
		{
			UpdateHunger(currentHunger, maxHunger);
		}

		private void OnStarvationStarted()
		{
			// 굶주림 상태일 때 허기 바 색상 변경 등의 시각적 피드백
			if (_hungerBar != null)
			{
				// 예: 허기 바를 빨간색으로 변경
				_hungerBar.Modulate = Colors.Red;
			}
			
			GD.Print("UI: Player is starving!");
		}

		// UI 업데이트 메서드들
		public void UpdateHealth(int currentHealth, int maxHealth)
		{
			if (_heartHealthBar != null)
			{
				_heartHealthBar.UpdateHearts(currentHealth, maxHealth);
			}
		}

		public void UpdateScore(int score)
		{
			if (_scoreLabel != null)
			{
				_scoreLabel.Text = _logic.FormatScoreText(score);
			}
		}

		public void UpdateWave(int waveNumber)
		{
			if (_waveLabel != null)
			{
				_waveLabel.Text = _logic.FormatWaveText(waveNumber);
			}
		}

		public void UpdateHunger(int currentHunger, int maxHunger)
		{
			// 허기 바 업데이트
			if (_hungerBar != null)
			{
				_hungerBar.Value = _logic.CalculateHungerPercentage(currentHunger, maxHunger);
				
				// 굶주림 상태에 따라 색상 변경
				if (_logic.IsStarving(currentHunger))
				{
					_hungerBar.Modulate = Colors.Red;
				}
				else
				{
					_hungerBar.Modulate = Colors.White; // 기본 색상으로 복원
				}
			}

			// 허기 라벨 업데이트
			if (_hungerLabel != null)
			{
				_hungerLabel.Text = _logic.FormatHungerText(currentHunger, maxHunger);
				
				// 굶주림 상태일 때 텍스트 색상도 변경
				if (_logic.IsStarving(currentHunger))
				{
					_hungerLabel.Modulate = Colors.Red;
				}
				else
				{
					_hungerLabel.Modulate = Colors.White; // 기본 색상으로 복원
				}
			}
		}
	}
}
