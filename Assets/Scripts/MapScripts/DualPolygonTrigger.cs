using UnityEngine;

namespace MapScripts
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class DualPolygonTrigger : MonoBehaviour
    {
        // Inside Collider Used For Mech Transform
        private PolygonCollider2D innerCollider;
        // Outside Collider Used For Ship Transform 
        private PolygonCollider2D outerCollider;

        // These multipliers will control how much to scale the polygon collider.
        [Range(0.8f, 0.95f)] public float innerScale = 0.9f;
        [Range(1.05f, 1.2f)] public float outerScale = 1.1f;

        void Awake()
        {
            // Get the original collider (inner)
            innerCollider = GetComponent<PolygonCollider2D>();
            innerCollider.isTrigger = true;

            // Create outer collider as a duplicate
            outerCollider = gameObject.AddComponent<PolygonCollider2D>();
            outerCollider.isTrigger = true;

            // Copy points
            outerCollider.pathCount = innerCollider.pathCount;

            for (int i = 0; i < innerCollider.pathCount; i++)
            {
                Vector2[] path = innerCollider.GetPath(i);
                Vector2[] innerScaled = ScalePath(path, innerScale);
                Vector2[] outerScaled = ScalePath(path, outerScale);

                innerCollider.SetPath(i, innerScaled);
                outerCollider.SetPath(i, outerScaled);
            }
            
        }

        // Scale a polygon’s vertices outward or inward relative to its center
        private Vector2[] ScalePath(Vector2[] original, float scale)
        {
            Vector2[] scaled = new Vector2[original.Length];
            Vector2 centroid = Vector2.zero;
            foreach (Vector2 v in original) centroid += v;
            centroid /= original.Length;

            for (int i = 0; i < original.Length; i++)
            {
                scaled[i] = centroid + (original[i] - centroid) * scale;
            }

            return scaled;
        }

        // Handle trigger events separately for each collider
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null) return;

            // Check which collider triggered
            if (innerCollider.IsTouching(other))
            {
                // Debug.Log($"{name}: Inner trigger entered by {other.name}");
                OnInnerEnter(other);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other == null) return;
        
            if (!outerCollider.IsTouching(other))
            {
                // Debug.Log($"{name}: Outer trigger exited by {other.name}");
                OnOuterExit(other);
            }
        }

        // Separate methods for custom handling logic
        private void OnInnerEnter(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerMovement player))
            {
                if (player.ship)
                    player.SwapForm();
            }
        }

        private void OnOuterExit(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerMovement player))
            {
                if (!player.ship)
                    player.SwapForm();
            }
        }
    }
}
