using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Core.EquippableWeapon
{ 
    public class EquippableWeaponView : MonoBehaviour
    {
        #region Fields
        
        [FoldoutGroup("Events")]
        public UnityEvent<string> OnAnimationChage;
        
        private Dictionary<AnimationClip, int> _animationsHashes = new();

        #endregion

        #region Properties

        public Animator AnimatorController { get; private set; }

        #endregion

        #region Public

        public void Init(Animator animatorController) => AnimatorController = animatorController;
        
        public void ChangeAnimation(AnimationClip clip, float crossFadeDuration)
        {
            if (_animationsHashes.TryGetValue(clip, out var animationHash))
            {
                AnimatorController?.CrossFadeInFixedTime(animationHash, crossFadeDuration);
                OnAnimationChage?.Invoke(clip.name);
                return;
            }
            
            var newAnimationHash = Animator.StringToHash(clip.name);
            AnimatorController?.CrossFadeInFixedTime(newAnimationHash, crossFadeDuration);
            _animationsHashes.Add(clip, newAnimationHash);
        }

        public void PlayAnimation(AnimationClip clip)
        {
            if (_animationsHashes.TryGetValue(clip, out var animationHash))
            {
                AnimatorController?.Play(animationHash);
                return;
            }
        }

        #endregion
    }
}