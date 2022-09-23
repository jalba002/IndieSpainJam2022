using UnityEngine;
using UnityEngine.VFX;

namespace CosmosDefender
{
    public class SkillPreview : MonoBehaviour
    {
        [SerializeField] private VisualEffect vfx;

        // 
        public bool IsActive = false;

        public void Activate()
        {
            IsActive = true;
            gameObject.SetActive(true);
            vfx.Play();
        }

        public void Deactivate()
        {
            IsActive = false;
            gameObject.SetActive(false);
            vfx.Stop();
        }

        public void Move(Vector3 position)
        {
            transform.position = position;
        }

        public void UpdateVisuals(float size)
        {
            vfx.SetFloat("Size", size);
        }
    }
}