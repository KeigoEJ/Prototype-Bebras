using UnityEngine;

[CreateAssetMenu(fileName = "NewQuiz", menuName = "Quiz/Create New Quiz")]
public class QuizData : ScriptableObject
{
    [Tooltip("Unique ID for this quiz (e.g. \"Level1_Q1\").")]
    public string quizID;

    public Sprite  questionImage;
    public string  questionText;
    public string[] options = new string[4];
    public int     correctOptionIndex;
}
