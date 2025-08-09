using Godot;
using System;

namespace projectgodot.Components
{
    public partial class CameraShakeComponent : Node
    {
        private Camera2D camera;
        private Vector2 originalPosition;
        private float shakeTimer = 0f;
        private float shakeDuration = 0f;
        private float shakeIntensity = 0f;
        private readonly Random random = new();

        public override void _Ready()
        {
            // 카메라 참조 찾기
            camera = GetParent().GetNode<Camera2D>("Camera2D");
            if (camera != null)
            {
                originalPosition = camera.Position;
                GD.Print("CameraShakeComponent: Camera found and initialized");
            }
            else
            {
                GD.PrintErr("CameraShakeComponent: Camera2D not found!");
            }
        }

        public override void _Process(double delta)
        {
            if (shakeTimer > 0 && camera != null)
            {
                // 랜덤한 오프셋 계산
                float offsetX = (float)(random.NextDouble() - 0.5) * 2 * shakeIntensity;
                float offsetY = (float)(random.NextDouble() - 0.5) * 2 * shakeIntensity;
                
                // 카메라 위치 설정
                camera.Position = originalPosition + new Vector2(offsetX, offsetY);
                
                // 타이머 감소
                shakeTimer -= (float)delta;
                
                // 쉐이크 종료 시 원래 위치로 복원
                if (shakeTimer <= 0)
                {
                    camera.Position = originalPosition;
                }
            }
        }

        public void StartShake(float intensity, float duration)
        {
            if (camera == null)
            {
                GD.PrintErr("CameraShakeComponent: Cannot start shake - camera is null!");
                return;
            }
            
            shakeIntensity = intensity;
            shakeDuration = duration;
            shakeTimer = duration;
            
            GD.Print($"CameraShakeComponent: Starting shake - Intensity: {intensity}, Duration: {duration}");
        }

        public void StopShake()
        {
            shakeTimer = 0f;
            if (camera != null)
            {
                camera.Position = originalPosition;
            }
        }

        public bool IsShaking => shakeTimer > 0;
    }
}