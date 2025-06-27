using System;
using UnityEngine;

namespace Player
{
    public class AnimationController
    {
        private static Animator _animator;
        private static bool _isJumping;
        private static bool _isWalking;
        
        private const string Slash = "Slash";
        private const string Walking = "Walking";
        private const string Jumping = "Jumping";
        private const string Falling = "Falling";

        public static Animator Animator
        {
            get => _animator;
            set => _animator ??= value;
        }

        public static void SetSlash()
        {
            if (!_isWalking)
                _animator.SetTrigger(Slash);
        }

        public static void SetWalking(float speed)
        {
            if (_isJumping)
                speed = 0f;
            
            float movingSpeed = Mathf.Abs(speed);
            _isWalking = movingSpeed > 0f;
            _animator.SetFloat(Walking, movingSpeed);
        }

        public static void SetJumping(bool isJumping)
        {
            _isJumping = isJumping;
            _animator.SetBool(Jumping, isJumping);
        }

        public static void SetFalling(bool  isFalling)
        {
            _animator.SetBool(Jumping, false);
            _animator.SetBool(Falling, isFalling);
        }
    }
}