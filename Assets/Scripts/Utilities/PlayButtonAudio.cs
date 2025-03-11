using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    [RequireComponent(typeof(Button))]
    public class PlayButtonAudio : MonoBehaviour
    {
        [Header("Audio Settings")]
        [SerializeField] private string clipName = "ui_buttons";
        [SerializeField] private bool callManually = false;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            if (!callManually)
                _button.onClick.AddListener(PlayAudio);
        }

        private void OnDisable()
        {
            if (!callManually)
                _button.onClick.RemoveListener(PlayAudio);
        }

        public void PlayAudio()
        {
            if (!string.IsNullOrEmpty(clipName))
            {
                AudioManager.Instance.PlayAudioClip(clipName);
            }
        }
    }
}