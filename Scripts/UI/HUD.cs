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

	}

	public partial class HUD : CanvasLayer
	{
		// UI 노드들에 대한 참조
		private HeartHealthBar _heartHealthBar;
		private Label _scoreLabel;
		private Label _waveLabel;
		private HUDLogic _logic;

		public override void _Ready()
		{
			_logic = new HUDLogic();

			// 씬의 노드들을 찾아서 참조 저장
			_heartHealthBar = GetNode<HeartHealthBar>("UIContainer/HeartHealthBar");
			_scoreLabel = GetNode<Label>("UIContainer/ScoreLabel");
			_waveLabel = GetNode<Label>("UIContainer/WaveLabel");

			// 이벤트 버스 구독
			var events = EventsHelper.GetEventsNode(this);
			if (events != null)
			{
				events.PlayerHealthChanged += OnPlayerHealthChanged;
				events.ScoreChanged += OnScoreChanged;
				events.WaveChanged += OnWaveChanged;
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

	}
}
