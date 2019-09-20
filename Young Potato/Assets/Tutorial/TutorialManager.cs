using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{

    public List<Tutorial> Tutorials = new List<Tutorial>();
    public Text explanationText;

    private static TutorialManager instance;
    public static TutorialManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<TutorialManager>();

            if (instance == null)
                Debug.Log("There is no TutorialManager");

            return instance;
    
        }
    }

    private Tutorial currentTutorial;

    // Start is called before the first frame update
    void Start()
    {
        SetNextTutorial(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTutorial)
            currentTutorial.CheckIfHappening();
    }

    public void CompletedTutorial()
    {
        SetNextTutorial(currentTutorial.Order + 1);
    }

    public void SetNextTutorial(int currentOrder)
    {
        currentTutorial = GetTutorialByOrder(currentOrder);

        if (!currentTutorial)
        {
            //CompletedAllTutoirals();
            return;
        }

        explanationText.text = currentTutorial.Explanation;
    }

    public void CompletedAllTutorials()
    {
        explanationText.text = "You've made it too the end! Hope you took note of everything. Press 'BACK' to back to the Menu.";

        if (Input.GetKeyUp(KeyCode.M))
            SceneManager.LoadScene(0);
    }

    public Tutorial GetTutorialByOrder(int Order)
    {
        for (int i = 0; i < Tutorials.Count; i++)
        {
            if (Tutorials[i].Order == Order)
            {
                return Tutorials[i];
            }
        }

        return null;
    }
}
