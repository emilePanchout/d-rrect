using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelector : MonoBehaviour
{
    public Button publishButton;
    public Button replayButton;

    public TransformPublisher publisher;
    public TransformReplayer replayer;
    public FakeMovement parentMovement;
    public FakeMovement childMovement;
    public TMP_Text text;


    public void Start()
    {
        ToPublishMode();
    }

    public void ToPublishMode()
    {
        publisher.isPublishing = true;
        replayer.enabled = false;

        publishButton.interactable = false;
        replayButton.interactable = true;

        
        parentMovement.enabled = true;
        childMovement.enabled = true;

        text.text = "Sending data to " + publisher.topic;
    }

    public void ToReplayMode()
    {
        publisher.isPublishing = false;
        replayer.enabled = true;

        publishButton.interactable = true;
        replayButton.interactable = false;

        parentMovement.enabled = false;
        childMovement.enabled = false;

        text.text = "Waiting for data from " + replayer.topic;
    }

}
