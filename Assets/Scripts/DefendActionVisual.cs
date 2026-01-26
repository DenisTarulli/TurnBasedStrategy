using UnityEngine;

public class DefendActionVisual : MonoBehaviour
{
    [SerializeField] private GameObject defendBuffVisual;
    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private void Update()
    {
        defendBuffVisual.SetActive(unit.IsDefending());
    }
}
