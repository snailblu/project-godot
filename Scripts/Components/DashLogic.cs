using System;

namespace projectgodot
{
    /// <summary>
    /// 대시 기능의 순수 로직을 담당하는 클래스
    /// Godot에 의존하지 않으므로 단위 테스트 가능
    /// </summary>
    public class DashLogic
    {
        public float DashSpeed { get; set; } = 1000f;
        public float DashDuration { get; set; } = 0.2f;
        public bool IsDashing { get; private set; }
        
        private DateTime _dashStartTime;
        private bool _dashStarted;

        public void StartDash()
        {
            if (IsDashing) return;

            IsDashing = true;
            _dashStarted = true;
            _dashStartTime = DateTime.Now;
        }

        /// <summary>
        /// 대시 상태를 업데이트합니다. 매 프레임 호출해야 합니다.
        /// </summary>
        /// <param name="deltaTime">프레임 간격 시간</param>
        public void Update(float deltaTime)
        {
            if (!_dashStarted) return;

            var elapsed = (float)(DateTime.Now - _dashStartTime).TotalSeconds;
            
            if (elapsed >= DashDuration)
            {
                IsDashing = false;
                _dashStarted = false;
            }
        }

        /// <summary>
        /// 대시 상태를 강제로 종료합니다.
        /// </summary>
        public void EndDash()
        {
            IsDashing = false;
            _dashStarted = false;
        }

        /// <summary>
        /// 남은 대시 시간을 반환합니다.
        /// </summary>
        public float GetRemainingTime()
        {
            if (!_dashStarted) return 0f;
            
            var elapsed = (float)(DateTime.Now - _dashStartTime).TotalSeconds;
            return Math.Max(0f, DashDuration - elapsed);
        }
    }
}