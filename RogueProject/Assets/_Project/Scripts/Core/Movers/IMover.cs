using UnityEngine;

namespace _Project.Scripts.Core
{
    public interface IMover
    {
        public void Move(Vector3 movement);
        public float MoveSpeed { get; set; }
        public float RotationSpeed { get; set; }
        public float StoppingDistance { get; set; }
        public bool EnableDirectionRotation { get; set; }
    }
}