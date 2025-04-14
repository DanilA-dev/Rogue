using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler
{
    public class AnimationClipPlayableHandler : BaseAnimationPlayableHandler
    {
        #region Fields

        [SerializeField] private bool _hasStartAnimations;
        [ShowIf(nameof(_hasStartAnimations))]
        [SerializeField] private AnimationPlayableClipConfig[] _onStartAnimations;
        
        private AnimationLayerMixerPlayable _animationLayerMixer;
        private Dictionary<int, AnimationPlayablePairConfig> _playablesPair = new();

        private Coroutine _blendInCoroutine;
        private Coroutine _blendOutCoroutine;
        #endregion

        #region Monobehaviour

        protected override void OnEnable()
        {
            base.OnEnable();
            _animationLayerMixer = AnimationLayerMixerPlayable.Create(_playableRoot.PlayableGraph, 1);
            _playableRoot.RootLayerMixer.ConnectInput(2, _animationLayerMixer, 0, 1);
        }

        private void Start()
        {
            if (_hasStartAnimations && _onStartAnimations.Length > 0)
                foreach (var animation in _onStartAnimations)
                    Play(animation);
        }

        #endregion
        
        #region Public

        public void Play(AnimationPlayableClipConfig config)
        {
            if (_playablesPair.TryGetValue(config.Layer, out var playableConfig))
            {
                if(config.GetAnimationClip() == playableConfig.Playable.GetAnimationClip())
                    return;
                
                InterruptOneShot(playableConfig.ClipConfig);
            }
            
            var newAnimationPlayable = CreatePlayableClip(config);
            if(config.Layer >= _animationLayerMixer.GetInputCount())
                _animationLayerMixer.SetInputCount(_animationLayerMixer.GetInputCount() + config.Layer);
            
            _animationLayerMixer.ConnectInput(config.Layer, newAnimationPlayable, 0);
            if(config.Mask != null)
                _animationLayerMixer.SetLayerMaskFromAvatarMask((uint)config.Layer,config.Mask);
            
            _animationLayerMixer.SetLayerAdditive((uint)config.Layer, config.Mask);

            if (_playablesPair.Count > 0)
            {
                var lastConfig = _playablesPair.Last();
               
                BlendIn(config);
                BlendOut(lastConfig.Value.ClipConfig);
            }
            else
                _animationLayerMixer.SetInputWeight(config.Layer, config.TargetWeight);
            
            _playablesPair.TryAdd(config.Layer, new AnimationPlayablePairConfig 
                {ClipConfig = config, Playable = newAnimationPlayable});
        }

        #endregion

        #region Private
        
        private AnimationClipPlayable CreatePlayableClip(AnimationPlayableClipConfig config)
        {
            var clip = config.GetAnimationClip();
            var newPlayable = AnimationClipPlayable.Create(_playableRoot.PlayableGraph, clip);
            if(!config.IsLooping)
                newPlayable.SetDuration(clip.length);
            
            newPlayable.SetApplyFootIK(config.UseFootIK);
            newPlayable.SetTime(0);
            newPlayable.SetSpeed(config.AnimationSpeed);
            return newPlayable;
        }

        private void InterruptOneShot(AnimationPlayableClipConfig config)
        {
            if(_blendInCoroutine != null)
                StopCoroutine(_blendInCoroutine);
            
            if(_blendOutCoroutine != null)
                StopCoroutine(_blendOutCoroutine);
            
            //Set MainPlayableHandler(Animator/BlendTree) to (main.Layer, 1f)
            _animationLayerMixer.SetInputWeight(config.Layer, 0);
            DisconnectOneShot(config);
        }
        

        private void DisconnectOneShot(AnimationPlayableClipConfig config)
        {
            if (_playablesPair.TryGetValue(config.Layer, out var playableConfig))
            {
                if (!playableConfig.Playable.IsValid())
                    return;
            
                if (!config.IsStatic)
                {
                    _playableRoot.PlayableGraph.DestroyPlayable(playableConfig.Playable);
                    _animationLayerMixer.DisconnectInput(config.Layer);
                }
                _playablesPair.Remove(config.Layer);
            }
        }
        
        private void BlendIn(AnimationPlayableClipConfig config)
        {
            if(config == null)
                return;
            
            _blendInCoroutine = StartCoroutine(BlendCoroutine(config, blend =>
            {
                float weight = Mathf.Lerp(0f, 1f, blend);
                _animationLayerMixer.SetInputWeight(config.Layer, weight);
            }));
        }
        
        private void BlendOut(AnimationPlayableClipConfig config)
        {
            if(config == null)
                return;
            
            _blendInCoroutine = StartCoroutine(BlendCoroutine(config, blend =>
            {
                float weight = Mathf.Lerp(1f, 0, blend);
                _animationLayerMixer.SetInputWeight(config.Layer, weight);
            }, config.FadeDelay, () => DisconnectOneShot(config)));
        }
        
        #endregion
        
        #region Coroutines

        private IEnumerator BlendCoroutine(AnimationPlayableClipConfig config,
            Action<float> onBlendCallback, float blendDelay = 0,Action onFinish = null)
        {
            if(blendDelay > 0)
                yield return new WaitForSeconds(blendDelay);
            
            float blendTime = 0;
            while (blendTime < config.TargetWeight)
            {
                blendTime += Time.deltaTime / config.CrossFadeTime;
                onBlendCallback(blendTime);
                yield return null;
            }
            onBlendCallback(config.TargetWeight);
            onFinish?.Invoke();
        }

        #endregion
        
    }
}