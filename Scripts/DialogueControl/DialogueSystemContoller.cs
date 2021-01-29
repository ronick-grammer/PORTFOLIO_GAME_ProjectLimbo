using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemContoller : MonoBehaviour
{
    private Queue<string> sentences;

    public Text dialogue_name;
    public Text dialogue_sentence;
    public Text dialogue_continue;

    private string[] splited_name_sentence;

    private bool isInDialogueMode;
    private bool endOfSentence;

    private InputController script_InputController;
    private bool canInputKeys;

    Container_TimeLineAsset script_Container_TimeLineAsset;
    DialogueOrderSetting script_DialogueOrderSetting;
    MovementController script_movementController;

    void Start()
    {
        sentences = new Queue<string>();
        splited_name_sentence = new string[2];
        isInDialogueMode = false;

        dialogue_name.text = "";
        dialogue_sentence.text = "";
        dialogue_continue.text = "";

        script_InputController = FindObjectOfType<InputController>();
    }

    private void FixedUpdate()
    { 
        // if current sentece of a dialogue is done showing up and press next key. 
        if (isInDialogueMode && Input.GetKeyDown(KeyCode.Return) && endOfSentence) 
        {
            endOfSentence = false;
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue, bool value, Container_TimeLineAsset script, DialogueOrderSetting dialougueOrderSetting = null, MovementController movementControllerScript = null)
    {
        isInDialogueMode = true;
        dialogue_continue.text = ">>";
        sentences.Clear();

        script_Container_TimeLineAsset = script;
        script_DialogueOrderSetting = dialougueOrderSetting;

        canInputKeys = value; // player can input keys or can't while dialogue is on
        script_InputController.ChangeValueOfCanInputKey(canInputKeys); 

        script_movementController = movementControllerScript;

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 1)
        {
            dialogue_continue.text = "";
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            
            if(script_DialogueOrderSetting != null) // set the Dialogue Ordering when conversation ends 
            {
                script_DialogueOrderSetting.SetOrdering();
            }

            if (script_Container_TimeLineAsset != null)  // Timeline After Dialogue
            {                Debug.Log("dialogueSystemController: " + script_movementController.name);
                script_Container_TimeLineAsset.StartTimeLineAndSetOrders(script_movementController);
            }
            return;
        }

        splited_name_sentence =  sentences.Dequeue().Split(':'); // split name and sentence by ':'; 

        dialogue_name.text = splited_name_sentence[0].Trim(); // and get rid of blanks of front and back

        StopAllCoroutines();
        StartCoroutine(TypeSentence(splited_name_sentence[1].Trim()));
    }
    
    IEnumerator TypeSentence (string sentence)
    {
        dialogue_sentence.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogue_sentence.text += letter;

            
            yield return null;
        }
        endOfSentence = true;
    }


    void EndDialogue()
    {
        isInDialogueMode = false;

        if (!canInputKeys)
        {
            script_InputController.ChangeValueOfCanInputKey(true);
        }

        dialogue_name.text = "";
        dialogue_sentence.text = "";
    }

    public bool GetValue_isInDialogueMode()
    {
        return isInDialogueMode;
    }
}
