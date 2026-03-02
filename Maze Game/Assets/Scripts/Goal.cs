using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Goal : MonoBehaviour
{
    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return; // ignore if already triggered

        if (other.gameObject.layer == 8)
        {
            hasTriggered = true;

            // stop player movement
            var playerGobj = GameObject.Find("Player");

            if (playerGobj == null)
                Debug.LogError("Couldn't find a Player in the level!");
            else 
            {
                var playerScript = playerGobj.GetComponent<Player>();
                playerScript.enabled = false;

                // start coroutine to play sound and load scene
                StartCoroutine(WinAndLoad());
            }
        }
    }

    IEnumerator WinAndLoad()
    {
        AudioClip clip = SoundManager.instance.winSound;

        SoundManager.instance.PlaySound(clip, true);

        yield return new WaitForSeconds(clip.length);

        SceneManager.LoadScene("Main Menu");
    }
}
