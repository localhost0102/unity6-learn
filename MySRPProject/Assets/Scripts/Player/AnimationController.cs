using System;
using UnityEngine;

namespace Player
{
    public class AnimationController
    {
        private static bool _isFightingAndWalking;
        private static float _previousSpeed;
        private static bool _stopWalkingOnAnimationEnd;
        private static Animator _animator;
        private static bool _isJumping;
        private static bool _isWalking;
        private static bool _isFalling;

        private const string Slash = "Slash";
        private const string IsFighting = "IsFighting"; // Same as slash, but also walks
        private const string Walking = "Walking";
        private const string Jumping = "Jumping";
        private const string Falling = "Falling";

        public static Animator Animator
        {
            get => _animator;
            set => _animator ??= value;
        }

        public static void StartAttack()
        {
            if (!_isJumping && !_isFalling && !_isWalking)
            {
                _animator.SetTrigger(Slash);
            }
            else if (!_isJumping && !_isFalling && _isWalking)
            {
                _animator.SetTrigger(IsFighting);
                _isFightingAndWalking = true;
            }
        }

        public static void EndAttack()
        {
            _isFightingAndWalking = false;
        }
        
        public static void SetWalking(float speed)
        {
            if (_isFalling || _isJumping)
            {
                ForceStopWalking();
                return;
            }

            speed = Mathf.Abs(speed);
            _isWalking = speed > 0f;

            if (_isWalking)
            {
                 _animator.SetFloat(Walking, speed);
            }
            else if (_previousSpeed > speed && !_isWalking)
            {
                _stopWalkingOnAnimationEnd = true;
            }

            _previousSpeed = speed;
        }

        private static void ForceStopWalking()
        {
            float speed = 0f;
            _previousSpeed = speed;
            _animator.SetFloat(Walking, speed);
        }

        public static void SetWalkingEnded()
        {
            if (_stopWalkingOnAnimationEnd)
            {
                _animator.SetFloat(Walking, 0f);
                _stopWalkingOnAnimationEnd = false;
            }
        }

        public static void SetJumping(bool isJumping)
        {
            _isJumping = isJumping;
            _animator.SetBool(Jumping, isJumping);
        }

        public static void SetFalling(bool isFalling)
        {
            if (_isFalling == isFalling) return;

            _isFalling = isFalling;
            _animator.SetBool(Jumping, false);
            _animator.SetBool(Falling, isFalling);
        }

        
    }
}