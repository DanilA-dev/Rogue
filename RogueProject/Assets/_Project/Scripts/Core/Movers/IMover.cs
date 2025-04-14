using System;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public interface IMover
    {
        public Vector3 Target { get; set; }
        public void Move();
        public event Action<Vector3> OnMove; 
        public float MoveSpeed { get; set; }
        public float StoppingDistance { get; set; }
    }
}