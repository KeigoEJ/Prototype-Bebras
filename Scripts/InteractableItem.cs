using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public QuizData quizData;     // assign in Inspector

    private bool used = false;
    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            QuizManager.Instance.ShowPrompt("Press E to interact");
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
        if (!used && playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            used = true;
            QuizManager.Instance.HidePrompt();
            QuizManager.Instance.StartQuiz(quizData);
        }
    }
}
