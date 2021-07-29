using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;


namespace RPG.Combat
{
    public class Pickups : MonoBehaviour
    {
        public Weapon weapon = null;
        public float HideTime = 2f;
        private void OnTriggerEnter(Collider other)
        {
       
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);

                StartCoroutine(HideForSeconds(HideTime));
            }
        }
        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        
        public void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform item in transform)
            {
                item.gameObject.SetActive(shouldShow);
            }
        }
    }
}


