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

                if(TestOverlap(bodyA, bodyB))
                {
                    contacts.Add(new Contact() { bodyA = bodyA, bodyB = bodyB });
                }

            }
        }
    }

    public static bool TestOverlap(Body bodyA, Body bodyB)
    {
        return Circle.Intersects(new Circle(bodyA), new Circle(bodyB)) ;
    }
}
