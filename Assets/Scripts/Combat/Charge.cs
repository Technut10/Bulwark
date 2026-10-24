using System;
using UnityEngine;

///<summary> 
/// Magnetic Charge stacks on an enemy. The gaunlet adds stacks, they fade over time, and the ultimate loos for enemies that have any.
/// </summary>
public class Charge : MonoBehaviour
{
	[SerializeField, Min(1)] private int maxStacks = 5;
	[Tooltip("Seconds after the last hit before stacks start fading.")]
	[SerializeField, Min(0f)] private float decayDelay = 2f;
	[Tooltip("Seconds between losing each stack once fading starts.")]
	[SerializeField, Min(0.5f)] private float decayInterval  = 0.5f;

	///	<summary> Multiplies decayDelay. The Lingering Charge perk will raise this later.</summary>
	
	public float DecayDelayMultiplier { get; set;} = 1f;

	public int Stacks {get; private set;}
	public int MaxStacks => maxStacks;
	public bool IsCharged => Stacks > 0;
	public bool IsFull => Stacks >= maxStacks;

	public event Action<int> StacksChanged;

	private float timeSinceLastHit;
	private float decayTimer;

	private void Update()
	{
		if (Stacks == 0) return;

		timeSinceLastHit += Time.deltaTime;
		if (timeSinceLastHit < decayDelay * DecayDelayMultiplier) return;

		decayTimer += Time.deltaTime;
		if (decayTimer >= decayInterval)
		{
			decayTimer = 0f;
			SetStacks(Stacks-1);
		}
	}

	public void AddStacks(int amount)
	{
		if ( amount <= 0) return;

		timeSinceLastHit = 0f;
		decayTimer = 0f;
		SetStacks(Stacks + amount);
	}

	///<summary>Fills to max instantly (used by the OverLoad Tether perk).</summary>
	public void Fill() => AddStacks(maxStacks);

	///<summary> Clears allstacks and returns how many there were (used by the ultimate).</summary> 
	public int Consume()
	{
		int consumed = Stacks;
		SetStacks(0);
		return consumed;
	}

	private void SetStacks(int value)
	{
		int clamped = Mathf.Clamp(value, 0, maxStacks);
		if (clamped == Stacks) return;

		Stacks = clamped;
		StacksChanged?.Invoke(Stacks);
	}
}

