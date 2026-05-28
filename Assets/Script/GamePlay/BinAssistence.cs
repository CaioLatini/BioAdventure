using UnityEngine;
using System.Collections.Generic;
using System;
using BioAdventure.Assets.Script.Managers;
using UnityEngine.UI;

// BinAssistence.cs
/*
Gerencia a distribuição e a troca fluida de lugares das lixeiras no ecrã.
Contém os pontos de ancoragem (Spots) e os pontos temporários onde as lixeiras
aguardam enquanto uma está a ser arrastada.
*/

namespace BioAdventure.Assets.Script.Gameplay
{
    public class BinAssistence : MonoBehaviour
    {
        [Header("Configuração de Spots Fixos (A, B, C, D)")]
        [Tooltip("As posições âncoras principais onde as lixeiras descansam.")]
        public RectTransform[] Spot;

        [Tooltip("Os objetos detetores (hitboxes invisíveis) que ficam por trás dos Spots.")]
        public GameObject[] SpotDetected;

        [Tooltip("Indicadores visuais (ex: contorno brilhante) para mostrar onde a lixeira vai encaixar.")]
        public GameObject[] dropIndicators;

        [Header("Configuração de Spots Temporários (X, Y, Z)")]
        [Tooltip("Posições para onde as lixeiras que NÃO estão a ser arrastadas recuam temporariamente.")]
        public RectTransform[] SpotTemp;

        [Header("Lista Dinâmica de Lixeiras")]
        [Tooltip("A lista contendo as 4 lixeiras. A ordem desta lista define as posições reais delas no ecrã.")]
        [SerializeField] private List<RectTransform> Bins = new List<RectTransform>();

        private void Start()
        {
            SetTransformFixedSpot();
        }

        // Organiza as lixeiras nos spots fixos (A B C D) baseada na ordem atual da lista
        public void SetTransformFixedSpot()
        {
            if (Bins == null || Spot == null) return;

            for (int i = 0; i < Bins.Count; i++)
            {
                if (i < Spot.Length && Bins[i] != null && Spot[i] != null)
                {
                    Bins[i].position = Spot[i].position;
                }
            }
        }

        // Move as lixeiras que não estão a ser arrastadas para os spots temporários (recolhimento)
        public void SetTransformTempSpot(RectTransform dragItem)
        {
            if (Bins == null || SpotTemp == null) return;

            int tempIdx = 0;
            for (int i = 0; i < Bins.Count; i++)
            {
                if (Bins[i] == dragItem) continue;

                if (tempIdx < SpotTemp.Length && SpotTemp[tempIdx] != null)
                {
                    Bins[i].position = SpotTemp[tempIdx].position;
                    tempIdx++;
                }
            }
        }

        // Lógica de Reordenação: Remove de onde estava e insere no novo índice, voltando a alinhar todas
        public void RefreshListFixedBin(RectTransform draggedItem, int newIndex)
        {
            // Se soltou fora de um spot válido, cancela o movimento e repõe tudo
            if (newIndex == -1 || !Bins.Contains(draggedItem))
            {
                if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("back", 0.5f);
                SetTransformFixedSpot();
                return;
            }

            // Som de encaixe bem sucedido
            if (SoundManager.Instance != null) SoundManager.Instance.PlayEffect("go", 0.8f);

            Bins.Remove(draggedItem);

            // Segurança: Garante que o novo índice não seja maior do que o tamanho da lista após a remoção
            newIndex = Mathf.Clamp(newIndex, 0, Bins.Count);
            Bins.Insert(newIndex, draggedItem);

            SetTransformFixedSpot();
        }

        // Liga o indicador de brilho por trás da lixeira alvo e desliga os restantes
        public void SetIndicator(GameObject hitObject)
        {
            if (dropIndicators == null) return;

            // Primeiro, desliga todos para garantir a limpeza do ecrã
            foreach (GameObject ind in dropIndicators)
            {
                if (ind != null) ind.SetActive(false);
            }

            if (hitObject == null || SpotDetected == null) return;

            // Identifica qual SpotDetected foi atingido pelo dedo
            int index = Array.IndexOf(SpotDetected, hitObject);

            // Se o dedo estiver sobre um spot válido, liga o indicador correspondente
            if (index != -1 && index < dropIndicators.Length && dropIndicators[index] != null)
            {
                dropIndicators[index].SetActive(true);
            }
        }

        public void DesableRayCast(GameObject currentBin)
        {
            for (int i = 0; i < Bins.Count; i++)
            {
                if (Bins[i].gameObject != currentBin)
                {
                    Image image = Bins[i].gameObject.GetComponent<Image>();
                    if (image != null) image.raycastTarget = false;
                }
            }
        }
        public void EnableRayCast()
        {
            for (int i = 0; i < Bins.Count; i++)
            {
                Image image = Bins[i].gameObject.GetComponent<Image>();
                if (image != null) image.raycastTarget = true;
            }
        }
    }
}