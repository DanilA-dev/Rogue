using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler
{
    [RequireComponent(typeof(AnimationPlayableGraph))]
    public abstract class BaseAnimationPlayableMixer : MonoBehaviour
    {
        #region Fields

        [Title("Playable Graph")] 
        [SerializeField] protected int _layer;
        [SerializeField] protected AnimationPlayableGraph _playableGraph;
        [Space]
        [SerializeField] protected bool _tryFoundOnItSelf;
        [SerializeField] protected bool _tryFoundOnGameObject;
        [ShowIf(nameof(_tryFoundOnGameObject))]
        [SerializeField] protected GameObject _graphObject;
        [SerializeField] protected bool _tryFoundOnParent;

        #endregion

        #region Properties

        public int Layer
        {
            get => _layer;
            set => _layer = value;
        }

        #endregion

        #region Monobehaviour

        private void Reset()
        {
            if (TryGetComponent(out BaseAnimationPlayableMixer otherHandler) && otherHandler.Layer == _layer)
                Debug.LogError($"Mixer layer - {_layer}, is already taken by {otherHandler.GetType().Name}");
        }

        protected virtual void OnEnable()
        {
            if(_tryFoundOnItSelf)
                TryGetComponent(out _playableGraph);
            
            if(_tryFoundOnGameObject && _graphObject != null)
                _graphObject.TryGetComponent(out _playableGraph);
            
            if(_tryFoundOnParent && transform.parent != null)
                transform.parent.TryGetComponent(out _playableGraph);
        }

        #endregion

        #region Helpers

        protected AnimationClipPlayable CreatePlayableClip(AnimationPlayableClipConfig config)
        {
            var clip = config.GetAnimationClip();
            var newPlayable = AnimationClipPlayable.Create(_playableGraph.PlayableGraph, clip);
            if(!config.IsLooping)
                newPlayable.SetDuration(clip.length);
            
            newPlayable.SetApplyFootIK(config.UseFootIK);
            newPlayable.SetTime(0);
            newPlayable.SetSpeed(config.AnimationSpeed);
            return newPlayable;
        }

        #endregion
    }
}