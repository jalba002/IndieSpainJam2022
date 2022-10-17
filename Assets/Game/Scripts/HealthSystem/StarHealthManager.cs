using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX.Utility;

namespace CosmosDefender
{
	public class StarHealthManager : EnemyHealthManager
	{
		[SerializeField] private Transform parentGameObject;
		[SerializeField] private Gradient vfxGradientOverride;
		public override void Die()
		{
			//base.Die();
			
			// Plays the disintegrate VFX and gives stars.
			if (GameManager.Instance.ResourceManager != null)
			{
				GameManager.Instance.ResourceManager.IncreaseResource(ResourceType.Stars, data.StarResourceOnDeath);
				GameManager.Instance.ResourceManager.IncreaseResource(ResourceType.Goddess, data.StarResourceOnDeath);
			}

			dieSound.Play();

			foreach (var meshRenderer in attachedMeshRenderers)
			{
				var vfxObject = Instantiate(prefab, meshRenderer.transform.position,
					meshRenderer.transform.rotation);
        
				Mesh m = new Mesh
				{
					name = "TemporalSkinnedMesh",
				};
        
				vfxObject.SetInt("ParticleAmount", particleAmountOverride);
				vfxObject.gameObject.GetComponent<VFXPropertyBinder>().AddPropertyBinder<VFXTransformBinder>().Init("VictimTransform", meshRenderer.transform);
				meshRenderer.BakeMesh(m);
				vfxObject.SetMesh("VictimMesh", m);
				vfxObject.SetGradient("Color", vfxGradientOverride); 

				Destroy(vfxObject.gameObject, 5f);
			}
			
			if (destroyOnDie)
				Destroy(parentGameObject.gameObject);
		}
		
	}
}