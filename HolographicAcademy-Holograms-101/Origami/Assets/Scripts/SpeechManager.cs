using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    PhotoCaptureTest pct;
    // Use this for initialization
    void Start()
    {
        pct = gameObject.GetComponent<PhotoCaptureTest>();
        keywords.Add("Reset world", () =>
        {
            // Call the OnReset method on every descendant object.
            // this.BroadcastMessage("OnReset");
            // Application.Quit();
            //Name.Bacon();
            pct.TakePhoto();
        });

        //keywords.Add("Drop Sphere", () =>
       // {
        //    var focusObject = GazeGestureManager.Instance.FocusedObject;
        //        // Call the OnDrop method on just the focused object.
        //        focusObject.SendMessage("OnDrop");
        //});

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}