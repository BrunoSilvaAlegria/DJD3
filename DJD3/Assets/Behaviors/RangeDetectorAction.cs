using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "Range Detector",
    story: "Update Range [Detector] and assign [Target]",
    category: "Action",
    id: "0705dddccb43747e64c6343336eec0ef")]
public partial class RangeDetectorAction : Action
{
    [SerializeReference] public BlackboardVariable<PlayerDetector> Detector;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnUpdate()
    {
        var detector = Detector?.Value;

        if (detector == null)
        {
            return Status.Failure;
        }

        var detectedTarget = detector.Target;

        if (detectedTarget != null)
        {
            Target.Value = detectedTarget;
            return Status.Success;
        }

        return Status.Failure;
    }
}
