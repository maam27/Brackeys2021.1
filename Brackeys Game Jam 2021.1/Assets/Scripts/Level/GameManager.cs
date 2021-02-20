using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace Level
{
    public class GameManager : MonoBehaviour
    {
        //Menu Fade In
        //Game Over Stuff
        //Playable Director Management for intro at the start of the game
        //Slow down for pause menu on toggle

        private PlayableDirector m_MenuDirector;
        public PlayableAsset normalMenuFadeIn;
        public PlayableAsset gameOverMenuFadeIn;
        public PlayableAsset gameCompleteMenuFadeIn;
        [Space] public CanvasGroup menuGroup;

        private void Awake()
        {
            m_MenuDirector = transform.GetChild(0).GetComponent<PlayableDirector>();
            LevelManager.PublicAccess.ONGameOver += () => FullReset(gameOverMenuFadeIn);
            LevelManager.PublicAccess.ONGameComplete += () => FullReset(gameCompleteMenuFadeIn);


            m_MenuDirector.Play(normalMenuFadeIn);
        }


        #region Main Menu Methods

        public void StartGame()
        {
            if (menuGroup)
            {
                menuGroup.interactable = false;
                menuGroup.blocksRaycasts = false;
                menuGroup.gameObject.SetActive(false);
            }

            LevelManager.PublicAccess.StartGame();
        }


        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        #endregion


        private void FullReset(PlayableAsset overMenuFadeIn)
        {
            //Clear the level
            LevelManager.PublicAccess.DestroyAllRegistedObjects();
            LevelManager.PublicAccess.StopSpawningElements();

            Debug.Log("Resetting the game!");

            if (menuGroup)
            {
                menuGroup.interactable = true;
                menuGroup.blocksRaycasts = true;
                menuGroup.gameObject.SetActive(true);
            }

            //Open the main menu UI element
            if (overMenuFadeIn)
                m_MenuDirector.Play(overMenuFadeIn);
        }
    }
}