using UnityEngine;

public enum InvestigationType
{
    LostPlayer,
    Noise,
    DeadBody,
    SuspiciousObject
}

public struct InvestigationRequest
{
    public Vector3 Position;
    public InvestigationType Type;

    public InvestigationRequest(Vector3 position, InvestigationType type)
    {
        Position = position;
        Type = type;
    }
}
