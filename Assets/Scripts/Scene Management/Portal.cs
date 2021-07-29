using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {

        enum DestinationIdentifier
        {
            A,B,C,D,E
            
        }

        [SerializeField] DestinationIdentifier destination;
        public int SceneToLoad;
        public Transform SpawnPoint;
        public float FadeOutTime = 1f;
        public float FadeInTime = 2f;
        public float FadeWaitTime = 0.5f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(SceneTransition());
            }
        }

        private IEnumerator SceneTransition()
        {
            if (SceneToLoad < 0)
            {
                yield break;
            }
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(FadeOutTime);

            SavingWrapper saving = FindObjectOfType<SavingWrapper>();

            saving.Save();

            yield return SceneManager.LoadSceneAsync(SceneToLoad);

            saving.Load();
       
            Portal otherPortal = GetOtherPortal();
            FindPlayer(otherPortal);

            saving.Save();

            yield return new WaitForSeconds(FadeWaitTime);
            yield return fader.FadeIn(FadeInTime);

            

            Destroy(gameObject);
        }

        private void FindPlayer(Portal otherPortal)
        {
            GameObject player = GameObject.Find("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.SpawnPoint.position);
            player.transform.rotation = otherPortal.SpawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }
        private Portal GetOtherPortal()
        {
            foreach (Portal item in FindObjectsOfType<Portal>())
            {
                if (item == this) continue;
                if (item.destination != destination) continue;
                return item;


            }
            return null;
        }

        

        
    }
}
