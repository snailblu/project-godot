using Godot;
using projectgodot.Scripts.Interfaces;

namespace projectgodot.Components
{
    /// <summary>
    /// Node 기반 컴포넌트들의 베이스 클래스
    /// 에디터에서 배치 가능하며 일관된 초기화 패턴을 제공
    /// </summary>
    public abstract partial class ComponentNode : Node, IGameComponent
    {
        /// <summary>
        /// 컴포넌트가 초기화되었는지 여부
        /// </summary>
        protected bool IsInitialized { get; private set; }
        
        /// <summary>
        /// Player에 대한 참조
        /// </summary>
        protected Player Player { get; private set; }
        
        /// <summary>
        /// 컴포넌트 초기화 메서드
        /// 하위 클래스에서 오버라이드하여 구체적인 초기화 로직을 구현
        /// </summary>
        /// <param name="player">Player 인스턴스</param>
        public virtual void Initialize(Player player)
        {
            Player = player ?? throw new System.ArgumentNullException(nameof(player));
            IsInitialized = true;
            OnInitialize();
        }
        
        /// <summary>
        /// 하위 클래스에서 구현할 초기화 로직
        /// Player가 설정된 후에 호출됨
        /// </summary>
        protected virtual void OnInitialize() { }
        
        /// <summary>
        /// 초기화 상태를 확인하는 헬퍼 메서드
        /// </summary>
        protected void EnsureInitialized()
        {
            if (!IsInitialized)
                throw new System.InvalidOperationException($"{GetType().Name} is not initialized. Call Initialize() first.");
        }
    }
}