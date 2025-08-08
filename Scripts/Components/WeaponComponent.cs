using System;

namespace projectgodot
{
    public class WeaponComponent
    {
        private readonly float _cooldown;
        private float _cooldownTimer;
        public float Damage { get; set; } = 10f;
        
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