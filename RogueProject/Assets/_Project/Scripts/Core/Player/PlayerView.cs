using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler;
using UnityEngine;

namespace _Project.Scripts.Core.Player
{
    public class PlayerView : AnimationClipPlayableHandler
    {
        #region Fields
        
        private const string AIM_LOCOMOTION = "IsTargetNearBy";
        private const string INPUT_X = "InputX";
        private const string INPUT_Y = "InputY";
        
        [SerializeField] private float _aimBlendTreeSpeedMultiplier;
        [SerializeField] private AnimationPlayableClipConfig _idleAnimation;
        [SerializeField] private AnimationPlayableClipConfig _moveAnimation;
        [SerializeField] private AnimationPlayableClipConfig _deathAnimation;
        [SerializeField] private AnimationPlayableClipConfig _getHitAnimation;

        #endregion

        #region Public

        public void PlayIdleAnimation() => Play(_idleAnimation);
        public void PlayMoveAnimation() => Play(_moveAnimation);
        public void PlayDeathAnimation() => Play(_deathAnimation);
        public void PlayGetHitAnimation() => Play(_getHitAnimation);
        
        // public void ToggleAimLocomotion(bool value) => SetBool(AIM_LOCOMOTION, value);
        // public void EvaluateAimLocomotionSpeed(Vector3 velocity)
        // {
        //     var velocityNormalized = velocity.normalized;
        //     var targetX = velocity != Vector3.zero ? velocityNormalized.x : 0;
        //     var targetZ = velocity != Vector3.zero ? velocityNormalized.z : 0;
        //     var animInputX = Animator.GetFloat(INPUT_X);
        //     var animInputY = Animator.GetFloat(INPUT_Y);
        //     var x = Mathf.Lerp(animInputX, targetX, _aimBlendTreeSpeedMultiplier * Time.deltaTime);
        //     var y = Mathf.Lerp(animInputY, targetZ, _aimBlendTreeSpeedMultiplier * Time.deltaTime);
        //     SetFloat(INPUT_X, x);
        //     SetFloat(INPUT_Y, y);
        // }

        #endregion
    }
}