using UnityEngine;

public class GhostProjectionHandler : MonoBehaviour
{
    [SerializeField] GameObject targetModel;
    [SerializeField] Material ghostMaterial;

    public static GhostProjectionHandler Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            InstantiateProjection();
    }

    public void InstantiateProjection(float timeToDestroy = 5f)
    {
        SkinnedMeshRenderer[] meshRenderers = targetModel.GetComponentsInChildren<SkinnedMeshRenderer>();

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            GameObject projectionObj = new();
            projectionObj.transform.SetPositionAndRotation(targetModel.transform.position, targetModel.transform.rotation);

            MeshRenderer newMeshRendrer = projectionObj.AddComponent<MeshRenderer>();
            MeshFilter newMeshFilter = projectionObj.AddComponent<MeshFilter>();

            Mesh bakedMesh = new();
            meshRenderers[i].BakeMesh(bakedMesh);

            newMeshFilter.mesh = bakedMesh;
            newMeshRendrer.material = ghostMaterial;
            newMeshRendrer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            Destroy(projectionObj, timeToDestroy);
        }
    }
}
