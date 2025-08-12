using System;

namespace projectgodot.Scripts.Interfaces
{
    /// <summary>
    /// 무기 컴포넌트의 인터페이스
    /// 발사 로직, 데미지, 쿨다운 등을 관리
    /// </summary>
    public interface IWeapon
    {
        /// <summary>
        /// 무기 데미지
        /// </summary>
        float Damage { get; set; }
        
        /// <summary>
        /// 발사 가능한 상태인지 확인
        /// </summary>
        bool CanShoot();
        
        /// <summary>
        /// 무기 발사
        /// </summary>
        void Shoot();
        
        /// <summary>
        /// 쿨다운 등 무기 상태 업데이트
        /// </summary>
        /// <param name="deltaTime">프레임 델타 시간</param>
        void Process(float deltaTime);
        
        /// <summary>
        /// 발사 시 발생하는 이벤트
        /// </summary>
        event Action OnShoot;
    }
}