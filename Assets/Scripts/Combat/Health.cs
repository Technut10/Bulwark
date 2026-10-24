using System; 
using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField, Min(1f)] private float maxHealth = 200f;
	[Tooltip("For test dummies: refill to full on death instead of disabling.")]
	[SerializeField] private bool resetOnDeath;

	public float Max => maxHealth;
	public float Current {get; private set;}
	public bool IsDead => Current <= 0f;

	/// <summary>Amount of damage taken, and who dealt it.</summary>
	
	public event Action<float, GameObject> Damaged;
	public event Action<float> Healed;
	public event Action Died;

	private void Awake()
	{
		Current = maxHealth;

	}

	public void TakeDamage(float amount, GameObject source = null)
	{
		if (IsDead || amount <= 0f) return;

		Current = Mathf.Max(Current - amount, 0f);
		Damaged?.Invoke(amount, source);

		if (IsDead)
		{
			Died?.Invoke();

			if (resetOnDeath)
				Current = maxHealth;
			else
				gameObject.SetActive(false);
		}
	}

	public void Heal(float amount)
	{
		if (IsDead || amount <= 0f) return;

		float before = Current;
		Current = Mathf.Min(Current + amount, maxHealth);

		Healed?.Invoke(Current - before);
	}
}
