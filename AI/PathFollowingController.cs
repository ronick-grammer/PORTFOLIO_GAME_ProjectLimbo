using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.AI;



public class PathFollowingController : MonoBehaviour 
{

    public PathCreator pathCreator;
    private MovementController script_movementController;
    private Animator animator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed;
    public float rotationSpeed;
    public bool FollowYPath;
    public float delaySeconds_atLastPoint;
    public bool setNavDestinationOnStart;

    private float distanceTravelled;
    private Vector3 rotation_reverse;    

    private bool reverse;
    private bool delay;
    private bool followPath;

    int index;


   // [HideInInspector]
   // public bool retreived;
    [HideInInspector]
    public int parameterNum;

    [HideInInspector]
    public List<string> animParameter_facingRight;
    [HideInInspector]
    public int index_name_facingRight;

    [HideInInspector]
    public List<string> animParameter_movement;
    [HideInInspector]
    public int index_name_movement;

    [HideInInspector]
    public List<string> animParameterAtLastPoint;
    [HideInInspector]
    public int index_name_AtLastPoint;

    NavMeshAgent navMeshAgent;
    bool ToNavDestination;

    void Start()
    {
        animParameter_facingRight = Get_ParameterList();/
        animParameter_movement = Get_ParameterList(); 
        animParameterAtLastPoint = Get_TriggerParameterList();

        script_movementController = GetComponent<MovementController>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        // object should go to the first point with its correct rotation
        ReverseRotation_comparingThisPositionWithTargets();

        SetNavDestination(setNavDestinationOnStart);
    }

    void FixedUpdate()
    {
        animParameter_facingRight = Get_ParameterList();
        animParameter_movement = Get_ParameterList();
        animParameterAtLastPoint = Get_TriggerParameterList();

        animator.SetBool(animParameter_facingRight[index_name_facingRight], script_movementController.GetValue_facingRight());
        if (!delay && followPath)
        {
            distanceTravelled += speed * Time.fixedDeltaTime;
            
            // Object follows each point of the path 
            Vector3 newPoint;
            if(!FollowYPath)
                newPoint = new Vector3(pathCreator.path.GetPoint(index).x, transform.position.y, pathCreator.path.GetPoint(index).z);
            else
                newPoint = new Vector3(pathCreator.path.GetPoint(index).x, pathCreator.path.GetPoint(index).y, pathCreator.path.GetPoint(index).z);

            transform.position = Vector3.MoveTowards(transform.position, newPoint, speed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(pathCreator.path.GetRotationAtDistance(pathCreator.path.GetClosestDistanceAlongPath(transform.position), endOfPathInstruction).eulerAngles.x,
                 rotation_reverse.y + pathCreator.path.GetRotationAtDistance(pathCreator.path.GetClosestDistanceAlongPath(transform.position), endOfPathInstruction).eulerAngles.y,
                   pathCreator.path.GetRotationAtDistance(pathCreator.path.GetClosestDistanceAlongPath(transform.position), endOfPathInstruction).eulerAngles.z));

            animator.SetBool(animParameter_movement[index_name_movement], true);
            
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
                    StartCoroutine(Delay_AtLastPoint());
                }
                else if (index < 0 && endOfPathInstruction.Equals(EndOfPathInstruction.Reverse)) // at the first point
                {
                    rotation_reverse = new Vector3(0f, 0f, 0f); // original
                    index = 1;
                    reverse = false;
                    StartCoroutine(Delay_AtLastPoint());
                }
            }
        }

        if (ToNavDestination)
        {            
            

            //SetDestination() is not accurate for Y position(this code should cause the bug with flying objs).
            if (transform.position == new Vector3(navMeshAgent.destination.x, transform.position.y, navMeshAgent.destination.z))
            {

                Quaternion rotateTowards = Quaternion.Euler(new Vector3(pathCreator.path.GetRotationAtDistance(pathCreator.path.GetClosestDistanceAlongPath(transform.position), endOfPathInstruction).eulerAngles.x,
                    180f + pathCreator.path.GetRotationAtDistance(pathCreator.path.GetClosestDistanceAlongPath(transform.position), endOfPathInstruction).eulerAngles.y,
                   pathCreator.path.GetRotationAtDistance(pathCreator.path.GetClosestDistanceAlongPath(transform.position), endOfPathInstruction).eulerAngles.z));
               
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTowards, rotationSpeed * Time.fixedDeltaTime);
                
                if (transform.rotation == rotateTowards)
                {                   
                    script_movementController.Set_flip(false); // left at the first point of the path
                    
                    
                    StartCoroutine(Delay_AtLastPoint());

                    followPath = true;

                    rotation_reverse = new Vector3(0f, 0f, 0f); // original
                    index = 1;
                    reverse = false;

                    SetNavDestination(false);
                }
            }
        }
    }
   

    IEnumerator Delay_AtLastPoint() // back and forth
    {
        delay = true;
        animator.SetBool(animParameter_movement[index_name_movement], false);
        animator.SetTrigger(animParameterAtLastPoint[index_name_AtLastPoint]);

        yield return new WaitForSeconds(delaySeconds_atLastPoint);

        delay = false;
        script_movementController.Set_flip(!script_movementController.GetValue_facingRight()); // flip
    }

    public void SetValueOf_followPath(bool value)
    {
        followPath = value;
    }

    public bool GetValueOf_followPath()
    {
        return followPath;
    }

    public bool IsAtLastPoint()
    {
        return delay;
    }

    public void ReverseRotation_comparingThisPositionWithTargets()
    {
        if (transform.position.x > pathCreator.path.GetPoint(0).x)
        {
            ReverseRotation(true); // reverse
        }
        else
        {
            ReverseRotation(false); // original
        }
    }

    public void ReverseRotation(bool value)
    {
        if (value)
        {
            rotation_reverse = new Vector3(180f, 180f, 180f);
            reverse = true;

            script_movementController.Set_flip(false); // left
        }
        else
        {
            rotation_reverse = new Vector3(0f, 0f, 0f); // original
            reverse = false;

            script_movementController.Set_flip(true); // right
        }
    }

    public Vector3 GetValueOf_FirstPointOfPath()
    {
        return pathCreator.path.GetPoint(1);
    }

    public void SetNavDestination(bool value) 
    {
        
        if (value) // object(monster or whatever) goes for the first point of its path accoring to navigation system setting
        {
            navMeshAgent.enabled = true;
            
            navMeshAgent.SetDestination(new Vector3(pathCreator.path.GetPoint(0).x, transform.position.y, pathCreator.path.GetPoint(0).z));

            ToNavDestination = true;
            index = 0;
            
            animParameter_movement = Get_ParameterList(); 
            animator.SetBool(animParameter_movement[index_name_movement], true);

            ReverseRotation_comparingThisPositionWithTargets();
        }
        else
        {
            navMeshAgent.enabled = false;
            ToNavDestination = false;
            animator.SetBool(animParameter_movement[index_name_movement], false);
        }
    }

    public List<string> Get_ParameterList()
    {
        List<string> parameterList = new List<string>(GetComponent<Animator>().parameterCount);

        for (int i = 0; i < GetComponent<Animator>().parameterCount; i++) // It causes lagging;
        {
            if (GetComponent<Animator>().GetParameter(i).type.ToString().Equals("Bool"))
            {
                parameterList.Add(gameObject.GetComponent<Animator>().GetParameter(i).name);
                //Debug.Log(gameObject.GetComponent<Animator>().GetParameter(i).name);
            }
        }
        return parameterList;
    }

    public List<string> Get_TriggerParameterList()
    {
        List<string> parameterList = new List<string>(GetComponent<Animator>().parameterCount);

        for (int i = 0; i < GetComponent<Animator>().parameterCount; i++) // It causes lagging;
        {
            if (GetComponent<Animator>().GetParameter(i).type.ToString().Equals("Trigger"))
            {
                parameterList.Add(gameObject.GetComponent<Animator>().GetParameter(i).name);
            }
        }
        return parameterList;
    }

    public void Set_pathCreator(PathCreator value)
    {
        pathCreator = value;
    }
}
