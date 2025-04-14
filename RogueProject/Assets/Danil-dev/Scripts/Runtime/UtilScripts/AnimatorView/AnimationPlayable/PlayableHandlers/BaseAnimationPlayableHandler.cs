using UnityEngine;

namespace D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler
{
    [RequireComponent(typeof(AnimationPlayableRoot))]
    public abstract class BaseAnimationPlayableHandler : MonoBehaviour
    {
        protected AnimationPlayableRoot _playableRoot;
        protected virtual void OnEnable() => _playableRoot = GetComponent<AnimationPlayableRoot>();
    }
}