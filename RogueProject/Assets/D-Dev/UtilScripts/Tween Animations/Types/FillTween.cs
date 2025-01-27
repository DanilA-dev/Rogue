﻿#if DOTWEEN
using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace D_Dev.UtilScripts.Tween_Animations.Types
{
    [System.Serializable]
    public class FillTween : BaseAnimationTween
    {
        #region Enums

        public enum FillType
        {
            Slider = 0,
            Image = 1
        }

        #endregion

        #region Fields

        [SerializeField] private FillType _fillType;
        [ShowIf(nameof(_fillType), FillType.Image)]
        [SerializeField] private Image _fillImage;
        [ShowIf(nameof(_fillType), FillType.Slider)]
        [SerializeField] private Slider _fillSlider;
        [Space]
        [SerializeField] private float _startValue;
        [SerializeField] private float _endValue;
        [SerializeField] private bool _snapping;

        #endregion

        #region Properties

        public FillType Fill
        {
            get => _fillType;
            set => _fillType = value;
        }

        public Image FillImage
        {
            get => _fillImage;
            set => _fillImage = value;
        }

        public Slider FillSlider
        {
            get => _fillSlider;
            set => _fillSlider = value;
        }

        public float StartValue
        {
            get => _startValue;
            set => _startValue = value;
        }

        public float EndValue
        {
            get => _endValue;
            set => _endValue = value;
        }

        public bool Snapping
        {
            get => _snapping;
            set => _snapping = value;
        }

        #endregion

        #region Override

        public override Tween Play()
        {
            return _fillType switch
            {
                FillType.Slider => Tween = _fillSlider.DOValue(_endValue, Duration, _snapping).From(_startValue),
                FillType.Image => Tween = _fillImage.DOFillAmount(_endValue, Duration).From(_startValue),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        #endregion
    }
}
#endif
