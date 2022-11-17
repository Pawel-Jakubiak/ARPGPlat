using UnityEngine;
using TMPro;

public class HealthbarController : MonoBehaviour
{
    public static HealthbarController Instance;

    [SerializeField] private RectTransform _slider;
    [SerializeField] private TextMeshProUGUI _textMesh;

    private float _maxSliderWidth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }

        _maxSliderWidth = _slider.sizeDelta.x;
    }

    public void SetHealthbar(int currentHealth, int maxHealth)
    {
        _textMesh.SetText($"{currentHealth}/{maxHealth}");

        float sliderValue = (float) currentHealth / (float) maxHealth;

        _slider.sizeDelta = new Vector2(_maxSliderWidth * sliderValue, _slider.sizeDelta.y);
    }
}
