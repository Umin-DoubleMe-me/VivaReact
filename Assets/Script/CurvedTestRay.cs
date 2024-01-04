using Oculus.Interaction;
using Oculus.Interaction.Surfaces;
using UnityEngine;

public class CurvedTestRay : OculusPlatformRay
{
	private LayerMask uiMask;

	protected override void Awake()
	{
		uiMask = LayerMask.GetMask("UI");
		base.Awake();
	}

	protected override RayInteractable ComputeCandidate()
	{
		RayInteractable result = base.ComputeCandidate();
		result = CheckCurved();

		return result;
	}

	private RayInteractable CheckCurved()
	{
		RayInteractable result = null;
		float closestDist = float.MaxValue;
		var hits = Physics.RaycastAll(Ray, MaxRayLength, uiMask);
		RaycastHit closestUI = new RaycastHit();
		foreach(RaycastHit hitUI in hits)
		{
			if (hitUI.distance < closestDist)
			{
				closestDist = hitUI.distance;
				closestUI = hitUI;
			}
		}

		if(closestUI.transform != null)
		{
			End = Origin + closestUI.distance * Forward;
			CollisionInfo = new SurfaceHit()
			{
				Point = closestUI.point,
				Normal = closestUI.normal,
				Distance = closestUI.distance
			};
			result = closestUI.transform.GetComponent<RayInteractable>();
		}

		return result;
	}
}
