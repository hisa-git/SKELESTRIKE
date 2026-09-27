using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public float speed = 1f;
    public int currentState = 0;
    public List<TransformData> states = new List<TransformData>();

    private TransformData lastState;

    private float timeRotating = 0f;

    public bool DebugMoveEnable = false;

    void Start()
    {
        lastState = new TransformData(transform);
    }

    void Update()
    {
        if (DebugMoveEnable == true)
        {
            DebugMoveEnable = false;
            Move();
        }

        if (Vector3.Magnitude(states[currentState].position - lastState.position) != 0)
        {
            transform.position = Vector3.Lerp(
                lastState.position,
                states[currentState].position,

                Vector3.Magnitude(transform.position - lastState.position) /
                Vector3.Magnitude(states[currentState].position - lastState.position) +
                (speed * Time.deltaTime / Vector3.Magnitude(states[currentState].position - lastState.position))
            );

            timeRotating += Time.deltaTime;
            float t = timeRotating / (Vector3.Magnitude(states[currentState].position - lastState.position) / speed);
            transform.rotation = Quaternion.Slerp(
                Quaternion.Euler(lastState.rotation),
                Quaternion.Euler(states[currentState].rotation),
                Mathf.Clamp01(t));
        }
        else
        {
            timeRotating += Time.deltaTime;
            float t = timeRotating / (1 / speed);
            transform.rotation = Quaternion.Slerp(
                Quaternion.Euler(lastState.rotation),
                Quaternion.Euler(states[currentState].rotation),
                Mathf.Clamp01(t));
        }
    }

    public void Move()
    {
        currentState += 1;
        if (currentState >= states.Count()) currentState = 0;

        lastState = new TransformData(transform);

        timeRotating = 0f;
    }
}
