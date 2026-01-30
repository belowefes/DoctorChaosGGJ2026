using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CameraFolower : MonoBehaviour
{
    [SerializeField] private bool freezeX;
    [SerializeField] private bool freezeY;
    [SerializeField] private Transform target;
    [SerializeField] private float followDeadZone;
    [SerializeField] private float stepLength;
    [SerializeField] private AnimationCurve easeCurve;

    [SerializeField] private bool progressiveMode = false;
    [SerializeField] private float duration = 1f;

    float progress = 0;    
    
    void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(freezeX ? this.transform.position.x : target.position.x,
            freezeY ? this.transform.position.y : target.position.y, this.transform.position.z);
        
        if (followDeadZone != 0 && Vector2.Distance(transform.position, targetPos) < followDeadZone)
        {
            progress = 0;
            return;
        }

        Follow(targetPos);
    }

    void Follow(Vector3 pos)
    {
        if (progressiveMode)
        {
            progress += stepLength / duration;
        }
        else
        {
            progress = stepLength;
        }
        this.transform.position = Vector3.Lerp(this.transform.position, pos, easeCurve.Evaluate(progress));
    }
}
