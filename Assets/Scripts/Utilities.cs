using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{

    public enum Behaviour
    {
        Align = 1, AlignOpposite, Arrive, Blended, CollisionAvoidance, Evade, Face, Leave, LookAndRun, ObstacleAvoidance, PathFollowing, Pursue, Separation, VelocityMatch, Wander,
    };



    public static float MapToInterval(float value, float left, float right, float toLeft, float toRight)
    {
        return (value - left) * (toRight - toLeft) / (right - left) + toLeft;
    }
        

    //Get a random small binomial
    public static float randomBinomial()
    {
        float a = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        return a - b;
    }


    //Angle in degrees
    public static float getAngleDifference(float fromAngle, float toAngle)
    {
        float angleDiff = (fromAngle - toAngle + 180) % 360 - 180;
        return angleDiff < -180 ? angleDiff + 360 : angleDiff;
    }


    //Angle in degrees
    public static Vector3 getDirectionalVector(float angle)
    {
        Vector3 directionalVector;

        angle = angle * Mathf.Deg2Rad;

        directionalVector = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

        return directionalVector;

    }


    public class RandomizedResourceArray
    {
        private SortedList<string, int> AvailableResources;
        private HashSet<string> UnavailableResources;
        
        public RandomizedResourceArray()
        {
            AvailableResources = new SortedList<string, int>();
            UnavailableResources = new HashSet<string>();
        }

        public void AddResource(string key, int value)
        {
            if(AvailableResources.TryGetValue(key,out int currentCount))
            {
                throw new System.Exception("Resource already exists");
            }

            AvailableResources.Add(key, value);
        }

        public void UpdateResource(string key)
        {
            if (UnavailableResources.Contains(key))
            {
                UnavailableResources.Remove(key);
                AddResource(key, 1);
            }
            else
            {
                int currentCount;

                AvailableResources.TryGetValue(key, out currentCount);
                AvailableResources[key] = currentCount + 1;
            }

        }

        public void DecreaseResource(string key)
        {
            if (UnavailableResources.Contains(key))
            {
                throw new System.Exception("Tried to decrease a fully used resource");
            }
            else
            {
                int currentCount;
                AvailableResources.TryGetValue(key, out currentCount);

                if (currentCount - 1 < 0)
                {
                    throw new System.Exception("Cannot decrease more that total resources");
                }
                else if (currentCount - 1 == 0)
                {
                    UnavailableResources.Add(key);
                    AvailableResources.Remove(key);
                }
                else
                {
                    AvailableResources[key] = currentCount - 1;
                }

            }

        }


        public string GetRandomResource()
        {
            int randomElement = Random.Range(0, AvailableResources.Count - 1);

            string ResourceName = AvailableResources.Keys[randomElement];

            DecreaseResource(ResourceName);

            return ResourceName;
        }



    }



}
