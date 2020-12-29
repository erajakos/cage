using System;
using UnityEngine;
using Events;

namespace Characters
{
    public class Character
    {
        public Vector2 cellPos;

        public string characterId = Guid.NewGuid().ToString("N");

        private GameObject gameObject;

        public Character(string sprite)
        {
            gameObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Character"));
            Sprite characterSprite = Resources.Load<Sprite>("Sprites/Characters/" + sprite);
            gameObject.GetComponent<SpriteRenderer>().sprite = characterSprite;

            EventManager em = EventManager.GetInstance();
            em.AddListener<CharacterTurnStartEvent>(OnCharacterTurnStart);

            CharacterMovement cm = gameObject.GetComponent<CharacterMovement>();
            cm.character = this;
            cm.enabled = false;
        }

        private void OnCharacterTurnStart(CharacterTurnStartEvent e)
        {
            if (ReferenceEquals(e.character, this))
            {
                gameObject.GetComponent<CharacterMovement>().enabled = true;
            } else
            {
                gameObject.GetComponent<CharacterMovement>().enabled = false;
            }
        }
    }
}