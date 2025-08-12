using System;
using projectgodot.Constants;
using projectgodot.Scripts.Interfaces;

namespace projectgodot
{
    public class WeaponComponent : IWeapon
    {
        private readonly float _cooldown;
        private float _cooldownTimer;
        public float Damage { get; set; } = GameConstants.Weapon.DEFAULT_DAMAGE;
        
        public event Action OnShoot;

        public WeaponComponent(float cooldown)
        {
            _cooldown = cooldown;
            _cooldownTimer = 0.0f;
        }

        public bool CanShoot() => _cooldownTimer <= 0;

        public void Shoot()
        {
            if (!CanShoot()) return;
            
            _cooldownTimer = _cooldown;
            OnShoot?.Invoke();
        }
        
        public void Process(float deltaTime)
        {
            if (_cooldownTimer > 0)
            {
                _cooldownTimer -= deltaTime;
            }
        }
    }
}