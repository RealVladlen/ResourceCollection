using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class SettingsView : WindowView
    {   
        [SerializeField] private Button soundButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Transform soundButtonTransform;
        [SerializeField] private AnimationCurve scaleCurve;
        [SerializeField] private Color onColor;
        [SerializeField] private Color offColor;
        
        private TextMeshProUGUI _soundText;
        private bool _soundState;
        private Image _soundImage;
        private Tween _moveAnimation;

        public override void Awake()
        {
            base.Awake();
            _soundImage = soundButtonTransform.GetComponent<Image>();
            _soundText = soundButtonTransform.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            Show();

            _soundState = GameDataManager.Instance.Sound == 1;
            ApplySoundStateInstantly();

            soundButton.onClick.AddListener(SoundChange);
            closeButton.onClick.AddListener(CloseWindow);
            
            GameEventManager.CameraMoverStateMethod(true);
            GameEventManager.PlayerMoverStateMethod(false);
        }

        private void SoundChange()
        {
            _soundState = !_soundState;
            ViewUpdate();

            GameDataManager.Instance.Sound = _soundState ? 1 : 0;
            GameDataManager.Instance.SaveData();
        }

        private void ViewUpdate()
        {
            float duration = 0.1f;
            
            _moveAnimation?.Kill();
            _moveAnimation = null;
            
            float targetPosition = _soundState ? -45f : 45f;
            Color targetColor = _soundState ? onColor : offColor;
            string targetText = _soundState ? "ON" : "OFF";

            _moveAnimation = soundButtonTransform.DOLocalMoveX(targetPosition, duration).SetEase(scaleCurve);
            _soundImage.DOColor(targetColor, duration);
            _soundText.text = targetText;
        }

        private void ApplySoundStateInstantly()
        {
            float targetPosition = _soundState ? -45f : 45f;
            Color targetColor = _soundState ? onColor : offColor;
            string targetText = _soundState ? "ON" : "OFF";

            soundButtonTransform.localPosition = new Vector3(targetPosition, soundButtonTransform.localPosition.y, soundButtonTransform.localPosition.z);
            _soundImage.color = targetColor;
            _soundText.text = targetText;
        }

        private void CloseWindow()
        {
            Hide(() =>
            {
                GameEventManager.CameraMoverStateMethod(false);
                GameEventManager.PlayerMoverStateMethod(true);
            });
        }
        
        private void OnDisable()
        {
            soundButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
            
            GameEventManager.CameraMoverStateMethod(false);
            GameEventManager.PlayerMoverStateMethod(true);
            
            _moveAnimation?.Kill();
            _moveAnimation = null;
        }
    }
}
