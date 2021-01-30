using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class CameraPosition : MonoBehaviour
{
    private EventManager em;
    public float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    private bool moving = false;
    private Vector3 targetPosition;
    private float closeDistance = 0.01f;

    private void Awake()
    {
        em = EventManager.GetInstance();
        em.AddListener<CharacterStartPosEvent>(OnCharacterStartPos);
    }

    private void OnCharacterStartPos(CharacterStartPosEvent e)
    {
        targetPosition = e.position;
        targetPosition.z = -8;
        moving = true;
    }

    private void Update()
    {
        if (moving)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            Vector3 offset = transform.position - targetPosition;
            float sqrLen = offset.sqrMagnitude;
            if (sqrLen < closeDistance * closeDistance)
            {
                moving = false;
            }
        }
    }

}
