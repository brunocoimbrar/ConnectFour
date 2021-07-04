using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ConnectFour
{
    [Serializable]
    public sealed class UISystem : IDisposable
    {
        public event UnityAction OnRestartButtonClick
        {
            add => _restartButton.onClick.AddListener(value);
            remove => _restartButton.onClick.RemoveListener(value);
        }

        [SerializeField]
        private GameObject _drawMoveFeedback;
        [SerializeField]
        private GameObject _winMoveFeedback;
        [SerializeField]
        private GameObject _invalidMoveFeedback;
        [SerializeField]
        private float _invalidMoveFeedbackDuration = 2;
        [SerializeField]
        private Button _restartButton;
        [SerializeField]
        private Image _discImage;
        [SerializeField]
        private TextMeshProUGUI _controllerText;
        [SerializeField]
        private string _controllerTextFormat = "Controller {0} ({1})";

        private IWorldContext _worldContext;

        public void Dispose()
        {
            _restartButton.onClick.RemoveAllListeners();
        }

        public void Initialize(IWorldContext worldContext)
        {
            _worldContext = worldContext;
            _drawMoveFeedback.SetActive(false);
            _winMoveFeedback.SetActive(false);
            _invalidMoveFeedback.SetActive(false);
            _discImage.color = Color.clear;
            _controllerText.text = string.Empty;
        }

        public void SetController(int controllerNumber, string controllerName, Color discColor)
        {
            _discImage.color = discColor;
            _controllerText.text = string.Format(_controllerTextFormat, controllerNumber, controllerName);
        }

        public void SetDrawMoveFeedbackActive()
        {
            _drawMoveFeedback.SetActive(true);
        }

        public void SetInvalidMOveFeedbackActive()
        {
            IEnumerator coroutine()
            {
                _invalidMoveFeedback.SetActive(true);

                yield return new WaitForSeconds(_invalidMoveFeedbackDuration);

                _invalidMoveFeedback.SetActive(false);
            }

            _worldContext.StartCoroutine(coroutine());
        }

        public void SetWinMoveFeedbackActive()
        {
            _winMoveFeedback.SetActive(true);
        }
    }
}
