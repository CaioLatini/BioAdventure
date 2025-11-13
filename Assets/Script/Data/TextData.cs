using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class TextData
{
    public List<string> tutorialString;
    public List<string> infoString;

    public void StartTutorial()
    {
        TextAsset tutorialAsset = Resources.Load<TextAsset>("TutorialStrings");

        string conteudoTutorial = tutorialAsset.text;

        tutorialString = conteudoTutorial
            .Split(new[] { '\r' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .ToList();
    }
    public void StartInfo()
    {
        TextAsset infoAsset = Resources.Load<TextAsset>("InfoStrings");

        string contetudoInfo = infoAsset.text;

        infoString = contetudoInfo
            .Split(new[] { '\r' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .ToList();
    }
}

