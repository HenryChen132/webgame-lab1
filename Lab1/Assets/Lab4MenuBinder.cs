using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Lab4MenuBinder : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject panelRoot;          //  Lab4MenuPanel
    [SerializeField] private CameraLook cameraLook;         //  Player's CameraLook
    [SerializeField] private Slider sensitivitySlider;      // SensitivitySlider
    [SerializeField] private TMP_Text sensitivityValueText; // SensitivityValueText
    [SerializeField] private Button resetSensitivityButton; // Reset Sensitivity Button

    [Header("Sensitivity Range")]
    [SerializeField] private float minSens = 0.02f;
    [SerializeField] private float maxSens = 0.30f;

    [Header("Defaults")]
    [SerializeField] private float defaultSens = 0.08f;
    [SerializeField] private bool startMenuOpen = false; 

    private bool menuOpen;

    private void Awake()
    {

        if (sensitivitySlider != null)
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);

        if (resetSensitivityButton != null)
            resetSensitivityButton.onClick.AddListener(ResetSensitivity);

        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = minSens;
            sensitivitySlider.maxValue = maxSens;

            float start = (cameraLook != null) ? cameraLook.Sensitivity : defaultSens;

            sensitivitySlider.SetValueWithoutNotify(start);
            UpdateSensitivityText(start);

     
            if (cameraLook != null) cameraLook.Sensitivity = start;
        }


        SetMenu(startMenuOpen);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SetMenu(!menuOpen);
        }
    }

    private void OnSensitivityChanged(float v)
    {
        if (cameraLook != null)
            cameraLook.Sensitivity = v;

        UpdateSensitivityText(v);
    }

    private void UpdateSensitivityText(float v)
    {
        if (sensitivityValueText != null)
            sensitivityValueText.text = v.ToString("0.00");
    }

    public void ResetSensitivity()
    {
        float v = defaultSens;

        if (cameraLook != null)
            cameraLook.Sensitivity = v;

        if (sensitivitySlider != null)
            sensitivitySlider.SetValueWithoutNotify(v);

        UpdateSensitivityText(v);
    }

    private void SetMenu(bool open)
    {
        menuOpen = open;

        if (panelRoot != null)
            panelRoot.SetActive(open);

        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = open;
    }
}
