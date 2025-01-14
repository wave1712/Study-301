using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    private float threshold = 0.1f;
    private float deadZone = 0.025f;
    // Start is called before the first frame update

    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;

    public UnityEvent onPressed, onReleased;
    void Start()
    {
        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPressed && GetValue() + threshold >= 1)
            Pressed();
        if (!_isPressed && GetValue() + threshold <= 0)
            Released();
    }

    private float GetValue()
    {
        var value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;

        if (Mathf.Abs(value) < deadZone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);
    }
    private void Pressed()
    {
        _isPressed = true;
        onPressed.Invoke();
        Debug.Log(message: "Pressed");
    }

    private void Released()
    {
        _isPressed = false;
        onReleased.Invoke();
        Debug.Log(message: "Released");
    }
}
