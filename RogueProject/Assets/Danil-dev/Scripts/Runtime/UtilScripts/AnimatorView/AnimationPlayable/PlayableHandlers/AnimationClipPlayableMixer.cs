using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler
{
    public class AnimationClipPlayableMixer : BaseAnimationPlayableMixer
    {
        #region Fields

        [PropertySpace(10)]
        [SerializeField] private bool _hasStartAnimations;
        [ShowIf(nameof(_hasStartAnimations))]
        [SerializeField] private AnimationPlayableClipConfig[] _onStartAnimations;
        
        private AnimationLayerMixerPlayable _animationLayerMixer;
        private AnimationPlayableClipConfig _lastPlayedConfig;
        private Dictionary<int, AnimationPlayablePairConfig> _playablesPair = new();

        private Coroutine _blendInCoroutine;
        private Coroutine _blendOutCoroutine;
        #endregion

        #region Monobehaviour

        protected override void OnEnable()
        {
            base.OnEnable();
            _animationLayerMixer = AnimationLayerMixerPlayable.Create(_playableGraph.PlayableGraph, 1);
            _playableGraph.RootLayerMixer.ConnectInput(_layer, _animationLayerMixer, 0, 1);
        }

        private void Start()
        {
            if (_hasStartAnimations && _onStartAnimations.Length > 0)
                foreach (var animation in _onStartAnimations)
                    Play(animation);
        }

        #endregion
        
        #region Public

        public void Play(AnimationPlayableClipConfig newPlayableConfig)
        {
            if (_playablesPair.TryGetValue(newPlayableConfig.Layer, out var oldPlayable))
            {
                if (newPlayableConfig.GetAnimationClip() == oldPlayable.Playable.GetAnimationClip() &&
                    !oldPlayable.ClipConfig.IsStatic)
                    return;
                
                if(!oldPlayable.ClipConfig.IsStatic)
                    InterruptOneShot(oldPlayable.ClipConfig);
            }
            
            var newAnimationPlayable = CreatePlayableClip(newPlayableConfig);
            if(newPlayableConfig.Layer >= _animationLayerMixer.GetInputCount())
                _animationLayerMixer.SetInputCount(_animationLayerMixer.GetInputCount() + newPlayableConfig.Layer);

            if(!_playablesPair.ContainsKey(newPlayableConfig.Layer))    
                _animationLayerMixer.ConnectInput(newPlayableConfig.Layer, newAnimationPlayable, 0);
           
            if(newPlayableConfig.Mask != null)
                _animationLayerMixer.SetLayerMaskFromAvatarMask((uint)newPlayableConfig.Layer,newPlayableConfig.Mask);
            
            _animationLayerMixer.SetLayerAdditive((uint)newPlayableConfig.Layer, newPlayableConfig.Mask);

            if (_playablesPair.Count > 0)
            {
                BlendIn(newPlayableConfig);
                
                if(newPlayableConfig != _lastPlayedConfig)
                    BlendOut(_lastPlayedConfig);
            }
            else
                _animationLayerMixer.SetInputWeight(newPlayableConfig.Layer, newPlayableConfig.TargetWeight);
            
            _lastPlayedConfig = newPlayableConfig;
            _playablesPair.TryAdd(newPlayableConfig.Layer, new AnimationPlayablePairConfig 
                {ClipConfig = newPlayableConfig, Playable = newAnimationPlayable});
        }

        #endregion

        #region Private

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
                    _playableGraph.PlayableGraph.DestroyPlayable(playableConfig.Playable);
                    _animationLayerMixer.DisconnectInput(config.Layer);
                    _playablesPair.Remove(config.Layer);
                }
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