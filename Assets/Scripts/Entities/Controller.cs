using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Entities
{
    public class Controller : MonoBehaviour
    {
         private Transform target;
        
        private Vector3 _initPosition;
        private Vector3 _tmpPostion;
        
        private float _distance;
        private float _moveSpeed = 5;

        private bool isMovingToTarget = true;
        private void Start()
        {
            _distance = 1f;
            _initPosition = transform.position;
            _tmpPostion = Vector3.zero;
        }

        public void UpdateMovement()
        {
            if (isMovingToTarget)
            {
                MoveTo(target.position, _distance);
            }
            else
            {
                GoBack();
            }
        }

        private void MoveTo(Vector3 destination, float distance)
        {
            if (Vector3.Distance(transform.position, destination) > distance)
            {
                _tmpPostion = destination;
                Vector3 direction = (_tmpPostion - transform.position).normalized;
                transform.Translate(direction * (Time.deltaTime * _moveSpeed));
            }
            else
            {
                isMovingToTarget = false;
            }
        }

        private void GoBack()
        {
            if (Vector3.Distance(transform.position, _initPosition) > 0.1f)
            {
                Vector3 direction = (_initPosition - transform.position).normalized;
                transform.Translate(direction * (Time.deltaTime * _moveSpeed));
            }
            else
            {
                isMovingToTarget = true;
            }
        }
    }
}