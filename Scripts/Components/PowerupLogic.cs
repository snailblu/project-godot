using System;

namespace projectgodot
{
    /// <summary>
    /// 파워업 효과의 순수 로직을 담당하는 클래스
    /// Godot에 의존하지 않으므로 단위 테스트 가능
    /// </summary>
    public class PowerupLogic
    {
        public float DamageMultiplier { get; set; } = 2.0f;
        public float Duration { get; set; } = 5.0f;
        public bool IsActive { get; private set; }
        
        private DateTime _activationTime;
        private float _originalDamage;
        private bool _effectStarted;

        public void Activate(float originalDamage)
        {
            if (IsActive) return; // 이미 활성화된 경우 무시

            _originalDamage = originalDamage;
            IsActive = true;
            _effectStarted = true;
            _activationTime = DateTime.Now;
        }

        /// <summary>
        /// 파워업 효과를 업데이트합니다. 매 프레임 호출해야 합니다.
        /// </summary>
        public void Update()
        {
            if (!_effectStarted) return;

            var elapsed = (float)(DateTime.Now - _activationTime).TotalSeconds;
            
            if (elapsed >= Duration)
            {
                IsActive = false;
                _effectStarted = false;
            }
        }

        /// <summary>
        /// 현재 적용할 데미지 값을 계산합니다.
        /// </summary>
        /// <param name="baseDamage">기본 데미지 값</param>
        /// <returns>파워업이 적용된 데미지 값</returns>
        public float CalculateDamage(float baseDamage)
        {
            if (!IsActive) return baseDamage;
            
            return baseDamage * DamageMultiplier;
        }

        /// <summary>
        /// 파워업 효과를 강제로 종료합니다.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
            _effectStarted = false;
        }

        /// <summary>
        /// 남은 효과 시간을 반환합니다.
        /// </summary>
        public float GetRemainingTime()
        {
            if (!_effectStarted) return 0f;
            
            var elapsed = (float)(DateTime.Now - _activationTime).TotalSeconds;
            return Math.Max(0f, Duration - elapsed);
        }

        /// <summary>
        /// 원래 데미지 값을 반환합니다.
        /// </summary>
        public float GetOriginalDamage()
        {
            return _originalDamage;
        }
    }
}