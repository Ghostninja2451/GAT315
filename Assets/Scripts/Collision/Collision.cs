using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision 
{

    public static void CreateContact(List<Body> bodies, out List<Contact> contacts)
    {
        contacts = new List<Contact>();
        for (int i = 0; i < bodies.Count - 1; i++)
        {
            for (int j = i + 1; j < bodies.Count; j++)
            {
                Body bodyA = bodies[i];
                Body bodyB = bodies[j];

                if(bodyA.bodyType == Body.eBodyType.Dynamic || bodyB.bodyType == Body.eBodyType.Dynamic)
                {
                    if (TestOverlap(bodyA, bodyB))
                    {
                        contacts.Add(GenerateContact(bodyA, bodyB));
                    }
                }

            }
        }
    }

    public static bool TestOverlap(Body bodyA, Body bodyB)
    {
        return Circle.Intersects(new Circle(bodyA), new Circle(bodyB)) ;
    }

    public static Contact GenerateContact(Body bodyA, Body bodyB)
    {
        Contact contact = new Contact();

        contact.bodyA = bodyA;
        contact.bodyB = bodyB;

        Vector2 direction = bodyA.position - bodyB.position;
        float distance = direction.magnitude;
        float radius = ((CircleShape)bodyA.shape).radius + ((CircleShape)bodyB.shape).radius;
        contact.depth = radius - distance;

        contact.normal = direction.normalized;


        Vector2 position = bodyB.position + (((CircleShape)bodyB.shape).radius * contact.normal);
        Debug.DrawRay(position, contact.normal);
        
        return contact;
    }

    public static void SeparateContacts(List<Contact> contacts)
    {
        foreach(var contact in contacts)
        {
            float totalInverseMass = contact.bodyA.inverseMass + contact.bodyB.inverseMass;

            Vector2 separation = contact.normal * (contact.depth / totalInverseMass);
            contact.bodyA.position += separation * contact.bodyA.inverseMass;
            contact.bodyB.position -= separation * contact.bodyB.inverseMass;
        }

    }

    public static void ApplyImpulses(List<Contact> contacts)
    {
        foreach (var contact in contacts)
        {
            Vector2 relativeVelocity = contact.bodyA.velocity - contact.bodyB.velocity;
            float normalVelcoity = Vector2.Dot(relativeVelocity, contact.normal);

            if (normalVelcoity > 0) continue;

            float totalInverseMass = contact.bodyA.inverseMass + contact.bodyB.inverseMass;

            float restitution = (contact.bodyA.restitution + contact.bodyB.restitution) * 0.5f;

            float impulseMagnitude = -((1 + restitution) * normalVelcoity) / totalInverseMass;

            Vector2 impulse = contact.normal * impulseMagnitude;

            contact.bodyA.ApplyForce(contact.bodyA.velocity + (impulse * contact.bodyA.inverseMass), Body.eForceMode.Velocity);
            contact.bodyB.ApplyForce(contact.bodyB.velocity - (impulse * contact.bodyB.inverseMass), Body.eForceMode.Velocity);
        }

    }
}
