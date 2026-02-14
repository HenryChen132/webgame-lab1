using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Lab4MenuBinder : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject panelRoot;          // 拖 Lab4MenuPanel
    [SerializeField] private CameraLook cameraLook;         // 拖 Player 上的 CameraLook
    [SerializeField] private Slider sensitivitySlider;      // 拖 SensitivitySlider
    [SerializeField] private TMP_Text sensitivityValueText; // 拖 SensitivityValueText
    [SerializeField] private Button resetSensitivityButton; // 拖 Reset Sensitivity Button

    [Header("Sensitivity Range")]
    [SerializeField] private float minSens = 0.02f;
    [SerializeField] private float maxSens = 0.30f;

    [Header("Defaults")]
    [SerializeField] private float defaultSens = 0.08f;
    [SerializeField] private bool startMenuOpen = false; // 你要 false

    private bool menuOpen;

    private void Awake()
    {
        // ---- 绑定事件（运行时绑定就够交作业）----
        if (sensitivitySlider != null)
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);

        if (resetSensitivityButton != null)
            resetSensitivityButton.onClick.AddListener(ResetSensitivity);

        // ---- 初始化 Slider ----
        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = minSens;
            sensitivitySlider.maxValue = maxSens;

            float start = (cameraLook != null) ? cameraLook.Sensitivity : defaultSens;

            // 不触发事件的设置值（避免 Awake 时乱改）
            sensitivitySlider.SetValueWithoutNotify(start);
            UpdateSensitivityText(start);

            // 同步到 CameraLook（保险）
            if (cameraLook != null) cameraLook.Sensitivity = start;
        }

        // ---- 开局菜单状态 ----
        SetMenu(startMenuOpen);
    }

    private void Update()
    {
        // ESC：开关菜单（必须可来回切）
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

    // ✅ 必须 public：这样 Button OnClick 下拉里才选得到
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

        // 菜单开：解锁鼠标点 UI
        // 菜单关：锁鼠标控制相机
        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = open;
    }
}
