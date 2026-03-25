using UnityEngine;
using System.Collections.Generic;
using System;

namespace BioAdventure.Assets.Script.Gameplay.Mobile
{
    public class BinAssistenceMobile : MonoBehaviour
    {
        public RectTransform[] Spot = new RectTransform[4]; // A B C D  
        public GameObject[] SpotDetected = new GameObject[4]; // Detectores
        public RectTransform[] SpotTemp = new RectTransform[3]; // X Y Z  
        public GameObject[] dropIndicators = new GameObject[4];

        [SerializeField] private List<RectTransform> Bins = new List<RectTransform>(); // Paper Plastic Metal Glass

        private void Start()
        {
            SetTransformFixedSpot();
        }

        // Organiza as lixeiras nos spots fixos (A B C D) baseada na ordem da lista
        public void SetTransformFixedSpot()
        {
            for (int i = 0; i < Bins.Count; i++)
            {
                if (i < Spot.Length)
                    Bins[i].position = Spot[i].position;
            }
        }

        // Move as lixeiras que NÃO estão sendo arrastadas para X Y Z
        public void SetTransformTempSpot(RectTransform dragItem)
        {
            int tempIdx = 0;
            for (int i = 0; i < Bins.Count; i++)
            {
                if (Bins[i] == dragItem) continue;

                if (tempIdx < SpotTemp.Length)
                {
                    Bins[i].position = SpotTemp[tempIdx].position;
                    tempIdx++;
                }
            }
        }

        // Lógica de Reordenar: Remove de onde estava e insere no novo índice
        public void RefreshListFixedBin(RectTransform draggedItem, int newIndex)
        {
            if (newIndex == -1)
            {
                SetTransformFixedSpot();
                return;
            }

            Bins.Remove(draggedItem);
            Bins.Insert(newIndex, draggedItem);

            SetTransformFixedSpot();
        }

        public void SetIndicator(GameObject hitObject)
        {
            // Primeiro, desliga todos para garantir que não fiquem acesos
            foreach (GameObject ind in dropIndicators) ind.SetActive(false);

            // Identifica qual SpotDetected foi atingido
            int index = Array.IndexOf(SpotDetected, hitObject);

            // Se o mouse estiver sobre um spot válido (0 a 3), liga o indicador dele
            if (index != -1 && index < dropIndicators.Length)
            {
                dropIndicators[index].SetActive(true);
            }
        }
    }
}