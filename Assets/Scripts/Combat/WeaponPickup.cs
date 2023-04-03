using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Attributes;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weaponPickup = null;
        [SerializeField] float healthToRestore = 0;
        [SerializeField] float respawnTime = 5f;


        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
                Pickup(other.gameObject);
        }

        private void Pickup(GameObject subject)
        {
            if (weaponPickup != null)
                subject.GetComponent<Fighter>().EquipWeapon(weaponPickup);

            if (healthToRestore > 0)
                subject.GetComponent<Health>().Heal(healthToRestore);

            StartCoroutine(HideForSeconds(respawnTime));
        }


        IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;

            foreach (Transform child in transform)
                child.gameObject.SetActive(shouldShow);
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
                Pickup(callingController.gameObject);

            return true;
        }

        public CursorType GetCursorType() =>
            CursorType.Pickup;
    }
}