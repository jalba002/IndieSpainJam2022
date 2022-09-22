using UnityEngine;
using UnityEngine.VFX;

public class MeteorProjectile : BaseProjectile
{
    [SerializeField]
    private VisualEffect vfx;

    [SerializeField] private ParticleSystem particles;

    [SerializeField] private MeshRenderer mrend;

    public override void InitializeProjectile(Vector3 spawnPoint, Vector3 velocity)
    {
        base.InitializeProjectile(spawnPoint, velocity);
        // Enable collider?
        m_Rigidbody.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.gameObject.name}");
        vfx.SendEvent("Play");
        vfx.transform.parent = null;
        Ray r = new Ray(transform.position, Vector3.down);
        //Physics.Raycast(r, out RaycastHit hit, 5f, Int32.MaxValue);
        //vfx.transform.position = hit.point;
        vfx.transform.position = other.ClosestPoint(transform.position);
        
        mrend.enabled = false;
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.useGravity = false;
        particles.Stop();
        particles.gameObject.transform.parent = null;
        //this.gameObject.SetActive(false);
        Destroy(vfx.gameObject, 10f);
        Destroy(particles.gameObject, 3f);
        Destroy(this.gameObject);
    }
}
