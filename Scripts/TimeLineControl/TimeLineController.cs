using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

//[TrackBindingType(typeof(Animator))]

public class TimeLineController : MonoBehaviour
{   
    private PlayableDirector playableDirector_timeline;
    private TimelineAsset asset_timeline;
    private AnimationTrack animTrack_timeline;

    private Animator animator_from_timeline; // get each of all animators from a timeline
    private Rigidbody rigidbody_from_timeline;

    private DialogueTrigger script_DialogueTrigger;
    private Container_TimeLineAsset nextTimeline;
    private InputController script_InputController;
    
    private bool timeline_played;
    private bool hasDialogues;

    private bool unfreezeAll; // "true" as "Unfreeze All Transform", "false" as "Freeze Original Transform"
    private bool isKinematic;

    private DialogueSystemContoller dialogueSystemController;
    private MovementController script_movementController;

    private void Awake()
    {
        script_InputController = FindObjectOfType<InputController>();
        dialogueSystemController = FindObjectOfType<DialogueSystemContoller>();
    }

    void Start()
    {
        playableDirector_timeline = GetComponent<PlayableDirector>();

        timeline_played = false;        
    }

    void FixedUpdate()
    {
        if(playableDirector_timeline.state.ToString().Equals("Paused") && timeline_played) // if TimeLine's finished
        {
            timeline_played = false;

            unfreezeAll = false;
            isKinematic = false;
            ChangeAnimatorSettings();

            // if script_dialogueTrigge is equal to null. because it has InputController too.
            if (!hasDialogues && !dialogueSystemController.GetValue_isInDialogueMode()) 
            {
                script_InputController.ChangeValueOfCanInputKey(true); // can use input keys
                hasDialogues = false;
            }
        }

    }

    // nextTimeline event or Dialogue event dont need to be assigned, if not needed
    public void StartTimeLine(PlayableAsset value_playableAsset, bool canInput, DialogueTrigger dialogueTrigger = null, 
         Container_TimeLineAsset value_nextTimeline = null, float secs_EventStartsAt = 0, MovementController movementControllerScript = null)
    {
        script_InputController.ChangeValueOfCanInputKey(canInput);
        script_movementController = movementControllerScript;

        asset_timeline = (TimelineAsset) value_playableAsset;

        
        unfreezeAll = true;
        isKinematic = true;
        ChangeAnimatorSettings();
        playableDirector_timeline.playableAsset = value_playableAsset;
        playableDirector_timeline.Play();
       
        if (dialogueTrigger != null) // if has dialgoue with this timeline
        {
            script_DialogueTrigger = dialogueTrigger;
            hasDialogues = true;
            StartCoroutine(TriggerDialogueInTimeline(secs_EventStartsAt)); // start a dialogue in timeline in certain time
        }
        else
        {
            hasDialogues = false;
        }

        if(value_nextTimeline != null) // if there is a timeline after the onplaying timeline, 
        {
            nextTimeline = value_nextTimeline; 
            StartCoroutine(TriggerNextTimeline(secs_EventStartsAt)); // play the next timeline after the first timeline.
        }
        
        timeline_played = true;       
    }

    private void ChangeAnimatorSettings() // each animator setting for a timeline;
    {
        for (int i = 0; i < asset_timeline.outputTrackCount; i++)
        {

            //this MarkerTrack added on the TimeLine Binding List unwillingly so i had to deal with it
            //the animator setting of every object of the onplaying timeline is changed
            if (!asset_timeline.GetOutputTrack(i).GetType().Equals(typeof(MarkerTrack)) && !asset_timeline.GetOutputTrack(i).GetType().Equals(typeof(SignalTrack))) 
            {
                if (playableDirector_timeline.GetGenericBinding(asset_timeline.GetOutputTrack(i)).GetType().Equals(typeof(Animator)))
                {
                    animator_from_timeline = (Animator)playableDirector_timeline.GetGenericBinding(asset_timeline.GetOutputTrack(i));

                    rigidbody_from_timeline = animator_from_timeline.GetComponent<Rigidbody>();
                    if(rigidbody_from_timeline != null)
                    {
                        rigidbody_from_timeline.isKinematic = isKinematic;
                        SetValue_FreezeTransform(rigidbody_from_timeline, unfreezeAll);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Unfreeze All Trasform or  Freeze Original Transform   
    /// </summary>
    /// <param name="rigidbody">rigidbody of an object</param>
    /// <param name="value">"true" as "Unfreeze All Transform", "false" as "Freeze Original Transform"</param>
    private void SetValue_FreezeTransform(Rigidbody rigidbody, bool value)
    {
        if (value)
        {
            rigidbody.constraints = RigidbodyConstraints.None;
        }
        else
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }
    }

    IEnumerator TriggerDialogueInTimeline(float sces)
    {
        yield return new WaitForSeconds(sces);
        script_DialogueTrigger.TriggerDialogue(script_movementController);
    }

    IEnumerator TriggerNextTimeline(float secs)
    {
        yield return new WaitForSeconds(secs);
        unfreezeAll = false;
        isKinematic = false;
        ChangeAnimatorSettings();
        nextTimeline.StartTimeLine();
    }


}