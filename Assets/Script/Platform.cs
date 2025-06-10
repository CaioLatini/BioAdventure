namespace BioAdventure.Assets.Script
{
    public static class Platform
    {
        public static bool IsWebPlatform()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
    }
}
