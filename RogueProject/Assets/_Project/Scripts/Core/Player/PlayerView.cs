using Danil_dev.Scripts.Runtime.UtilScripts.AnimatorView;
using UnityEngine;

namespace _Project.Scripts.Core.Player
{
    public class PlayerView : AnimatorView
    {
        #region Fields
        
        private const string AIM_LOCOMOTION = "IsTargetNearBy";
        private const string INPUT_X = "InputX";
        private const string INPUT_Y = "InputY";
        
        [SerializeField] private float _aimBlendTreeSpeedMultiplier;
        [SerializeField] private AnimationConfig _idleAnimation;
        [SerializeField] private AnimationConfig _moveAnimation;

        #endregion
       
        #region Properties
        
        public AnimationConfig IdleAnimation => _idleAnimation;

        public AnimationConfig MoveAnimation => _moveAnimation;
        
        #endregion

        #region Public

        public void PlayIdleAnimation() => PlayAnimation(_idleAnimation);
        public void PlayMoveAnimation() => PlayAnimation(_moveAnimation);
        
        public void ToggleAimLocomotion(bool value) => SetBool(AIM_LOCOMOTION, value);
        public void EvaluateAimLocomotionSpeed(Vector3 velocity)
        {
            var velocityNormalized = velocity.normalized;
            var targetX = velocity != Vector3.zero ? velocityNormalized.x : 0;
            var targetZ = velocity != Vector3.zero ? velocityNormalized.z : 0;
            var animInputX = Animator.GetFloat(INPUT_X);
            var animInputY = Animator.GetFloat(INPUT_Y);
            var x = Mathf.Lerp(animInputX, targetX, _aimBlendTreeSpeedMultiplier * Time.deltaTime);
            var y = Mathf.Lerp(animInputY, targetZ, _aimBlendTreeSpeedMultiplier * Time.deltaTime);
            SetFloat(INPUT_X, x);
            SetFloat(INPUT_Y, y);
        }

        #endregion
    }
}