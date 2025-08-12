using System;

namespace projectgodot.Scripts.Interfaces
{
    /// <summary>
    /// 체력 관리를 담당하는 인터페이스
    /// 체력, 데미지, 힐링 등을 처리
    /// </summary>
    public interface IHealth
    {
        /// <summary>
        /// 최대 체력
        /// </summary>
        int MaxHealth { get; }
        
        /// <summary>
        /// 현재 체력
        /// </summary>
        int CurrentHealth { get; }
        
        /// <summary>
        /// 죽은 상태인지 여부
        /// </summary>
        bool IsDead { get; }
        
        /// <summary>
        /// 데미지를 받음
        /// </summary>
        /// <param name="amount">데미지 양</param>
        void TakeDamage(int amount);
        
        /// <summary>
        /// 체력을 회복함
        /// </summary>
        /// <param name="amount">회복 양</param>
        void Heal(int amount);
        
        /// <summary>
        /// 죽음 이벤트
        /// </summary>
        event Action Died;
        
        /// <summary>
        /// 체력 변경 이벤트
        /// </summary>
        event Action<int> HealthChanged;
    }
}