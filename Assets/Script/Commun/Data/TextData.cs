using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TextData.cs
/*
Classe responsável por carregar e processar arquivos de texto (TXT) da pasta Resources.
Utilizada para alimentar os sistemas de diálogo e informações da UI com os textos do jogo.
*/

namespace BioAdventure.Assets.Script.Data
{
    public class TextData
    {
        // Listas que armazenam as frases extraídas dos arquivos de texto
        public List<string> tutorialString;
        public List<string> infoString;

        // Carrega o arquivo TutorialStrings.txt e divide seu conteúdo em uma lista de frases limpas
        public void StartTutorial()
        {
            TextAsset tutorialAsset = Resources.Load<TextAsset>("TextTutorial");

            if (tutorialAsset != null)
            {
                tutorialString = tutorialAsset.text
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();
            }
        }

        // Carrega o arquivo InfoStrings.txt e divide seu conteúdo em uma lista de frases limpas
        public void StartInfo()
        {
            // 1. Inicializa a lista para evitar erro de "Object reference" no GameUI
            infoString = new List<string>();

            // 2. Tenta carregar o arquivo (Certifique-se que o nome é IGUAL ao do arquivo na pasta)
            TextAsset infoAsset = Resources.Load<TextAsset>("TextInfo");

            if (infoAsset != null)
            {
                Debug.Log("Arquivo InfoStrings encontrado!");

                // 3. Divide o texto por linhas
                infoString = infoAsset.text
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s)) // Garante que não pego linhas vazias
                    .ToList();

                Debug.Log("Total de frases carregadas: " + infoString.Count);
            }
            else
            {
                // Se cair aqui, o Unity não achou o arquivo com esse nome em Resources
                Debug.LogError("ERRO: O arquivo 'InfoStrings' não foi encontrado em Assets/Resources");
            }
        }
    }
}