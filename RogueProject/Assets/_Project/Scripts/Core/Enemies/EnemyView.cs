using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyView : AnimationClipPlayableHandler
    {
        #region Fields

        [SerializeField] private AnimationPlayableClipConfig _idleAnimation;
        [SerializeField] private AnimationPlayableClipConfig _patrolAnimation;
        [SerializeField] private AnimationPlayableClipConfig _chaseAnimation;
        [SerializeField] private AnimationPlayableClipConfig _getHitAnimation;
        [SerializeField] private AnimationPlayableClipConfig _deathAnimation;

        #endregion

        #region Public

        public void PlayIdleAnimation() => Play(_idleAnimation);
        public void PlayPatrolAnimation() => Play(_patrolAnimation);
        public void PlayGetHitAnimation() => Play(_getHitAnimation);
        public void PlayDeathAnimation() => Play(_deathAnimation);
        public void PlayChaseAnimation() => Play(_chaseAnimation);

        #endregion
    }
}