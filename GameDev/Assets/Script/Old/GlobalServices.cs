using System.Collections.Generic;
using BioAdventure.Assets.Script.SceneScript;
using UnityEngine;

namespace BioAdventure.Assets.Script
{
    public static class GlobalServices
    {
        private static List<int> _unlockedLevels { get; set; } = new List<int>();

        private static void _refreshUnlockedLevels(UserSaveData data)
        {
            _unlockedLevels.Clear();
            _unlockedLevels.Add(data.Level1);
            _unlockedLevels.Add(data.Level2);
            _unlockedLevels.Add(data.Level3);
            _unlockedLevels.Add(data.Level4);
            _unlockedLevels.Add(data.Level5);
        }

        public static bool IsLevelUnlocked(int levelIndex)
        {
            _refreshUnlockedLevels(Global.CurrentUser);
            return _unlockedLevels[levelIndex] != 0;
        }
        public static void SaveProgress()
        {
            switch (Global.CurrentLevel)
            {
                case 0:
                    Global.CurrentUser.Level1 = Mathf.Max(Global.CurrentUser.Level1, int.Parse(Global.CurrentScore));
                    break;
                case 1:
                    Global.CurrentUser.Level2 = Mathf.Max(Global.CurrentUser.Level2, int.Parse(Global.CurrentScore));
                    break;
                case 2:
                    Global.CurrentUser.Level3 = Mathf.Max(Global.CurrentUser.Level3, int.Parse(Global.CurrentScore));
                    break;
                case 3:
                    Global.CurrentUser.Level4 = Mathf.Max(Global.CurrentUser.Level4, int.Parse(Global.CurrentScore));
                    break;
                case 4:
                    Global.CurrentUser.Level5 = Mathf.Max(Global.CurrentUser.Level5, int.Parse(Global.CurrentScore));
                    break;
            }
            AuthenticationService.SaveUser(Global.CurrentUser);

        }
    }
}