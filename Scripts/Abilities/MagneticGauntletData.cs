using UnityEngine;

[CreateAssetMenu(fileName = "MagneticGauntlet", menuName = "Bulwark/Abilities/Magnetic Gauntlet")]
public class MagneticGauntletData : AbilityData
{
	[Header("Burst")]
	[Min(0f)] public float damage = 25f;
	[Tooltip("Short range: this is a close-quarters tank weapon.")]
	[Min(0.5f)] public float range = 8f;
	[Min(0.5f)] public float radius = 0.4f;

	[Header("Charge")]
	[Min(0)] public int chargePerHit = 1;

	[Header("Targeting")]
	[Tooltip("Exclude the Player layer so you can't hit yourself.")]
	
	public LayerMask hitMask = ~0;

	private void Reset()
	{
		displayName = "Magnetic Gauntlet";
		description = "Short-range magnetic bursts. Repeated hits build Charge on enemies.";
		cooldown = 0.3f; // time between shots
	}
}

