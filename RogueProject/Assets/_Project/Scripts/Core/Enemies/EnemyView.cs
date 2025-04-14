using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyView : MonoBehaviour
    {
        #region Fields

        [SerializeField] private AnimationClipPlayableMixer _clipPlayableHandler;
        [Space]
        [SerializeField] private AnimationPlayableClipConfig _idleAnimation;
        [SerializeField] private AnimationPlayableClipConfig _patrolAnimation;
        [SerializeField] private AnimationPlayableClipConfig _chaseAnimation;
        [SerializeField] private AnimationPlayableClipConfig _getHitAnimation;
        [SerializeField] private AnimationPlayableClipConfig _deathAnimation;

        #endregion

        #region Public

        public void PlayIdleAnimation() => _clipPlayableHandler.Play(_idleAnimation);
        public void PlayPatrolAnimation() => _clipPlayableHandler.Play(_patrolAnimation);
        public void PlayGetHitAnimation() => _clipPlayableHandler.Play(_getHitAnimation);
        public void PlayDeathAnimation() => _clipPlayableHandler.Play(_deathAnimation);
        public void PlayChaseAnimation() => _clipPlayableHandler.Play(_chaseAnimation);

        #endregion
    }
}