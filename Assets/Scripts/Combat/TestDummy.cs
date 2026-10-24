using System.Xml.Linq;
using UnityEngine;

/// <summary>
/// Visual feedback for testing before there's any real UI:
/// flashes white when hit and shifts toward blue as charge stacks build.
/// Put it on a cube or capsule along with Health and Charge.
/// </summary>

public class TestDummy : MonoBehaviour
{
	[SerializeField] private Renderer targetRenderer;
	[SerializeField] private Color baseColor = new Color(0.8f, 0.3f, 0.3f);
	[SerializeField] private Color chargedColor = new Color(0.2f, 0.6f, 1f);
	[SerializeField] private Color hitFlashColor = Color.white;
	[SerializeField] private float flashDuration = 0.08f;

	private Health health;
	private Charge charge;
	private Material material;
	private float flashTimer;

	private void Awake()
	{
		health = GetComponent<Health>();
		charge = GetComponent<Charge>();
        if (targetRenderer == null) targetRenderer = GetComponentInChildren<Renderer>();
        material = targetRenderer.material; // makes a per-dummy copy
    }

    private void OnEnable()
    {
        health.Damaged += OnDamaged;
        charge.StacksChanged += OnStacksChanged;
    }

    private void OnDisable()
    {
        health.Damaged -= OnDamaged;
        charge.StacksChanged -= OnStacksChanged;
    }

    private void Update()
    {
        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0f) ApplyChargeColor();
        }
    }

    private void OnDamaged(float amount, GameObject source)
    {
        Debug.Log($"{name} took {amount} damage. Health: {health.Current}/{health.Max}");
        flashTimer = flashDuration;
        material.color = hitFlashColor;
    }

    private void OnStacksChanged(int stacks)
    {
        if (flashTimer <= 0f) ApplyChargeColor();
    }

    private void ApplyChargeColor()
    {
        float t = (float)charge.Stacks / charge.MaxStacks;
        material.color = Color.Lerp(baseColor, chargedColor, t);
    }
}
