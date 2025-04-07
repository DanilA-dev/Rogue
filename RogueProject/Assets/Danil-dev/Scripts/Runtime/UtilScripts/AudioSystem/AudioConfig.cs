using D_Dev.UtilScripts.ScriptableVaiables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace D_dev.Scripts.Runtime.UtilScripts.AudioSystem
{
    #region Enums

    public enum DelayType
    {
        None = 0,
        SimpleDelay = 1,
        ScheduledDelay = 2,
    }

    #endregion
    
    [System.Serializable]
    public class AudioConfig
    {
        #region Fields

        [SerializeField] private StringScriptableVariable _audioConfigName;
        [SerializeField] private AudioMixerGroup _soundMixer;
        [Space]
        [Title("Clips")]
        [HideIf(nameof(_isRandomClip))]
        [SerializeField] private AudioClip _clip;
        [SerializeField] private bool _isRandomClip;
        [ShowIf(nameof(_isRandomClip))]
        [SerializeField] private AudioClip[] _clips;
        [Title("Audio Settings")]
        [Range(0, 1),SerializeField] private float _volume;
        [SerializeField] private float _pitch;
        [SerializeField] private bool _isRandomVolume;
        [SerializeField] private bool _isRandomPitch;
        [ShowIf(nameof(_isRandomVolume))]
        [Range(0.01f, 1),SerializeField] private float _minVolume;
        [ShowIf(nameof(_isRandomVolume))]
        [Range(0.01f, 1),SerializeField] private float _maxVolume;
        [ShowIf(nameof(_isRandomPitch))]
        [SerializeField] private float _minPitch;
        [ShowIf(nameof(_isRandomPitch))]
        [SerializeField] private float _maxPitch;
        [Range(0, 1)] [SerializeField] private float _spatialBlend;
        [SerializeField] private float _fadeTime;
        [SerializeField] private DelayType _delayType;
        [ShowIf(nameof(_delayType), DelayType.SimpleDelay)]
        [SerializeField] private float _delay;
        [ShowIf(nameof(_delayType), DelayType.ScheduledDelay)]
        [SerializeField] private double _scheduledTime;
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private bool _isLooping;

        #endregion

        #region Properties

        public StringScriptableVariable AudioConfigName
        {
            get => _audioConfigName;
            set => _audioConfigName = value;
        }

        public AudioMixerGroup SoundMixer
        {
            get => _soundMixer;
            set => _soundMixer = value;
        }

        public AudioClip Clip
        {
            get => _clip;
            set => _clip = value;
        }

        public bool IsRandomClip
        {
            get => _isRandomClip;
            set => _isRandomClip = value;
        }

        public AudioClip[] Clips
        {
            get => _clips;
            set => _clips = value;
        }

        public float Volume
        {
            get => _volume;
            set => _volume = value;
        }

        public float Pitch
        {
            get => _pitch;
            set => _pitch = value;
        }

        public bool IsRandomVolume
        {
            get => _isRandomVolume;
            set => _isRandomVolume = value;
        }

        public bool IsRandomPitch
        {
            get => _isRandomPitch;
            set => _isRandomPitch = value;
        }

        public float MinVolume
        {
            get => _minVolume;
            set => _minVolume = value;
        }

        public float MaxVolume
        {
            get => _maxVolume;
            set => _maxVolume = value;
        }

        public float MinPitch
        {
            get => _minPitch;
            set => _minPitch = value;
        }

        public float MaxPitch
        {
            get => _maxPitch;
            set => _maxPitch = value;
        }

        public float SpatialBlend
        {
            get => _spatialBlend;
            set => _spatialBlend = value;
        }

        public bool PlayOnAwake
        {
            get => _playOnAwake;
            set => _playOnAwake = value;
        }

        public bool IsLooping
        {
            get => _isLooping;
            set => _isLooping = value;
        }

        public float FadeTime
        {
            get => _fadeTime;
            set => _fadeTime = value;
        }

        public float Delay
        {
            get => _delay;
            set => _delay = value;
        }

        public double ScheduledTime
        {
            get => _scheduledTime;
            set => _scheduledTime = value;
        }

        public DelayType DelayType
        {
            get => _delayType;
            set => _delayType = value;
        }

        #endregion

        #region Public

        public void SetAudioSource(ref AudioSource audioSource)
        {
            audioSource.clip = GetClip();
            audioSource.volume = _isRandomVolume ? Random.Range(_minVolume, _maxVolume) : _volume;
            audioSource.pitch = _isRandomPitch ? Random.Range(_minPitch, _maxPitch) : _pitch;
            audioSource.loop = _isLooping;
            audioSource.playOnAwake = _playOnAwake;
        }

        public AudioClip GetClip() => _isRandomClip ? _clips[Random.Range(0, _clips.Length)] : _clip;

        #endregion
    }
}