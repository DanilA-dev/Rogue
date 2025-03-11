using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Core.EquippableWeapon
{ 
    public class EquippableWeaponView : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Animator _animatorController;
        [FoldoutGroup("Events")]
        public UnityEvent<string> OnAnimationChage;
        
        private Dictionary<AnimationClip, int> _animationsHashes = new();

        #endregion

        #region Public

        public void ChangeAnimation(AnimationClip clip, float crossFadeDuration)
        {
            if (_animationsHashes.TryGetValue(clip, out var animationHash))
            {
                _animatorController.CrossFadeInFixedTime(animationHash, crossFadeDuration);
                OnAnimationChage?.Invoke(clip.name);
                return;
            }
            
            var newAnimationHash = Animator.StringToHash(clip.name);
            _animatorController.CrossFadeInFixedTime(newAnimationHash, crossFadeDuration);
            _animationsHashes.Add(clip, newAnimationHash);
        }

        public void PlayAnimation(AnimationClip clip)
        {
            if (_animationsHashes.TryGetValue(clip, out var animationHash))
            {
                _animatorController.Play(animationHash);
                return;
            }
        }

        #endregion
    }
}