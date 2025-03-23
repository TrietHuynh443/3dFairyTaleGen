using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EventProcessing;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image _loadingIcon;
    [SerializeField] private CanvasGroup _loadingGroup;
    private Tween _loadingTween;

    private void OnEnable()
    {
        EventAggregator.Instance.AddEventListener<InitialLoadingFinish>(StopAndFadeLoadingScreen);
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

    void Start()
    {
        _loadingTween = _loadingIcon.transform.DORotate(new Vector3(0, 0, -360), 2f, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
