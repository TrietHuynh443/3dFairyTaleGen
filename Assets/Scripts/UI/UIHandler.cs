using DG.Tweening;
using EventProcessing;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image _loadingIcon;
    [SerializeField] private CanvasGroup _loadingGroup;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    private Tween _loadingTween;
    
    private void OnEnable()
    {
        _nextButton.onClick.AddListener(ToNextScene);
        _previousButton.onClick.AddListener(ToPreviousScene);
        EventAggregator.Instance.AddEventListener<InitialLoadingFinish>(StopAndFadeLoadingScreen);
    }

    private void ToPreviousScene()
    {
        EventAggregator.Instance.RaiseEvent(new OnChangeParagraphEvent()
        {
            IsNext = false
        });
    }

    private void ToNextScene()
    {
        EventAggregator.Instance.RaiseEvent(new OnChangeParagraphEvent()
        {
            IsNext = true
        });
    }

    private void OnDisable()
    {
        EventAggregator.Instance?.RemoveEventListener<InitialLoadingFinish>(StopAndFadeLoadingScreen);
    }

    private void StopAndFadeLoadingScreen(InitialLoadingFinish obj)
    {
        _loadingTween?.Kill();
        _loadingGroup.DOFade(0, 1f);
    }

    private void Start()
    {
        _loadingTween = _loadingIcon.transform.DORotate(new Vector3(0, 0, -360), 2f, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart);
        _previousButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        _nextButton.interactable = InitialLoader.CurrentScene < InitialLoader.MaxScene;
        _previousButton.interactable = InitialLoader.CurrentScene > 0;
    }
}
