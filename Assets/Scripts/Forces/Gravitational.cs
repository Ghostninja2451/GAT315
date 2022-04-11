using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravitational : Force
{
    [SerializeField] FloatData gravitation;
    public override void ApplyForce(List<Body> bodies)
    {
        for(int i = 0; i < bodies.Count - 1; i++) 
        {
            for (int j = i + 1; j < bodies.Count; j++)
            {
                Body bodyA = bodies[i];
                Body bodyB = bodies[j];
                // apply gravitational force 
                Vector2 direction = bodyA.position - bodyB.position;
                float distanceSqr = Mathf.Max(1 ,direction.sqrMagnitude);

                float gravValue = gravitation.value * (bodyA.mass * bodyB.mass) / distanceSqr;

                bodyA.ApplyForce(-direction.normalized * gravValue, Body.eForceMode.Force);
                bodyB.ApplyForce(direction.normalized * gravValue, Body.eForceMode.Force);
            }
        }
    }
}
