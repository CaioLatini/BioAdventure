using UnityEngine;
using System.Collections;

namespace BioAdventure.Assets.Script.Game
{
    public class BinController : MonoBehaviour
    {
        [Header("Move o recipiente (bin) lateralmente e aplica impulso quando solicitado")]
        
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 7f;
        [SerializeField] private float _wrapX = 9f;

        [Header("Boost")]
        [SerializeField] private float _boostAmount = 2f;
        [SerializeField] private float _boostDuration = 0.2f;

        private int _direction = 1;
        private bool _isBoosting;

        private void Update()
        {
            HandleMovement();
            HandleBoost();
            CheckWrap();
        }

        private void HandleMovement()
        {
            float input = 0f;
            if (Input.GetKey(KeyCode.LeftArrow)) { input = -1f; _direction = -1; }
            if (Input.GetKey(KeyCode.RightArrow)) { input = 1f; _direction = 1; }
            transform.Translate(Vector3.right * input * _moveSpeed * Time.deltaTime);
        }

        private void CheckWrap()
        {
            var pos = transform.position;
            if (pos.x > _wrapX) pos.x = -_wrapX;
            else if (pos.x < -_wrapX) pos.x = _wrapX;
            transform.position = pos;
        }

        private void HandleBoost()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !_isBoosting)
                StartCoroutine(BoostRoutine());
        }

        private IEnumerator BoostRoutine()
        {
            _isBoosting = true;
            float elapsed = 0f;
            float speedPerSec = _boostAmount / _boostDuration;

            while (elapsed < _boostDuration)
            {
                transform.Translate(Vector3.right * _direction * speedPerSec * Time.deltaTime);
                elapsed += Time.deltaTime;
                yield return null;
            }
            _isBoosting = false;
        }
    }
}