using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{

    public enum Behaviour
    {
        Align = 1, AlignOpposite, Arrive, Blended, CollisionAvoidance, Evade, Face, Leave, LookAndRun, ObstacleAvoidance, PathFollowing, Pursue, Separation, VelocityMatch, Wander,
    };

    public enum Jobs
    {
        Civilian=1,Patrolman,GroupMember
    };

    public enum Actions
    {
        Nothing=1, Interact, Move, Patrol, Wait, Exit,
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

    public static Vector3 ClipVector(Vector3 inputVector, float maxValue)
    {
        if(inputVector.magnitude > maxValue)
        {
            inputVector.Normalize();
            inputVector *= maxValue;
        }
        return inputVector;
    }

    public static List<T> FisherYatesSuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
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
            if (AvailableResources.TryGetValue(key, out int currentCount))
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

        public void UpdateResource(string key, int value)
        {
            if (UnavailableResources.Contains(key))
            {
                UnavailableResources.Remove(key);
                AddResource(key, value);
            }
            else
            {
                int currentCount;

                AvailableResources.TryGetValue(key, out currentCount);
                AvailableResources[key] = currentCount + value;
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

                    throw new System.Exception("Cannot decrease more than total resources");
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

        public void DecreaseResource(string key,int numUsed)
        {
            if (UnavailableResources.Contains(key))
            {
                throw new System.Exception("Tried to decrease a fully used resource");
            }
            else
            {
                int currentCount;
                AvailableResources.TryGetValue(key, out currentCount);

                if (currentCount - numUsed < 0)
                {
                    throw new System.Exception("Cannot decrease more that total resources");
                }
                else if (currentCount - numUsed == 0)
                {
                    UnavailableResources.Add(key);
                    AvailableResources.Remove(key);
                }
                else
                {
                    AvailableResources[key] = currentCount - numUsed;
                }

            }

        }


        public string GetRandomResource()
        {
            int randomElement = Random.Range(0, AvailableResources.Count - 1);


            if (AvailableResources.Count != 0)
            {
                string ResourceName = AvailableResources.Keys[randomElement];
                DecreaseResource(ResourceName);
                return ResourceName;
            }
            else
                return "";
        }

        public string GetRandomResource(int numOfSpots)
        {
            List<int> availableSpotsPositions = new List<int>();
            
            for(int i = 0; i < AvailableResources.Keys.Count; ++i)
            {
                int currentCount;
                AvailableResources.TryGetValue(AvailableResources.Keys[i], out currentCount);

                if(currentCount >= numOfSpots)
                {
                    availableSpotsPositions.Add(i);
                }

                
            }
            if(availableSpotsPositions.Count == 0)
            {
                return "";
            }

            int randomElement = Random.Range(0, availableSpotsPositions.Count - 1);

            string ResourceName = AvailableResources.Keys[availableSpotsPositions[randomElement]];
            DecreaseResource(ResourceName, numOfSpots);
            return ResourceName;

        }




    }



}
