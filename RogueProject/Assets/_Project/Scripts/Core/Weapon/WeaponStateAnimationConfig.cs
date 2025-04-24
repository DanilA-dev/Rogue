using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler;

namespace _Project.Scripts.Core.Weapon
{
    [System.Serializable]
    public struct WeaponStateAnimationConfig
    {
        public WeaponState State;
        public AnimationPlayableClipConfig AnimationConfig;
    }
}