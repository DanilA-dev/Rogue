using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler
{
    [DefaultExecutionOrder(-1)]
    public class AnimationPlayableGraph : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Animator _animator;
        [SerializeField, Min(1)] private int _layerCount;

        private AnimationPlayableOutput _animationPlayableOutput;
            
        #endregion

        #region Properties

        public PlayableGraph PlayableGraph { get; private set; }
        public AnimationLayerMixerPlayable RootLayerMixer { get; private set; }

        public Animator Animator => _animator;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            PlayableGraph = PlayableGraph.Create($"AnimationPlayableGraph {gameObject.name} - {gameObject.GetInstanceID()}");
            _animationPlayableOutput = AnimationPlayableOutput.Create(PlayableGraph, "Animation", _animator);
            RootLayerMixer = AnimationLayerMixerPlayable.Create(PlayableGraph, _layerCount);
            _animationPlayableOutput.SetSourcePlayable(RootLayerMixer);
            PlayableGraph.Play();
        }

        private void OnDestroy()
        {
            if(PlayableGraph.IsValid())
                PlayableGraph.Destroy();
        }

        #endregion
    }
}