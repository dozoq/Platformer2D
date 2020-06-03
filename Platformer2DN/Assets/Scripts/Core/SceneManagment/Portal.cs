using platformer.control;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace platformer.scenemanagment
{
    public class Portal : MonoBehaviour
    {
        /// <summary>
        /// IF YOU WANT TO ADD ANOTHER PORTAL DESTINATION, ADD IT TO THE END OF ENUM
        /// OR IT WILL BREAK PREVIOUS PORTALS
        /// </summary>
        enum PortalDestination
        {
            A,
            B
            //NEW DESTINATIONS HERE
        }

        [Tooltip("Scene that the portal wiil teleport to")]
        [SerializeField] private int sceneToLoad = -1;

        [Tooltip("Portal will look for another portal with same destination across given scene")]
        [SerializeField] PortalDestination destination;

        [Tooltip("Where will player be spawned after teleport")]
        [SerializeField] private Transform spawnTransform;

        [Tooltip("How long FadeOut effect will last")]
        [SerializeField] private float fadeOutTime = 1f;
        
        [Tooltip("Delay between FadeOut and FadeIn")]
        [SerializeField] private float waitTime = 0.5f;

        [Tooltip("How long FadeIn effect will last")]
        [SerializeField] private float fadeInTime = 1f;

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                StartCoroutine(Teleport());
            }
        }

        public IEnumerator Teleport()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not initialized in Portal: " + this.gameObject);
                yield break;
            }
            LoadingFader loadingFader = FindObjectOfType<LoadingFader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            DontDestroyOnLoad(this.gameObject);

            var playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.DisableControls();

            yield return loadingFader.FadeOut(fadeOutTime);

            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            // Disable controls for new player;
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.DisableControls();

            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayerPosition(otherPortal);

            savingWrapper.Save();

            yield return new WaitForSeconds(waitTime);

            //enable playercontroller
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.EnableControls();

            yield return loadingFader.FadeIn(fadeInTime);

            Destroy(this.gameObject);
            print("Should be destroyed");
        }

        /// <summary>
        /// Check throught all available portals in given sceneToLoad to match with destination
        /// </summary>
        /// <returns>Portal with same destination in given scene</returns>
        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;

                //This will allow to create portals across same scene
                if (portal.name == this.name && portal.sceneToLoad == this.sceneToLoad) continue; 

                if (portal.destination == this.destination)
                {
                    return portal;
                }
            }
            return null; // No portals with this destination
        }

        /// <summary>
        /// Teleport position to otherportal spawnpoint position
        /// </summary>
        /// <param name="otherPortal">Portal to be teleported to</param>
        private void UpdatePlayerPosition(Portal otherPortal)
        {
            var player = GameObject.FindWithTag("Player");
            print("Player position: " + player.transform.position);
            print("Should be teleportred to: " + otherPortal.spawnTransform.position);
            player.transform.position = otherPortal.spawnTransform.position;
        }

    }

}