using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Provee feedback visual (sacudida / shake y parpadeo de color / color flash)
/// a elementos de UI usando corrutinas, sin sobrecargar Update.
/// </summary>
public class UIJuiceFeedback : MonoBehaviour
{
    [Header("Component References")]
    [Tooltip("RectTransform a sacudir. Si está vacío, usa el del mismo GameObject.")]
    [SerializeField] private RectTransform targetRectTransform;

    [Tooltip("Textos a los que aplicar el parpadeo de color.")]
    [SerializeField] private List<TextMeshProUGUI> targetTexts = new List<TextMeshProUGUI>();

    [Tooltip("Imágenes a las que aplicar el parpadeo de color.")]
    [SerializeField] private List<Image> targetImages = new List<Image>();

    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeStrength = 8f;

    [Header("Color Flash Settings")]
    [SerializeField] private Color flashColor = new Color(0.9f, 0.2f, 0.2f, 1f); // Rojo de advertencia
    [SerializeField] private float flashDuration = 0.3f;

    public event System.Action OnJuiceCompleted;

    private Vector3 shakeBasePosition;
    private bool isShaking = false;
    private bool isFlashing = false;

    private Dictionary<TextMeshProUGUI, Color> flashBaseTextColors = new Dictionary<TextMeshProUGUI, Color>();
    private Dictionary<Image, Color> flashBaseImageColors = new Dictionary<Image, Color>();

    private Coroutine shakeCoroutine;
    private Coroutine flashCoroutine;
    private bool isInitialized = false;

    private void Awake()
    {
        InitializeIfNeeded();
    }

    private void OnEnable()
    {
        InitializeIfNeeded();
    }

    private void OnDisable()
    {
        ResetToOriginalState();
    }

    private void InitializeIfNeeded()
    {
        if (isInitialized) return;

        if (targetRectTransform == null)
        {
            targetRectTransform = GetComponent<RectTransform>();
        }

        // Si no se asignaron manualmente en el Inspector, busca componentes locales
        if (targetTexts.Count == 0 && targetImages.Count == 0)
        {
            TextMeshProUGUI localText = GetComponent<TextMeshProUGUI>();
            if (localText != null) targetTexts.Add(localText);

            Image localImage = GetComponent<Image>();
            if (localImage != null) targetImages.Add(localImage);
        }

        isInitialized = true;
    }

    /// <summary>
    /// Dispara tanto la sacudida como el parpadeo de color en paralelo.
    /// </summary>
    public void PlayJuice()
    {
        PlayShake();
        PlayColorFlash();
    }

    /// <summary>
    /// Inicia el efecto de sacudida (Shake). Si ya había uno en curso, lo reinicia de forma limpia.
    /// </summary>
    public void PlayShake()
    {
        if (!gameObject.activeInHierarchy || targetRectTransform == null) return;

        if (isShaking)
        {
            // Si ya se estaba sacudiendo, detenemos la corrutina y volvemos a la base guardada
            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine);
            }
            targetRectTransform.localPosition = shakeBasePosition;
        }
        else
        {
            // Guardamos la posición actual real en la que el Layout Group lo posicionó
            shakeBasePosition = targetRectTransform.localPosition;
            isShaking = true;
        }

        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    /// <summary>
    /// Inicia el efecto de cambio y recuperación de color (Color Flash).
    /// </summary>
    public void PlayColorFlash()
    {
        if (!gameObject.activeInHierarchy) return;

        if (isFlashing)
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            RestoreFlashBaseColors();
        }
        else
        {
            // Capturar el color actual que tenga el texto o imagen (por ejemplo gris si está deshabilitado)
            flashBaseTextColors.Clear();
            foreach (TextMeshProUGUI text in targetTexts)
            {
                if (text != null && !flashBaseTextColors.ContainsKey(text))
                {
                    flashBaseTextColors[text] = text.color;
                }
            }

            flashBaseImageColors.Clear();
            foreach (Image img in targetImages)
            {
                if (img != null && !flashBaseImageColors.ContainsKey(img))
                {
                    flashBaseImageColors[img] = img.color;
                }
            }

            isFlashing = true;
        }

        flashCoroutine = StartCoroutine(ColorFlashRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            // Desplazamiento aleatorio de izquierda a derecha (eje X principal)
            float randomX = Random.Range(-1f, 1f) * shakeStrength;
            targetRectTransform.localPosition = shakeBasePosition + new Vector3(randomX, 0f, 0f);

            yield return null;
        }

        // Regresar con precisión milimétrica a su posición asignada en el grid
        targetRectTransform.localPosition = shakeBasePosition;
        isShaking = false;
        shakeCoroutine = null;
    }

    private IEnumerator ColorFlashRoutine()
    {
        // 1. Aplicar el color de advertencia instantáneamente
        SetColorToAllTargets(flashColor);

        // 2. Transicionar de vuelta suavemente al color base que tenía (ej. gris) usando Lerp
        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / flashDuration);

            foreach (var kvp in flashBaseTextColors)
            {
                if (kvp.Key != null)
                {
                    kvp.Key.color = Color.Lerp(flashColor, kvp.Value, t);
                }
            }

            foreach (var kvp in flashBaseImageColors)
            {
                if (kvp.Key != null)
                {
                    kvp.Key.color = Color.Lerp(flashColor, kvp.Value, t);
                }
            }

            yield return null;
        }

        RestoreFlashBaseColors();
        isFlashing = false;
        flashCoroutine = null;

        OnJuiceCompleted?.Invoke();
    }

    private void SetColorToAllTargets(Color color)
    {
        foreach (var kvp in flashBaseTextColors)
        {
            if (kvp.Key != null) kvp.Key.color = color;
        }

        foreach (var kvp in flashBaseImageColors)
        {
            if (kvp.Key != null) kvp.Key.color = color;
        }
    }

    private void RestoreFlashBaseColors()
    {
        foreach (var kvp in flashBaseTextColors)
        {
            if (kvp.Key != null) kvp.Key.color = kvp.Value;
        }

        foreach (var kvp in flashBaseImageColors)
        {
            if (kvp.Key != null) kvp.Key.color = kvp.Value;
        }
    }

    private void ResetToOriginalState()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

        if (targetRectTransform != null && isShaking)
        {
            targetRectTransform.localPosition = shakeBasePosition;
            isShaking = false;
        }

        if (isFlashing)
        {
            RestoreFlashBaseColors();
            isFlashing = false;
        }
    }
}
