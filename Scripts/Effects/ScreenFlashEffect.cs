using Godot;
using projectgodot;
using projectgodot.Constants;

namespace projectgodot
{
    public partial class ScreenFlashEffect : CanvasLayer
    {
        private ColorRect flashRect;
        private bool isFlashing = false;

        public override void _Ready()
        {
            Layer = 100; // 다른 UI 위에 표시되도록 높은 레이어 설정
            
            // 화면 전체를 덮는 ColorRect 생성
            flashRect = new ColorRect();
            flashRect.Color = new Color(1, 1, 1, 0); // 완전 투명한 흰색으로 시작
            // 수동으로 전체 화면 설정
            flashRect.AnchorLeft = 0;
            flashRect.AnchorTop = 0;
            flashRect.AnchorRight = 1;
            flashRect.AnchorBottom = 1;
            flashRect.OffsetLeft = 0;
            flashRect.OffsetTop = 0;
            flashRect.OffsetRight = 0;
            flashRect.OffsetBottom = 0;
            AddChild(flashRect);
            
            // 이벤트 연결
            var events = EventsHelper.GetEventsNode(this);
            if (events != null)
            {
                events.ScreenFlashRequested += OnScreenFlashRequested;
            }
        }

        private void OnScreenFlashRequested()
        {
            // 이미 플래시 진행 중이면 무시
            if (isFlashing)
                return;
                
            isFlashing = true;
            
            // 새로운 Tween 생성
            var tween = CreateTween();
            
            // 플래시 애니메이션
            var flashColor = new Color(1, 1, 1, GameConstants.DeathEffect.FLASH_INTENSITY);
            var transparentColor = new Color(1, 1, 1, 0);
            
            // 즉시 플래시 색상으로 변경
            flashRect.Color = flashColor;
            
            // 서서히 투명하게 페이드 아웃
            tween.TweenProperty(flashRect, "color", transparentColor, GameConstants.DeathEffect.FLASH_DURATION);
            
            // 애니메이션 완료 후 플래그 재설정
            tween.TweenCallback(Callable.From(() => {
                isFlashing = false;
            }));
        }

        public override void _ExitTree()
        {
            var events = EventsHelper.GetEventsNodeSafe(this);
            if (events != null)
            {
                events.ScreenFlashRequested -= OnScreenFlashRequested;
            }
        }
    }
}