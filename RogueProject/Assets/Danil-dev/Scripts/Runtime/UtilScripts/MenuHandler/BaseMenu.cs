﻿using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
using D_Dev.UtilScripts.Tween_Animations.Types;
#endif

namespace D_Dev.UtilScripts.MenuHandler
{
    public abstract class BaseMenu : MonoBehaviour
    {
        #region Fields

        [OnValueChanged(nameof(ActiveColor))]
        [GUIColor(nameof(ActiveColor))]
        [SerializeField, ReadOnly] private bool _isOpen;
        [Title("Animations")] 
        [SerializeField] private bool _hasOpenAnimation;
        [SerializeField] private bool _hasCloseAniation;
        [ShowIf(nameof(_hasCloseAniation))]
        [SerializeField] private bool _disableObjectOnComplete;
#if DOTWEEN
        [ShowIf(nameof(_hasOpenAnimation))]
        [SerializeReference] private BaseAnimationTween[] _openAnimations;
        [ShowIf(nameof(_hasCloseAniation))]
        [SerializeReference] private BaseAnimationTween[] _closeAnimations;
#endif
        #endregion

        #region Properties
        public bool IsOpen => _isOpen;

        #endregion

        #region Monobehaviour

        protected void Awake() => ForceClose();

        #endregion

        #region Public

        public async void Open()
        {
            if(IsOpen)
                return;
            
            gameObject.SetActive(true);
#if DOTWEEN
            if (_hasOpenAnimation && _openAnimations != null
                                  && _openAnimations.Length > 0)
            {
                var seq = DOTween.Sequence();
                seq.Restart();
                seq.SetAutoKill(gameObject);
               
                foreach (var openAnimation in _openAnimations)
                    seq.Append(openAnimation.Play());

                await seq.AsyncWaitForCompletion().AsUniTask();
                _isOpen = true;
                return;
            }
#endif
            _isOpen = true;
        }

        public async void Close()
        {
            if(!IsOpen)
                return;
            
#if DOTWEEN
            if (_hasCloseAniation && _closeAnimations != null
                                  && _closeAnimations.Length > 0)
            {
                var seq = DOTween.Sequence();
                seq.Restart();
                seq.SetAutoKill(gameObject);
                
                foreach (var closeAnimation in _closeAnimations)
                    seq.Append(closeAnimation.Play());

                await seq.AsyncWaitForCompletion().AsUniTask();
                _isOpen = false;
                gameObject.SetActive(!_disableObjectOnComplete);
                return;
            }
#endif
            ForceClose();
        }

        public void ForceOpen()
        {
            _isOpen = true;
            gameObject.SetActive(IsOpen);
        }

        public void ForceClose()
        {
            _isOpen = false;
            gameObject.SetActive(IsOpen);
        }

        #endregion

        #region Virtual
        protected virtual void OnOpen() {}
        protected virtual void OnClose() {}

        #endregion

        #region Private

        private Color ActiveColor() => _isOpen ? Color.green : Color.red;

        #endregion
    }
}
