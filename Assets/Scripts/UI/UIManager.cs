using Assets.Scripts.Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private TextMeshProUGUI _stepsCounter, _starCounter;
        [SerializeField] private Button _restartBtn;        

        public void UpdateStarsCount(int amnt)
        {
            _starCounter.text = "Stars: " + amnt;
        }

        public void UpdateStepsCount(int amnt)
        {
            _stepsCounter.text = "Steps: " + amnt;
        }

        public void RestartBtnClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void GameOver(BaseEventParams eventParams)
        {
            _restartBtn.gameObject.SetActive(true);
        }

        private void Start()
        {
            _restartBtn.gameObject.SetActive(false);
            EventBus.Subscribe(GameplayEvent.GameOver, GameOver);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe(GameplayEvent.GameOver, GameOver);
        }
    }
}
