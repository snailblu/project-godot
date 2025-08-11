using Godot;
using System;

namespace projectgodot
{
    public partial class HeartHealthBar : Control
    {
        [Export] public Texture2D HeartFullTexture { get; set; }
        [Export] public Texture2D HeartHalfTexture { get; set; }
        [Export] public Texture2D HeartEmptyTexture { get; set; }

        private TextureRect[] _heartTextures;
        private HeartHealthBarLogic _logic;

        // 내부적으로 사용할 텍스처들 (Export 프로퍼티나 임시 텍스처)
        private Texture2D _heartFullTexture;
        private Texture2D _heartHalfTexture;
        private Texture2D _heartEmptyTexture;

        public override void _Ready()
        {
            _logic = new HeartHealthBarLogic();
            
            // Export 프로퍼티 또는 임시 텍스처 설정
            InitializeTextures();
            
            // 3개의 하트 TextureRect 노드들 찾기
            _heartTextures = new TextureRect[HeartHealthBarLogic.TOTAL_HEARTS];
            
            for (int i = 0; i < HeartHealthBarLogic.TOTAL_HEARTS; i++)
            {
                string heartPath = $"Heart{i + 1}";
                _heartTextures[i] = GetNode<TextureRect>(heartPath);
                
                if (_heartTextures[i] != null)
                {
                    _heartTextures[i].Texture = _heartFullTexture;
                }
                else
                {
                    GD.PrintErr($"[HeartHealthBar] ERROR: {heartPath} 노드를 찾을 수 없습니다!");
                }
            }
        }

        public void UpdateHearts(int currentHealth, int maxHealth)
        {
            if (_heartTextures == null)
            {
                GD.PrintErr("[HeartHealthBar] ERROR: _heartTextures 배열이 null입니다!");
                return;
            }
            
            var heartStates = _logic.CalculateHeartStates(currentHealth, maxHealth);
            
            for (int i = 0; i < _heartTextures.Length && i < heartStates.Length; i++)
            {
                if (_heartTextures[i] != null && IsInstanceValid(_heartTextures[i]))
                {
                    var newTexture = GetTextureForState(heartStates[i]);
                    if (newTexture != null)
                    {
                        _heartTextures[i].Texture = newTexture;
                    }
                }
                else if (_heartTextures[i] != null && !IsInstanceValid(_heartTextures[i]))
                {
                    // disposed된 노드는 무시하고 계속 진행 (재시작 후 정상적인 현상)
                }
                else
                {
                    GD.PrintErr($"[HeartHealthBar] ERROR: _heartTextures[{i}]가 null입니다!");
                }
            }
        }

        private Texture2D GetTextureForState(HeartState state)
        {
            var texture = state switch
            {
                HeartState.Full => _heartFullTexture,
                HeartState.Half => _heartHalfTexture,
                HeartState.Empty => _heartEmptyTexture,
                _ => _heartEmptyTexture
            };
            
            if (texture == null)
            {
                GD.PrintErr($"[HeartHealthBar] ERROR: {state} 상태의 텍스처가 null입니다!");
            }
            
            return texture;
        }

        private void InitializeTextures()
        {
            // Export 프로퍼티가 설정되어 있으면 사용, 그렇지 않으면 임시 텍스처 생성
            _heartFullTexture = HeartFullTexture ?? CreateTemporaryTexture(Colors.Red);
            _heartHalfTexture = HeartHalfTexture ?? CreateTemporaryTexture(Colors.Yellow);
            _heartEmptyTexture = HeartEmptyTexture ?? CreateTemporaryTexture(Colors.Gray);
        }

        private Texture2D CreateTemporaryTexture(Color color)
        {
            // 임시 텍스처 생성 (Export 프로퍼티가 설정되지 않은 경우에만 사용)
            var image = Image.CreateEmpty(32, 32, false, Image.Format.Rgba8);
            image.Fill(color);
            return ImageTexture.CreateFromImage(image);
        }
    }
}