using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler
{
    [System.Serializable]
    public class AnimationPlayableClipConfig
    {
        #region Fields

        [SerializeField] private bool _isLocomotion;
        [SerializeField] private bool _isRandomizeClip;
        [HideIf(nameof(_isRandomizeClip))] 
        [SerializeField] private AnimationClip _animationClip;
        [ShowIf(nameof(_isRandomizeClip))]
        [SerializeField] private AnimationClip[] _animationClips;
        [HideIf(nameof(_isLocomotion))]
        [SerializeField] private int _layer;
        [SerializeField] private bool _isAdditive;
        [SerializeField, Range(0, 1f)] private float _weight;
        [SerializeField] private WrapMode _wrapMode;
        [SerializeField] private AvatarMask _avatarMask;
        [SerializeField] private bool _useFootIK;
        [SerializeField] private float _crossFadeTime;
        [Range(0, 10)]
        [SerializeField] private float _animationSpeed = 1;

        #endregion
        
        #region Properties

        public bool IsRandomizeClip
        {
            get => _isRandomizeClip;
            set => _isRandomizeClip = value;
        }

        public AnimationClip AnimationClip
        {
            get => _animationClip;
            set => _animationClip = value;
        }

        public AnimationClip[] AnimationClips
        {
            get => _animationClips;
            set => _animationClips = value;
        }

        public int Layer
        {
            get => _layer;
            set => _layer = value;
        }

        public float CrossFadeTime
        {
            get => _crossFadeTime;
            set => _crossFadeTime = value;
        }

        public float AnimationSpeed
        {
            get => _animationSpeed;
            set => _animationSpeed = value;
        }

        public AvatarMask Mask
        {
            get => _avatarMask;
            set => _avatarMask = value;
        }

        public WrapMode WrapMode
        {
            get => _wrapMode;
            set => _wrapMode = value;
        }

        public bool IsAdditive
        {
            get => _isAdditive;
            set => _isAdditive = value;
        }

        public float Weight
        {
            get => _weight;
            set => _weight = value;
        }

        public bool IsLocomotion
        {
            get => _isLocomotion;
            set => _isLocomotion = value;
        }

        public bool UseFootIK
        {
            get => _useFootIK;
            set => _useFootIK = value;
        }

        #endregion
        
        #region Public

        public AnimationClip GetAnimationClip() => _isRandomizeClip 
            ? _animationClips[Random.Range(0, _animationClips.Length)] 
            : _animationClip;

        #endregion
    }
}