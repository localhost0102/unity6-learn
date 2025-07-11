using System;
using UnityEngine;

namespace Player
{
    public class AnimationController
    {
        private static Animator _animator;
        private static bool _isJumping;
        private static bool _isWalking;
        private static bool _isFalling;
        
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
            if (!_isWalking && !_isJumping && !_isFalling)
                _animator.SetTrigger(Slash);
        }

        public static void SetWalking(float speed)
        {
            if (_isJumping)
                speed = 0f;
            
            if (_isFalling)
                return;
            
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
            if (_isFalling == isFalling) return;
            
            _isFalling =  isFalling;
            _animator.SetBool(Jumping, false);
            _animator.SetBool(Falling, isFalling);
        }
    }
}