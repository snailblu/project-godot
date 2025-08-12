using System;

namespace projectgodot.Scripts.Interfaces
{
    /// <summary>
    /// 허기 시스템을 담당하는 인터페이스
    /// 허기 수치, 음식 섭취, 굶주림 상태 등을 관리
    /// </summary>
    public interface IHunger
    {
        /// <summary>
        /// 최대 허기 수치
        /// </summary>
        int MaxHunger { get; }
        
        /// <summary>
        /// 현재 허기 수치
        /// </summary>
        int CurrentHunger { get; }
        
        /// <summary>
        /// 굶주림 상태인지 여부
        /// </summary>
        bool IsStarving { get; }
        
        /// <summary>
        /// 허기 수치를 감소시킴
        /// </summary>
        /// <param name="amount">감소시킬 양</param>
        void DecreaseHunger(int amount);
        
        /// <summary>
        /// 음식을 섭취하여 허기를 회복
        /// </summary>
        /// <param name="foodValue">음식의 회복량</param>
        void Eat(int foodValue);
        
        /// <summary>
        /// 허기 시스템의 업데이트 처리
        /// </summary>
        /// <param name="deltaTime">프레임 델타 시간</param>
        /// <param name="hungerDecreaseRate">허기 감소율</param>
        void ProcessHunger(float deltaTime, float hungerDecreaseRate);
        
        /// <summary>
        /// 굶주림 상태에서 데미지 처리
        /// </summary>
        /// <param name="deltaTime">프레임 델타 시간</param>
        void ProcessStarvation(float deltaTime);
        
        /// <summary>
        /// 허기 수치 변경 이벤트
        /// </summary>
        event Action<int> HungerChanged;
        
        /// <summary>
        /// 굶주림 시작 이벤트
        /// </summary>
        event Action StarvationStarted;
        
        /// <summary>
        /// 굶주림으로 인한 데미지 발생 이벤트
        /// </summary>
        event Action<int> StarvationDamageApplied;
    }
}