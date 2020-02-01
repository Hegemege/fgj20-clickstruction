using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityUtilities;

public class ManaBar : MonoBehaviour
{
    public Image BarImage;

    private float _startValue;
    private float _currentValue;
    private float _targetValue;
    private float _maxValue;
    public float AnimationLength = 0.5f;
    public AnimationCurve AnimationCurve;

    private float _animationTimer;
    private Coroutine _animation;

    public TextMeshProUGUI ValueText;

    private void Awake()
    {
        SetAmount(30f, 100f);
    }

    public void SetAmount(float value, float max)
    {
        if (_animation != null)
        {
            StopCoroutine(_animation);
        }

        _startValue = _currentValue;
        _targetValue = value;
        _maxValue = max;

        _animationTimer = 0f;

        _animation = StartCoroutine(AnimateBar());
    }

    private IEnumerator AnimateBar()
    {
        while (_animationTimer < AnimationLength)
        {
            _animationTimer += Time.deltaTime;

            var t = Mathf.Clamp01(_animationTimer / AnimationLength);
            var animationT = AnimationCurve.Evaluate(t);

            _currentValue = Mathf.Lerp(_startValue, _targetValue, animationT);
            UpdateFillAmount();

            yield return null;
        }

        _currentValue = _targetValue;
        UpdateFillAmount();
    }

    private void UpdateFillAmount()
    {
        var t = Mathf.Clamp01(_currentValue / _maxValue);
        BarImage.fillAmount = t;

        // Update value text position and value
        ValueText.text = FloatStringCache.Get(_currentValue, 0);
        var y = BarImage.rectTransform.rect.yMin + BarImage.rectTransform.rect.height * t;

        ValueText.rectTransform.localPosition = new Vector3(ValueText.rectTransform.localPosition.x, y, ValueText.rectTransform.localPosition.z);
    }
}
