using UnityEngine;
using System.Collections;
using BioAdventure.Assets.Script.Core;
using System.Collections.Generic;

// BinController.cs
/*
Esse script será responsável por controlar o comportamento da lixeira (bin).
Movimentação, a mecânica de boost e a alteração de seu tipo com base no input do jogador.
*/



public class BinController : MonoBehaviour, IBinController
{
    [Header("References")]
    [SerializeField] private SpriteRenderer binSprite;
    [SerializeField] private List<Sprite> binCollectionSprites;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 7f;

    [Header("Boost")]
    [SerializeField] private float _boostAmount = 2f;
    [SerializeField] private float _boostDuration = 0.2f;


    private int _moveDirection = 0;
    private bool _isBoosting = false;


    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMove += HandleMovement;
            InputManager.Instance.OnBoost += HandleBoost;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.onMove -= HandleMovement;
            InputManager.Instance.onBoost -= HandleBoost;
        }
    }

    private void Update()
    {

    }

    public void ChangeBinType(char typeCharacter)
    {
        switch (typeCharacter)
        {
            case 'q': gameObject.tag = "Paper"; binSprite.sprite = binCollectionSprites[0]; break;
            case 'w': gameObject.tag = "Plastic"; binSprite.sprite = binCollectionSprites[1]; break;
            case 'e': gameObject.tag = "Glass"; binSprite.sprite = binCollectionSprites[2]; break;
            case 'r': gameObject.tag = "Metal"; binSprite.sprite = binCollectionSprites[3]; break;
            case 'a': gameObject.tag = "Organic"; binSprite.sprite = binCollectionSprites[4]; break;
            case 's': gameObject.tag = "Hospital"; binSprite.sprite = binCollectionSprites[5]; break;
            case 'd': gameObject.tag = "Wood"; binSprite.sprite = binCollectionSprites[6]; break;
            case 'f': gameObject.tag = "Radioactive"; binSprite.sprite = binCollectionSprites[7]; break;
        }
    }

    private void HandleMovement(int moveInput)
    {
       transform.Translate(Vector3.right * moveInput * moveSpeed * Time.deltaTime);
    }

    private void HandleBoost()
    { 
        if (!_isBoosting)
        {
            StartCoroutine(BoostRoutine());
        }
    }
    private void BoostRoutine()
    {
        _isBoosting = true;
        float timer = 0f;
        float originalSpeed = _moveSpeed;

        while (timer < _boostDuration)
        {
            _moveSpeed = originalSpeed * _boostAmount;
            timer += Time.deltaTime;
            yield return null;
        }
        _isBoosting = false;
        _moveSpeed = originalSpeed;
    }
}
