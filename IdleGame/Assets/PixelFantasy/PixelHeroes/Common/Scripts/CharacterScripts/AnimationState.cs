namespace Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts
{
    /// <summary>
    /// Animation state. The same parameter controls animation transitions.
    /// </summary>
    public enum AnimationState
    {
        Idle,
        Ready,
        Walking,
        Running,
        Jumping,
        Blocking,
        Crawling,
        Climbing,
        Dead
    }
}