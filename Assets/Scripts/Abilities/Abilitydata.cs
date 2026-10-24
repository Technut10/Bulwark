using UnityEngine;

///<summary>
/// Base class for every ability's tunable numbers.
/// Each ability get its own subclass, and you create assests from it.
/// (right-click > Create > Bulwark > Abilities) so you can tune values in the Inspector without touching code.
///</summary>

public abstract class AbilityData : ScriptableObject
{
	[Header("Info")]
	public string displayName = "New Ability";
	[TextArea] public string description; 
	public Sprite icon;

	[Header("Timing")]
	[Tooltip("Seconds before the ability can be used again. For primary fire, this is the time between shots.")]
	[Min(0f)] public float cooldown = 1f;
}
