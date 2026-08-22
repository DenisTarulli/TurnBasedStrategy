using System.Collections.Generic;
using UnityEngine;

/// Oculta (desactiva el Renderer) de las paredes (o alguna otra cosa que usen el layer "Wall") que se interponen 

public class CameraWallOcclusion : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private float targetHeightOffset = 1.5f;
    [SerializeField] private float cameraClipCheckRadius = 0.6f;

    private Transform followTarget;
    private readonly HashSet<Renderer> hiddenLastFrame = new HashSet<Renderer>();
    private readonly HashSet<Renderer> hiddenThisFrame = new HashSet<Renderer>();

    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void Start()
    {
        if (UnitActionSystem.Instance != null)
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;

            if (UnitActionSystem.Instance.GetSelectedUnit() != null)
            {
                followTarget = UnitActionSystem.Instance.GetSelectedUnit().transform;
            }
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, System.EventArgs e)
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        followTarget = selectedUnit != null ? selectedUnit.transform : null;
    }

    private void LateUpdate()
    {
        if (targetCamera == null || followTarget == null)
        {
            return;
        }

        hiddenThisFrame.Clear();

        Vector3 targetPoint = followTarget.position + Vector3.up * targetHeightOffset;
        Vector3 origin = targetCamera.transform.position;
        Vector3 direction = targetPoint - origin;
        float distance = direction.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(origin, direction.normalized, distance, wallLayerMask);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent<Renderer>(out Renderer renderer))
            {
                renderer.enabled = false;
                hiddenThisFrame.Add(renderer);
            }
        }

        // La camara quedo metida DENTRO de alguna pared (al acercarse/rotar mucho):
        // ocultamos tambien cualquier pared que este tocando la posicion de la camara.
        Collider[] overlaps = Physics.OverlapSphere(origin, cameraClipCheckRadius, wallLayerMask);

        foreach (Collider overlapCollider in overlaps)
        {
            if (overlapCollider.TryGetComponent<Renderer>(out Renderer renderer))
            {
                renderer.enabled = false;
                hiddenThisFrame.Add(renderer);
            }
        }

        // Vuelve a mostrar las que estaban ocultas el frame anterior y ya no bloquean la vista
        foreach (Renderer renderer in hiddenLastFrame)
        {
            if (renderer != null && !hiddenThisFrame.Contains(renderer))
            {
                renderer.enabled = true;
            }
        }

        hiddenLastFrame.Clear();
        hiddenLastFrame.UnionWith(hiddenThisFrame);
    }
}