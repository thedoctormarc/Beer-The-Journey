using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;

public class VoiceRecognition : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;

    Dictionary<string, Action> actions = new Dictionary<string, Action>();

    // Start is called before the first frame update
    private CharacterLogic character_logic;
    
    public void Start()
    {
        character_logic = GetComponent<CharacterLogic>();

        actions.Add("adelante", Forward);
        actions.Add("stop", StopAction);
        actions.Add("left", Left);
        actions.Add("right", Right);
        actions.Add("patada", Kick);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void Kick()
    {
        character_logic.SetCharacterState(CHAR_STATES.KICK);
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {

        Debug.Log(speech.text);

        if(character_logic.GetState() != CHAR_STATES.DOING_ACTION)
            actions[speech.text].Invoke();
    }

    private void Forward()
    {
        character_logic.SetCharacterState(CHAR_STATES.WALKING_FORWARD);
        //transform.Translate(2, 0, 0);
    }

    private void StopAction()
    {
        character_logic.SetCharacterState(CHAR_STATES.IDLE);
    }


    private void Right()
    {
        character_logic.SetCharacterState(CHAR_STATES.RIGHT);
    }
    private void Left()
    {
        character_logic.SetCharacterState(CHAR_STATES.LEFT);
    }

}

