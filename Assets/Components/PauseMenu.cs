using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] pauseMenus;
    [SerializeField] private GameObject pauseMenu;
    
    public void HandlePause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameManager.GameIsPaused)
            {
                foreach (var menu in pauseMenus)
                {
                    menu.SetActive(false);
                }
                GameManager.ResumeGame();
            }
            else
            {
                pauseMenu.SetActive(true);
                gameObject.SetActive(true);
                GameManager.PauseGame();
            }
        }
    }
}
