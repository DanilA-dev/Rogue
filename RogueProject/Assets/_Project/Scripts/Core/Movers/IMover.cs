using UnityEngine;

namespace _Project.Scripts.Core
{
    public interface IMover
    {
        public void Move(Vector3 movement);
        public float Speed { get; set; }
        public float StoppingDistance { get; set; }
    }
}