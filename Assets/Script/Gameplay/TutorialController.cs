using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BioAdventure.Assets.Script.Core;
using BioAdventure.Assets.Script.Data;
using BioAdventure.Assets.Script.Managers;
using BioAdventure.Assets.Script.UI;
using TMPro;

namespace BioAdventure.Assets.Script.Gameplay
{
    public class TutorialController : MonoBehaviour
    {
        [Header("Referências de script")]
        private TextData textData = new TextData();
        [SerializeField] private GameController gameController;

        [Header("Referências da Cena")]
        [SerializeField] private GameObject trashPrefabPaper;
        [SerializeField] private GameObject trashPrefabPlastic;
        [SerializeField] private GameObject radialMenuObject;
        [SerializeField] private BinController binController;
        [SerializeField] private RadialMenuController radialMenuController;

        [Header("Referências da UI")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private GameObject _binPlayer;
        [SerializeField] private GameObject _binColection01;
        [SerializeField] private GameObject _binColection02;
        
        [Header("Posições Chave")]
        [SerializeField] private Vector3 spawnPos1 = new Vector3(-145, 350, 0);
        [SerializeField] private Vector3 spawnPos2 = new Vector3(-260, 350, 0);
        [SerializeField] private Vector3 spawnPos3 = new Vector3(-200, 350, 0);
        [SerializeField] private float waitPosY = 320f;
        [SerializeField] private float targetBinX1 = -145f;
        [SerializeField] private float targetBinX2 = -260f;
        [SerializeField] private float targetBinX3 = -200;

        // --- Flags de Controlo ---
        private bool _isWaitingForOk = false;
        private bool _wasBoosted = false;
        private bool _trashCollected = false;
        private GameObject _currentTrashInstance;
        private int _indexDialogue = 0;

        private void Awake()
        {
            binController.enabled = false;
            radialMenuController.enabled = false;
            dialoguePanel.SetActive(false);
            textData.StartTutorial();
        }

        private void Start()
        {
            StartCoroutine(TutorialSequence());
        }

        private void OnEnable()
        {
            // Ouve os eventos necessários
            InputManager.Instance.onSubmit += OnOkPressed; // Tecla Enter
            InputManager.Instance.onBoost += OnBoostPressed;
            TrashItem.OnCollected += OnTrashCollected;
        }

        private void OnDisable()
        {
            // Limpa os eventos
            InputManager.Instance.onSubmit -= OnOkPressed;
            InputManager.Instance.onBoost -= OnBoostPressed;
            TrashItem.OnCollected -= OnTrashCollected;
        }

        // --- O "CÉREBRO" DO TUTORIAL ---
        private IEnumerator TutorialSequence()
        {
            //espera fim do countDown
            yield return new WaitUntil(() => gameController.isGameRunning);

            // --- ETAPA 1: MOVIMENTAÇÃO ---
            yield return SpawnTrashAndWait(spawnPos1, waitPosY, true); // Spawna o lixo e espera ele cair e travar
            //ShowEmphasisAndWait(SpawnTrashAndWait - Dialogue 0 a 3)
            yield return ShowDialogueAndWait();
            yield return ShowDialogueAndWait();
            yield return ShowDialogueAndWait();
            yield return ShowDialogueAndWait();

            binController.enabled = true; // Destrava a lixeira
            yield return new WaitUntil(() => binController.transform.position.x >= targetBinX1); // Espera o jogador chegar

            binController.enabled = false; // Trava a lixeira
            ReleaseCurrentTrash(); // Solta o lixo

            yield return new WaitUntil(() => _trashCollected); // Espera a coleta
            _trashCollected = false; // Reseta a flag para a próxima etapa

            // --- ETAPA 2: BOOST ---
            //(Dialogue 4)
            yield return ShowDialogueAndWait();

            yield return SpawnTrashAndWait(spawnPos2, waitPosY, true); // Spawna o segundo lixo

            //(Dialogue 5 a 7)
            yield return ShowDialogueAndWait();
            yield return ShowDialogueAndWait();
            yield return ShowDialogueAndWait();

            yield return new WaitUntil(() => _wasBoosted); // Espera o jogador pressionar espaço

            binController.enabled = true; // Destrava a lixeira
            yield return new WaitUntil(() => binController.transform.position.x <= targetBinX2); // Espera o jogador chegar

            binController.enabled = false; // Trava a lixeira
            ReleaseCurrentTrash(); // Solta o lixo

            yield return new WaitUntil(() => _trashCollected); // Espera a coleta
            _trashCollected = false;

            // --- ETAPA 3: MENU RADIAL ---
            //(Dialogue 8)
            yield return ShowDialogueAndWait();

            binController.enabled = true;
            yield return SpawnTrashAndWait(spawnPos3, waitPosY, false); // Spawna o terceiro lixo, agora plastico
            yield return new WaitUntil(() => binController.transform.position.x >= targetBinX3); // Espera o jogador chegar
            binController.enabled = false;

            dialoguePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500, 300);

            //(Dialogue 9 a 12)
            yield return ShowDialogueAndWait();
            yield return ShowDialogueAndWait();

            
            yield return ShowDialogueAndWait();
            dialoguePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 300);

            _binPlayer.SetActive(false);//Desativa a UI do player
            _binColection01.SetActive(true);//Ativa exibição da das leixeiras junto ao texto
            yield return ShowDialogueAndWait();

            _binColection01.SetActive(false);//Desataiva a coleção 01
            _binColection02.SetActive(true);//Ativa a exeibição da colection 02 junto ao texto
            yield return ShowDialogueAndWait();

            _binColection02.SetActive(false);
            _binPlayer.SetActive(true);//retorna ao estado natural

            radialMenuObject.SetActive(true); // Força a abertura do menu para demonstração
            yield return new WaitForSeconds(3f); // Deixa o jogador ver o menu aberto
            radialMenuObject.SetActive(false); // Força o fechamento
            
            //(Dialogue 13 a 15)
            yield return ShowDialogueAndWait();
            yield return ShowDialogueAndWait();
            yield return ShowDialogueAndWait();

            radialMenuController.enabled = true;
            yield return new WaitUntil(() => binController.gameObject.CompareTag("plastic"));
            radialMenuController.enabled = false;

            ReleaseCurrentTrash();
            yield return new WaitUntil(() => _trashCollected);
            
            //(Dialogue 16)
            yield return ShowDialogueAndWait();
            // --- FIM ---
            GameManager.Instance.SaveProgress(); // Salva o progresso (ex: marcar tutorial como completo)
            SceneController.Instance.GoToMainMenu(); // Envia para o menu principal
        }


        // --- MÉTODOS DE EVENTOS (FLAGS) ---

        // Chamado pelo InputManager para avançar o diálogo
        private void OnOkPressed() { _isWaitingForOk = false; }

        // Chamado pelo InputManager quando o boost é usado em deu devido momento
        private void OnBoostPressed() { if(_indexDialogue >= 5)_wasBoosted = true; }

        // Chamado pelo evento estático do TrashItem quando um lixo é coletado
        private void OnTrashCollected(bool wasCorrect, string type) { _trashCollected = true; }


        // --- MÉTODOS DE AÇÃO (HELPERS) ---

        // Spawna um lixo e espera ele cair até a posição Y de espera
        private IEnumerator SpawnTrashAndWait(Vector3 spawnPos, float waitY, bool isPaper)
        {
            _currentTrashInstance = Instantiate((isPaper ? trashPrefabPaper : trashPrefabPlastic), spawnPos, Quaternion.identity);
            Rigidbody2D rb = _currentTrashInstance.GetComponent<Rigidbody2D>();
            rb.gravityScale = 3; // Define a gravidade

            // Espera até que o lixo caia até a altura definida
            yield return new WaitUntil(() => _currentTrashInstance.transform.position.y <= waitY);

            // Trava o lixo no ar
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
        }

        // Solta o lixo que está travado
        private void ReleaseCurrentTrash()
        {
            if (_currentTrashInstance != null)
            {
                _currentTrashInstance.GetComponent<Rigidbody2D>().gravityScale = 3;
            }
        }

        // Mostra uma mensagem e pausa a Coroutine principal até que o jogador aperte "OK"
        private IEnumerator ShowDialogueAndWait()
        {
            dialogueText.text = textData.tutorialString[_indexDialogue];
            _indexDialogue++;
            dialoguePanel.SetActive(true);
            _isWaitingForOk = true; // Ativa a flag de espera

            // A Coroutine vai ficar "presa" nesta linha até que OnOkPressed() mude a flag para false
            yield return new WaitUntil(() => !_isWaitingForOk);

            dialoguePanel.SetActive(false); // Esconde o painel
        }


        //Latter

        //ShowEmphasis

        //Animation Emphasis
    }
}