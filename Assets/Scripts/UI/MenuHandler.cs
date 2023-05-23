using Pang.Levels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Animation = Pang.Animations.Animation;

namespace Pang.UI
{
    internal sealed class MenuHandler : MonoBehaviour
    {
        [SerializeField] private LevelsLoader levelsLoader;
        [SerializeField] private Slider levelsSlider;
        [SerializeField] private Button startButton;
        [SerializeField] private Animation closeAnimation;
        [SerializeField] private SceneUtils sceneUtils;
        private bool gameStarted;

        private void Start()
        {
            gameStarted = false;
            levelsSlider.maxValue = levelsLoader.NumLevels - 1;
            levelsSlider.wholeNumbers = true;

            startButton.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            if (gameStarted)
            {
                sceneUtils.RestartScene();
                return;
            }
            closeAnimation.StartAnimation();
            startButton.GetComponentInChildren<TMP_Text>().text = "Restart";
            levelsLoader.LoadLevel((int)levelsSlider.value);
            gameStarted = true;
        }
    }
}