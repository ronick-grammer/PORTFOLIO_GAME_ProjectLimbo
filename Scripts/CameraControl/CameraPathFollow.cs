using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class CameraPathFollow : MonoBehaviour
{
    public PathCreator pathCreator;
    private CameraController cameraController;
    
    public EndOfPathInstruction endOfPathInstruction;
    public float speed;
    public float rotationSpeed;
    public float delaySeconds_atLastPoint;
    public bool setNavDestinationOnStart;


    private float distanceTravelled;
    private Vector3 rotation_reverse;

    private bool reverse;
    private bool followPath;

    int index;

    void Update()
    {
       
        if (followPath)
        {
            distanceTravelled += speed * Time.fixedDeltaTime;


            //set the point of the path the camera goes for
            Vector3 newPoint = new Vector3(pathCreator.path.GetPoint(index).x, transform.position.y, pathCreator.path.GetPoint(index).z);
            transform.position = Vector3.MoveTowards(transform.position, newPoint, speed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(pathCreator.path.GetRotationAtDistance(pathCreator.path.GetClosestDistanceAlongPath(transform.position), endOfPathInstruction).eulerAngles.x,
                 rotation_reverse.y + pathCreator.path.GetRotationAtDistance(pathCreator.path.GetClosestDistanceAlongPath(transform.position), endOfPathInstruction).eulerAngles.y,
                   pathCreator.path.GetRotationAtDistance(pathCreator.path.GetClosestDistanceAlongPath(transform.position), endOfPathInstruction).eulerAngles.z));

            

            // if obj reaches the point, move to the next point 
            if (transform.position == newPoint)
            {
                if (!reverse)
                {
                    index++;
                }
                else
                {
                    index--;
                }

                if (index > pathCreator.path.NumPoints - 1 && endOfPathInstruction.Equals(EndOfPathInstruction.Reverse)) // at the last point
                {
                    rotation_reverse = new Vector3(180f, 180f, 180f); // reverse
                    index = pathCreator.path.NumPoints - 2;
                    reverse = true;

                    
                }
                else if (index < 0 && endOfPathInstruction.Equals(EndOfPathInstruction.Reverse)) // at the first point
                {
                    rotation_reverse = new Vector3(0f, 0f, 0f); // original
                    index = 1;
                    reverse = false;
                    
                }
            }
        }
    }
}
