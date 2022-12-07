using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class EnemyEditorWindow : EditorWindow
{
    private static SerializedObject _serializedObject;

    public static void ShowWindow()
    {
        EnemyEditorWindow window = GetWindow<EnemyEditorWindow>("Enemy Editor");
    }


    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Scripts/Editor/EnemyEditorWindow.uxml");
        VisualElement view = visualTree.Instantiate();

        if (Selection.activeObject)
        {
            _serializedObject = new SerializedObject(Selection.activeObject);

            view.Bind(_serializedObject);
        }

        Toggle stationaryToggle = view.Q<Toggle>("stationary");
        stationaryToggle.RegisterValueChangedCallback(OnStationaryValueChanged);

        EnumField sensorEnumField = view.Q<EnumField>("sensorType");
        sensorEnumField.RegisterValueChangedCallback(OnSensorTypeValueChanged);

        Slider updateRateSlider = view.Q<Slider>("updateRate");
        updateRateSlider.RegisterValueChangedCallback(OnUpdateRateValueChanged);

        root.Add(view);
    }

    private void OnUpdateRateValueChanged(ChangeEvent<float> evt)
    {
        Slider updateRateSlider = evt.currentTarget as Slider;

        // Setting step size for update rate slider
        updateRateSlider.value = UnityEngine.Mathf.Round(evt.newValue / .1f) * .1f;
    }

    private void OnStationaryValueChanged(ChangeEvent<bool> evt) {
        VisualElement moveSpeedField = rootVisualElement.Q<FloatField>("movementSpeed");
        moveSpeedField.SetEnabled(!evt.newValue);
    }

    private void OnSensorTypeValueChanged(ChangeEvent<Enum> evt)
    {
        VisualElement angleSlider = rootVisualElement.Q<SliderInt>("angle");
        AISensor.SensorType value;

        if (Enum.TryParse(evt.newValue.ToString(), out value))
        {
            bool isCone = value == AISensor.SensorType.CONE ? true : false;
            angleSlider.SetEnabled(isCone);
        }
    }
}
