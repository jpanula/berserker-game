using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreen : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private void Start()
    {
        if (!GameManager.TutorialShown)
        {
            GameManager.PauseGame();
        }
        else
        {
            gameObject.transform.parent.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void TutorialExit()
    {
        GameManager.TutorialShown = true;
        animator.SetBool("Exit", true);
    }

    public void TutorialExitFinished()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(false);
        GameManager.ResumeGame();
    }
}
