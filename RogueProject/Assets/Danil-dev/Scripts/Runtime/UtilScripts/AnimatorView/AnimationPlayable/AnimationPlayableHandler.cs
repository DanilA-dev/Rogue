using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using Action = System.Action;

namespace D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler
{
    #region Classes/Structs

    public struct AnimationPlayablePairConfig
    {
        public AnimationClipPlayable Playable { get; set; }
        public AnimationPlayableClipConfig ClipConfig { get; set; }
    }

    #endregion
    
    public class AnimationPlayableHandler : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] private Animator _animator;
        [SerializeField] private int _layersBuffer;
        [SerializeField] private List<AnimationPlayableClipConfig> _clipConfigs;
        
        private PlayableGraph _playableGraph;
        
        private AnimationMixerPlayable _topMixerPlayable;
        private AnimationLayerMixerPlayable _locomotionMixerPlayable;
        private AnimationLayerMixerPlayable _mainLayerMixerPlayable;
        
        private AnimationClipPlayable _currentOneShotPlayable;

        private Dictionary<int, AnimationPlayablePairConfig> _playablesPair;
        private List<AnimationPlayablePairConfig> _locomotionPlayables;
        
        private Coroutine _blendInCoroutine;
        private Coroutine _blendOutCoroutine;

        public Animator Animator
        {
            get => _animator;
            set => _animator = value;
        }

        #endregion

        #region Monobehaviour

        private void Start()
        {
            _playablesPair = new();
            _locomotionPlayables = new();
            _playableGraph = PlayableGraph.Create($"AnimationPlayableGraph_{gameObject.name}");
            var animationPlayableOutput = AnimationPlayableOutput.Create(_playableGraph, "Animation", _animator);
            var locomotionConfigs = _clipConfigs.Where(c => c.IsLocomotion).ToList();
            var locomotionClipsCount = Mathf.Max(1, locomotionConfigs.Count);
                        
            _topMixerPlayable = AnimationMixerPlayable.Create(_playableGraph, 1);
            _mainLayerMixerPlayable = AnimationLayerMixerPlayable.Create(_playableGraph, 2);
            _locomotionMixerPlayable = AnimationLayerMixerPlayable.Create(_playableGraph, locomotionClipsCount);
            
            animationPlayableOutput.SetSourcePlayable(_topMixerPlayable);
            
            _topMixerPlayable.ConnectInput(0, _mainLayerMixerPlayable, 0, 1);
            _mainLayerMixerPlayable.ConnectInput(0, _locomotionMixerPlayable, 0, 1);
            if (locomotionConfigs.Count > 0)
            {
                for (var i = 0; i < locomotionConfigs.Count; i++)
                {
                    var newLocomotionPlayable = CreatePlayableClip(locomotionConfigs[i]);
                    
                    if(locomotionConfigs[i].Mask != null)
                        _locomotionMixerPlayable.SetLayerMaskFromAvatarMask((uint)i,locomotionConfigs[i].Mask);
                    
                    _locomotionMixerPlayable.ConnectInput(i, newLocomotionPlayable, 0, locomotionConfigs[i].Weight);
                    _locomotionPlayables.Add(new AnimationPlayablePairConfig {ClipConfig = locomotionConfigs[i], Playable = newLocomotionPlayable});
                }
            }
            _playableGraph.Play();
        }

        private void OnDestroy()
        {
            if(_playableGraph.IsValid())
                _playableGraph.Destroy();
        }

        #endregion
        
        #region Public

        public void UpdateLocomotionAnimation(Vector3 velocity, float maxSpeed)
        {
            float weight = Mathf.InverseLerp(0f, maxSpeed, velocity.magnitude);
            _locomotionMixerPlayable.SetInputWeight(0, 1f -weight);
            _locomotionMixerPlayable.SetInputWeight(1, weight);
        }
        
        public void PlayOneShotAnimation(AnimationPlayableClipConfig config)
        {
            if(_currentOneShotPlayable.IsValid() && _currentOneShotPlayable.GetAnimationClip() == config.GetAnimationClip())
                return;
            
            InterruptOneShot();
            _currentOneShotPlayable = CreatePlayableClip(config);
            _mainLayerMixerPlayable.ConnectInput(1, _currentOneShotPlayable, 0);
            if(config.Mask != null)
                _mainLayerMixerPlayable.SetLayerMaskFromAvatarMask(1,config.Mask);
            
            float blendDuration = Mathf.Max(0.1f, Mathf.Min(config.GetAnimationClip().length * 0.1f, config.GetAnimationClip().length / 2));
            BlendIn(config.CrossFadeTime, 1);
            BlendOut(config.CrossFadeTime, config.GetAnimationClip().length - config.CrossFadeTime, 1);
        }
        
        
        public void StopAnimation(AnimationPlayablePairConfig playablePair)
        {
            if (playablePair.Playable.IsValid())
            {
                _playableGraph.DestroyPlayable(playablePair.Playable);
                _playablesPair.Remove(playablePair.ClipConfig.Layer);
            }
        }
        
        #endregion

        #region Private

        private void InterruptOneShot()
        {
            if(_blendInCoroutine != null)
                StopCoroutine(_blendInCoroutine);
            
            if(_blendOutCoroutine != null)
                StopCoroutine(_blendOutCoroutine);
            
            _mainLayerMixerPlayable.SetInputWeight(0, 1f);
            _mainLayerMixerPlayable.SetInputWeight(1, 0);
            
            if(_currentOneShotPlayable.IsValid())
                DisconnectOneShot();
            
        }

        private void DisconnectOneShot()
        {
            _mainLayerMixerPlayable.DisconnectInput(1);
            _playableGraph.DestroyPlayable(_currentOneShotPlayable);
        }
        
        private AnimationClipPlayable CreatePlayableClip(AnimationPlayableClipConfig config)
        {
            var newPlayable = AnimationClipPlayable.Create(_playableGraph, config.GetAnimationClip());
            newPlayable.SetApplyFootIK(config.UseFootIK);
            newPlayable.GetAnimationClip().wrapMode = config.WrapMode;
            newPlayable.SetSpeed(config.AnimationSpeed);
            return newPlayable;
        }
        
        private void BlendIn(float duration, int layer)
        {
            _blendInCoroutine = StartCoroutine(Blend(duration, blendTime =>
            {
                float weight = Mathf.Lerp(1f, 0f, blendTime);
                _mainLayerMixerPlayable.SetInputWeight(layer, 1f - weight);
            }));
        }
        
        private void BlendOut(float duration,  float delay, int layer)
        {
            _blendOutCoroutine = StartCoroutine(Blend(duration, blendTime =>
            {
                float weight = Mathf.Lerp(0f, 1f, blendTime);
                _mainLayerMixerPlayable.SetInputWeight(layer, 1f - weight);
            }, delay, DisconnectOneShot));
        }
        
        private IEnumerator Blend(float duration, Action<float> blendCallback, float delay = 0, Action OnFinish = null)
        {
            if(delay > 0)
                yield return new WaitForSeconds(delay);

            float blendTime = 0;
            while (blendTime < 1f)
            {
                blendTime += Time.deltaTime / duration;
                blendCallback(blendTime);
                yield return null;
            }

            blendCallback(1f);
            OnFinish?.Invoke();
        }

        #endregion
    }
}