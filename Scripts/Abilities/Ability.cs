using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///  Non-generic base so other systems (HUD, perks, ultimate meter) can work with
///  any ability without knowing its specifc data type.
/// </summary>
public abstract class Ability : MonoBehaviour
{
	[Header("Input")]
	[SerializeField] private InputActionReference inputAction;
	[Tooltip("On: fires repeatedly while held (primary fire). Off: fires once per press.")]
	[SerializeField] private bool holdToActivate;

	public abstract AbilityData BaseData {get;}
	
	public float CooldownRemaining { get; private set;} // cannot be set outside of class or in the editor.

	public bool IsReady => CooldownRemaining <= 0f;

	public float CooldownPercent => 
		BaseData.cooldown > 0f ? Mathf.Clamp01((CooldownRemaining / BaseData.cooldown)) : 0f;

	/// <summary>
	///  Raised every time the ability successfully fires.
	/// </summary>
	public event Action<Ability> Activated;

	protected virtual void OnEnable()
	{
		if (inputAction != null)
			inputAction.action.Enable();
	}
	
	protected virtual void OnDisable()
	{
		if (inputAction != null) 
			inputAction.action.Disable();
	}

	protected virtual void Update()
	{
		if (CooldownRemaining > 0f)
			CooldownRemaining -= Time.deltaTime;

		if (inputAction == null) return;

		bool wantsToActivate = holdToActivate ? inputAction.action.IsPressed() : inputAction.action.WasPressedThisFrame();

		if (wantsToActivate)
			TryActivate();
	}

	/// <summary>Fires the ability if it's off cooldown and allowed. Returns true if it fired.</summary>
	public bool TryActivate()
	{
		if (!IsReady || !CanActivate()) 
			return false;

		Activate();
		CooldownRemaining = BaseData.cooldown;
		Activated?.Invoke(this);
		
		return true;
		
	}


	protected virtual bool CanActivate() => true; 

	protected abstract void Activate();

}

///<summary>
/// Typed versions: subclasses get their own data class with no casting. 
/// Example: MagneticGauntlet : Ability&lt;MagneticGaunletData&gt;
///</summary>

public abstract class Ability<TData> : Ability where TData : AbilityData
{
	
	[Header("Data")]
	[SerializeField] protected TData data;

	public override AbilityData BaseData => data;
	public TData Data => data;
}
