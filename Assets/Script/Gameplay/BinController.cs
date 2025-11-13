using UnityEngine;
using System.Collections;
using BioAdventure.Assets.Script.Core;
using System.Collections.Generic;

// BinController.cs
/*
Esse script será responsável por controlar o comportamento da lixeira (bin).
Movimentação, a mecânica de boost e a alteração de seu tipo com base no input do jogador.
*/



public class BinController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer binSprite;
    [SerializeField] private List<Sprite> binCollectionSprites;
    [SerializeField] private GameObject binCaptcha;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 70f;
    [SerializeField] private Vector2 _limitMap;

    [Header("Boost")]
    [SerializeField] private float _boostAmount = 3f;
    [SerializeField] private float _boostDuration = 0.2f;

    private bool _isBoosting = false;

    
    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.onMove += HandleMovement;
            InputManager.Instance.onBoost += HandleBoost;
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

    public void ChangeBinType(string trashtype)
    {
        switch (trashtype)
        {
            case "paper": gameObject.tag = trashtype; binSprite.sprite = binCollectionSprites[0]; break;
            case "radioactive": gameObject.tag = trashtype; binSprite.sprite = binCollectionSprites[1]; break;
            case "plastic": gameObject.tag = trashtype; binSprite.sprite = binCollectionSprites[2]; break;
            case "organic": gameObject.tag = trashtype; binSprite.sprite = binCollectionSprites[3]; break;
            case "glass": gameObject.tag = trashtype; binSprite.sprite = binCollectionSprites[4]; break;
            case "hospital": gameObject.tag = trashtype; binSprite.sprite = binCollectionSprites[5]; break;
            case "metal": gameObject.tag = trashtype; binSprite.sprite = binCollectionSprites[6]; break;
            case "wood": gameObject.tag = trashtype; binSprite.sprite = binCollectionSprites[7]; break;
        }
        binCaptcha.tag = gameObject.tag;
    }

    private void HandleMovement(float moveInput)
    {
        if (transform.position.x <= _limitMap.x && moveInput < 0)
        {
            transform.position = new Vector3(_limitMap.y,transform.position.y, transform.position.z);
        } if(transform.position.x >= _limitMap.y && moveInput > 0)
        {
            transform.position = new Vector3(_limitMap.x,transform.position.y, transform.position.z);
        }
        transform.Translate(Vector3.right * moveInput * _moveSpeed * Time.deltaTime);
    }

    private void HandleBoost()
    { 
        if (!_isBoosting)
        {
            StartCoroutine(BoostRoutine());
        }
    }
    private IEnumerator BoostRoutine()
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

    //Função para inverter a posição da lixeira quando quebrar o horizonte
}
