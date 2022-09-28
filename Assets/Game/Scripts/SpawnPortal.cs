using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace CosmosDefender
{
	public class SpawnPortal : MonoBehaviour
	{
		[SerializeField] private VisualEffect spawnPortalFX;
		[SerializeField] private EnemySpawner enemySpawner;

		[SerializeField] private MeshRenderer quad;
		[SerializeField] private MeshRenderer sphere;


		private Coroutine spawnCoroutine;
		public void Awake()
		{
			enemySpawner.OnWaveStart += OpenPortal;
			enemySpawner.OnWaveEnd += ClosePortal;
		}

		public void OpenPortal()
		{
			spawnPortalFX.Play();
			float a = spawnPortalFX.GetFloat("ExplosionDelay");
			if (spawnCoroutine != null)
			{
				StopCoroutine(spawnCoroutine);
			}
			spawnCoroutine = CronoScheduler.Instance.ScheduleForTime(a, () =>
			{
				quad.enabled = true;
				sphere.enabled = true;
			});
		}

		public void ClosePortal(float waitTime)
		{
			spawnPortalFX.Stop();
			quad.enabled = false;
			sphere.enabled = false;
		}

		private void OnDisable()
		{
			enemySpawner.OnWaveStart -= OpenPortal;
			enemySpawner.OnWaveEnd -= ClosePortal;
		}
	}
}