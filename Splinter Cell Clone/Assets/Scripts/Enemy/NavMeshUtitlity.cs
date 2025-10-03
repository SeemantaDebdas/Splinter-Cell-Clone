using UnityEngine;
using UnityEngine.AI;

public static class NavMeshUtility
{
    public static bool TryGetRandomPoint(Vector3 center, float radius, out Vector3 result)
    {
        for (int i = 0; i < 10; i++) // try multiple times in case NavMesh.SamplePosition fails
        {
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            Vector3 randomPoint = center + new Vector3(randomCircle.x, 0, randomCircle.y);

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = center; // fallback
        return false;
    }
}
