using System;
using System.Collections.Generic;
using UnityEngine;

namespace BioAdventure.Assets.Script.Data
{   
    [System.Serializable]
    public class Achievement
    {
        public AchievementID id;
        public string title;
        public string description;
        public Sprite icon;
    }

    public enum AchievementID
    {
        Defeat,
        Victory,
        Perfect,
        TheEnd,
        WhatYouDoing,
        AbsoluteCinema
    }


}