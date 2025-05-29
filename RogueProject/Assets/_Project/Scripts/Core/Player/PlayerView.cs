using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView;
using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core.Player
{
    public class PlayerView : AnimatorView
    {
        #region Fields

        private const string INPUT_X = "InputX";
        private const string INPUT_Y = "InputY";
        private const string AIM_LOCOMOTION = "IsTargetNearBy";

        [Title("Animator Clips")] 
        [SerializeField] private float _aimBlendTreeSpeedMultiplier;
        [SerializeField] private AnimationClipConfig _idle;
        [SerializeField] private AnimationClipConfig _run;
        [Space]
        [Title("Playables")]
        [SerializeField] private AnimationClipPlayableMixer _clipPlayableMixer;
        [Space]
        [SerializeField] private AnimationPlayableClipConfig _deathAnimation;
        [SerializeField] private AnimationPlayableClipConfig _getHitAnimation;

        #endregion

        #region Public

        public void PlayIdle() => PlayAnimation(_idle);
        public void PlayRun() => PlayAnimation(_run);
        
        public void PlayDeathAnimation() => _clipPlayableMixer.Play(_deathAnimation);
        public void PlayGetHitAnimation() => _clipPlayableMixer.Play(_getHitAnimation);
        public void ToggleAimLocomotion(bool value) => SetBool(AIM_LOCOMOTION, value);
        public void EvaluateAimLocomotionSpeed(Vector3 velocity)
        {
            var velocityNormalized = velocity.normalized;
            var targetX = velocity != Vector3.zero ? velocityNormalized.x : 0;
            var targetZ = velocity != Vector3.zero ? velocityNormalized.z : 0;
            var animInputX = GetFloat(INPUT_X);
            var animInputY = GetFloat(INPUT_Y);
            var x = Mathf.Lerp(animInputX, targetX, _aimBlendTreeSpeedMultiplier * Time.deltaTime);
            var y = Mathf.Lerp(animInputY, targetZ, _aimBlendTreeSpeedMultiplier * Time.deltaTime);
            SetFloat(INPUT_X, x);
            SetFloat(INPUT_Y, y);
        }

        #endregion
    }
}