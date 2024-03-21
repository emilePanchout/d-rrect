//This work is funded by the French program investment of the future under the project JENII - ANR-21-DMES-0006.

using RosMessageTypes.DRrect;
using RosMessageTypes.Std;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEvent : EventPublisher
{
    public List<Input> InputList;

    protected override void InitializePublisher()
    {
        rosConnection.RegisterPublisher<EventMsg>(topic);

        foreach (Input input in InputList)
        {
            if (input.type == Input.inputType.Binary)
            {
                // On press, publish directly
                input.inputRef.action.performed += ctx => Publish(input); ;
                input.inputRef.action.Enable();
            }

            if (input.type == Input.inputType.Analogue)
            {
                // On press, start coroutine to publish every x seconds
                input.inputRef.action.started += ctx => TriggerAnalogue(input);
                input.inputRef.action.Enable();
            }
        }
    }

    protected void Update()
    {
        // Update dynamically the value of the input when pressed
        foreach (Input input in InputList)
        {
            if (input.inputRef.action.IsPressed())
            {
                input.value = input.inputRef.action.ReadValueAsObject().ToString();
            }
        }
    }

    public void TriggerAnalogue(Input input)
    {
        // avoid multiple coroutine if pressed multiple times
        if (input.isWaiting == false)
        {
            StartCoroutine(DelayAnaloguePublish(input));
        }
    }

    protected IEnumerator DelayAnaloguePublish(Input input)
    {
        input.isWaiting = true;
        Publish(input);

        float timer = 0f;

        while (timer < input.publishDelay)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        input.isWaiting = false;

        if(input.inputRef.action.IsPressed())
        {
            TriggerAnalogue(input);
        }
        
    }


    // Definition with no use
    protected override EventMsg CreateMessage(HeaderMsg header)
    {

        EventMsg message = new EventMsg();
        return message;
    }

    protected override EventMsg CreateMessage(HeaderMsg header, Input input)
    {
        EventMsg message = new EventMsg();
        message.header = header;
        message.data = input.name + ";" + input.value;

        return message;
    }


}


