using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportTrigger : MonoBehaviour
{
    [Tooltip("Scene to load (must be in Build Settings)")]
    public string sceneName;

    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            QuizManager.Instance.ShowPrompt("Press E to teleport");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            QuizManager.Instance.HidePrompt();
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            QuizManager.Instance.HidePrompt();
            var player = FindObjectOfType<PlayerHandler>();
            if (player != null) player.canMove = false;
            SceneManager.LoadScene(sceneName);
        }
    }
}
