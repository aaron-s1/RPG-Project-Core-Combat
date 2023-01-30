using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoadIndex = -1;
        [SerializeField] Transform spawnPoint;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
                StartCoroutine(Transition());
        }

        IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneToLoadIndex);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayerPosition(otherPortal);

            Destroy(gameObject);
        }


        Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal != this)
                    return portal;
            }

            return null;
        }


        
        void UpdatePlayerPosition(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            
            // player.transform.position = otherPortal.spawnPoint.position;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);            
            player.transform.rotation = otherPortal.spawnPoint.rotation;

        }
    }
}