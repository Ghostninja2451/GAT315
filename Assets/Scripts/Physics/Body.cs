using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public enum eForceMode
    {
        Force,
        Acceleration,
        Velocity
    }

    public enum eBodyType
    {
        Static,
        Kinamatic,
        Dynamic
    }

    [Tooltip("The shape for the Body")]
    public Shape shape;
    [HideInInspector]
    public List<Spring> springs { get; set; } = new List<Spring>();

    public eBodyType bodyType { get; set; } = eBodyType.Dynamic;
    public Vector2 position { get => transform.position; set => transform.position = value; }
    public Vector2 velocity { get; set; } = Vector2.zero;
    public Vector2 acceleration { get; set; } = Vector2.zero;
    public float drag { get; set; } = 0;
    public float restitution { get; set; } = 1.0f;
    
    public float mass => shape.mass;
    public float inverseMass { get =>(mass == 0 || bodyType != eBodyType.Dynamic) ? 0 : 1 / mass; }
    

    public void ApplyForce(Vector2 force, eForceMode forceMode )
    {
        if (bodyType != eBodyType.Dynamic) return;

        switch (forceMode)
        {
            case eForceMode.Force:
                acceleration += force * inverseMass;
                break;
            case eForceMode.Acceleration:
                acceleration += force;
                break;
            case eForceMode.Velocity:
                velocity = force;
                break;
            default:
                break;
        }

    }

    public void Step(float dt)
    {
        //acceleration = Simulator.Instance.gravity + ( force * inverseMass);
    }
}
