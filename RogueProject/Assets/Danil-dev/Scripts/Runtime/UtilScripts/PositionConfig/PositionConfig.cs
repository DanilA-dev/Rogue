﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace D_Dev.UtilScripts.PositionConfig
{
    #region Enums

    public enum PositionType
    {
        Vector = 0,
        Transform = 1,
        Random_Vector = 2,
        Random_Transform = 3,
    }

    public enum RotationType
    {
        None = 0,
        EulerAngles = 1,
        Transform = 2
    }
    
    public enum PositionRandomizeType
    {
        None = 0,
        Random_In_Sphere = 1,
        Random_In_Circle = 2
    }

    #endregion
    
    [System.Serializable]
    public class PositionConfig
    {
        #region Fields

        [Title("Position")]
        [SerializeField] private PositionType _positionType;
        [SerializeField] private Transform _parent;
        [ShowIf(nameof(_positionType), PositionType.Vector)]
        [SerializeField] private Vector3 _vectorPos;
        [ShowIf(nameof(_positionType), PositionType.Transform)]
        [SerializeField] private bool _localPosTransform;
        [ShowIf(nameof(_positionType), PositionType.Transform)]
        [SerializeField] private Transform _transformPos;
        [ShowIf(nameof(_positionType), PositionType.Random_Vector)]
        [SerializeField] private Vector3[] _randomVectorPos;
        [ShowIf(nameof(_positionType), PositionType.Random_Transform)]
        [SerializeField] private Transform[] _randomTransforms;
        [SerializeField] private PositionRandomizeType _positionRandomizeType;
        [HideIf(nameof(_positionRandomizeType), PositionRandomizeType.None)]
        [SerializeField] private float _randomRadius;
        [Space]
        [Title("Rotation")] 
        [SerializeField] private RotationType _rotationType;
        [ShowIf(nameof(_rotationType), RotationType.EulerAngles)]
        [SerializeField] private Vector3 _eulerAngles;
        [ShowIf(nameof(_rotationType), RotationType.Transform)]
        [SerializeField] private bool _localRotTransform;
        [ShowIf(nameof(_rotationType), RotationType.Transform)]
        [SerializeField] private Transform _transformRot;

        

        #endregion

        #region Properties

        public PositionType PositionType
        {
            get => _positionType;
            set => _positionType = value;
        }

        public Transform Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public Vector3 VectorPos
        {
            get => _vectorPos;
            set => _vectorPos = value;
        }

        public bool LocalPosTransform
        {
            get => _localPosTransform;
            set => _localPosTransform = value;
        }

        public Transform TransformPos
        {
            get => _transformPos;
            set => _transformPos = value;
        }

        public Vector3[] RandomVectorPos
        {
            get => _randomVectorPos;
            set => _randomVectorPos = value;
        }

        public Transform[] RandomTransforms
        {
            get => _randomTransforms;
            set => _randomTransforms = value;
        }

        public PositionRandomizeType RandomizeType
        {
            get => _positionRandomizeType;
            set => _positionRandomizeType = value;
        }

        public float RandomRadius
        {
            get => _randomRadius;
            set => _randomRadius = value;
        }

        public RotationType RotationType
        {
            get => _rotationType;
            set => _rotationType = value;
        }

        public Vector3 EulerAngles
        {
            get => _eulerAngles;
            set => _eulerAngles = value;
        }

        public bool LocalRotTransform
        {
            get => _localRotTransform;
            set => _localRotTransform = value;
        }

        public Transform TransformRot
        {
            get => _transformRot;
            set => _transformRot = value;
        }

        #endregion

        #region Public

        public Vector3 GetPosition()
        {
            Vector3 pos = _positionType switch
            {
                PositionType.Vector => _vectorPos,
                PositionType.Transform => _localPosTransform ? _transformPos.localPosition : _transformPos.position,
                PositionType.Random_Vector => _randomVectorPos[Random.Range(0, _randomVectorPos.Length)],
                PositionType.Random_Transform => _randomTransforms[Random.Range(0, _randomTransforms.Length)].position,
                _ => Vector3.zero
            };

            return _positionRandomizeType switch
            {
                PositionRandomizeType.None => pos,
                PositionRandomizeType.Random_In_Sphere => pos += Random.insideUnitSphere * _randomRadius,
                PositionRandomizeType.Random_In_Circle => pos += new Vector3(Random.insideUnitCircle.x * _randomRadius,
                    Random.insideUnitCircle.y * _randomRadius, pos.z),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public Quaternion GetRotation()
        {
            Quaternion rot = _rotationType switch
            {
                RotationType.None => Quaternion.identity,
                RotationType.EulerAngles => Quaternion.Euler(_eulerAngles),
                RotationType.Transform => _localRotTransform ? _transformRot.localRotation : _transformRot.rotation,
                _ => throw new ArgumentOutOfRangeException()
            };
            return rot;
        }

        public void SetPosition(ref Transform target)
        {
            target.parent = _parent;
            target.position = GetPosition();
        }
        
        public void SetRotation(ref Transform target)
        {
            target.parent = _parent;
            target.rotation = GetRotation();
        }

        #endregion
    }
}