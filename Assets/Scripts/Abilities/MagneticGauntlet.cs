using UnityEngine;

public class MagneticGaunlet : Ability<MagneticGauntletData>
{
	[Header("References")]
	[Tooltip("Usually the Main Camera, so shots go where you're looking.")]
	///<summary></summary>
	[SerializeField] private Transform aimOrigin;
	
	[Header("Debug")]
	[SerializeField] private bool drawDebugLines = true;

	protected override void Activate()
	{
		Ray ray = new Ray(aimOrigin.position, aimOrigin.forward);

		bool hitSomething = Physics.SphereCast(
				ray, data.radius, out RaycastHit hit, data.range, data.hitMask, QueryTriggerInteraction.Ignore);

		if (drawDebugLines)
		{
			Vector3 end = hitSomething ? hit.point : ray.GetPoint(data.range);
			Debug.DrawLine(ray.origin, end, hitSomething ? Color.cyan : Color.gray, 0.25f);
		}

		if (!hitSomething) return;

		// GetComponentInParent so hits on child colliders still count.

		Health health = hit.collider.GetComponentInParent<Health>(); 

		if (health != null)
			health.TakeDamage(data.damage, gameObject);

		Charge charge = hit.collider.GetComponentInParent<Charge>();

		if (charge != null)
			charge.AddStacks(data.chargePerHit);

	}

}

