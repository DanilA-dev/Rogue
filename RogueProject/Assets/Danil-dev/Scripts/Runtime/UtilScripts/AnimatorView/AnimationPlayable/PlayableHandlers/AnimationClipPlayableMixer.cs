using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
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
        
        private AnimationPlayableClipConfig _currentPlayableClipConfig;
        private AnimationPlayableClipConfig _lastPlayedConfig;
        
        private Dictionary<int, AnimationPlayablePairConfig> _playablesPair = new();

        private Coroutine _blendInCoroutine;
        private Coroutine _blendOutCoroutine;
        
        #endregion

        #region Monobehaviour
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
            if(_playablesPair.Count == 1 && _lastPlayedConfig == newPlayableConfig)
                return;
            
            if (_playablesPair.TryGetValue(newPlayableConfig.Layer + 1, out var oldPlayable))
            {
                if (oldPlayable.Playable.IsValid() &&
                    newPlayableConfig.GetAnimationClip() == oldPlayable.Playable.GetAnimationClip() &&
                    !oldPlayable.ClipConfig.IsStatic)
                    return;

                if(!oldPlayable.ClipConfig.IsStatic && oldPlayable.Playable.IsValid())
                    InterruptOneShot(oldPlayable.ClipConfig);
            }

            var newAnimationLayer = newPlayableConfig.Layer + 1;
            var newAnimationPlayable = CreatePlayableClip(newPlayableConfig);
            if(newAnimationLayer >=  _playableGraph.RootLayerMixer.GetInputCount())
                _playableGraph.RootLayerMixer.SetInputCount(_playableGraph.RootLayerMixer.GetInputCount() + newAnimationLayer);

            if(!_playablesPair.ContainsKey(newAnimationLayer))    
                _playableGraph.RootLayerMixer.ConnectInput(newAnimationLayer, newAnimationPlayable, 0);
           
            if(newPlayableConfig.Mask != null)
                _playableGraph.RootLayerMixer.SetLayerMaskFromAvatarMask((uint)newAnimationLayer,newPlayableConfig.Mask);
            
            _playableGraph.RootLayerMixer.SetLayerAdditive((uint)newAnimationLayer, newPlayableConfig.IsAdditive);
            _currentPlayableClipConfig = newPlayableConfig;
            if (_playablesPair.Count > 0 || _layer != 0)
            {
                BlendIn(newPlayableConfig);
                BlendOut(_lastPlayedConfig);
            }
            else
                _playableGraph.RootLayerMixer.SetInputWeight(newAnimationLayer, newPlayableConfig.TargetWeight);
            
            _lastPlayedConfig = newPlayableConfig;
            _playablesPair.TryAdd(newAnimationLayer, new AnimationPlayablePairConfig 
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
            
            _playableGraph.RootLayerMixer.SetInputWeight(config.Layer + 1, 0);
            _playableGraph.RootLayerMixer.SetInputWeight(0, 1);
            
            DisconnectOneShot(config);
        }
        

        private void DisconnectOneShot(AnimationPlayableClipConfig config)
        {
            if (_playablesPair.TryGetValue(config.Layer + 1, out var playableConfig))
            {
                if (!playableConfig.Playable.IsValid())
                    return;

                if (!config.IsStatic)
                {
                    if(_lastPlayedConfig == config)
                        _lastPlayedConfig = null;
                    
                    _playableGraph.PlayableGraph.DestroyPlayable(playableConfig.Playable);
                    _playableGraph.RootLayerMixer.DisconnectInput(config.Layer + 1);
                    _playablesPair.Remove(config.Layer + 1);
                }
            }
        }
        
        private void BlendIn(AnimationPlayableClipConfig config)
        {
            if(config == null)
                return;
                       
            var clip = config.GetAnimationClip();
            var crossFadeTime = config.UseAutoFadeTimeBasedOnClipLength
                ? Mathf.Clamp(clip.length * 0.1f, 0.1f, clip.length * 0.5f)
                : config.CrossFadeTime;
            _blendInCoroutine = StartCoroutine(BlendCoroutine(crossFadeTime, config.TargetWeight,blend =>
            {
                float weight = Mathf.Lerp(0f, 1f, blend);
                _playableGraph.RootLayerMixer.SetInputWeight(config.Layer + 1, weight);
            }));
        }
        
        private void BlendOut(AnimationPlayableClipConfig config)
        {
            if (config == null)
                config = _currentPlayableClipConfig;
            
            var clip = config.GetAnimationClip();
            var crossFadeTime = config.UseAutoFadeTimeBasedOnClipLength
                ? Mathf.Clamp(clip.length * 0.1f, 0.1f, clip.length * 0.5f)
                : config.CrossFadeTime;
            var delayTime = config.UseAutoFadeTimeBasedOnClipLength
                ? clip.length - crossFadeTime
                : config.FadeDelay;
            
            _blendInCoroutine = StartCoroutine(BlendCoroutine(crossFadeTime, config.TargetWeight,blend =>
            {
                float weight = Mathf.Lerp(1f, 0, blend);
                if(!config.IsStatic)
                    _playableGraph.RootLayerMixer.SetInputWeight(config.Layer + 1, weight);
                
            }, delayTime, () => DisconnectOneShot(config)));
        }
        
        #endregion
        
        #region Coroutines

        private IEnumerator BlendCoroutine(float crossFadeDuration, float targetWeight,
            Action<float> onBlendCallback, float blendDelay = 0,Action onFinish = null)
        {
            if(blendDelay > 0)
                yield return new WaitForSeconds(blendDelay);
            
            float blendTime = 0;
            while (blendTime < targetWeight)
            {
                blendTime += Time.deltaTime / crossFadeDuration;
                onBlendCallback(blendTime);
                yield return null;
            }
            onBlendCallback(targetWeight);
            onFinish?.Invoke();
        }
        
        #endregion
    }
}