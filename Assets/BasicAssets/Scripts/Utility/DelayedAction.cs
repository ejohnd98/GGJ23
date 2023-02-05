using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedAction : MonoBehaviour
{
    [System.Serializable]
    public class Action{
        public float time = 0.0f;
        public UnityEvent Actions;
    }

    [SerializeField]
    private Action[] actions;
    public bool onStart = false;
    public bool onEnable = false;

    private float counter;
    private int actionIndex;
    private bool performingActions = false;

    // Start is called before the first frame update
    void Start()
    {
        if(onStart){
            StartActions();
        }
    }

    private void OnEnable() {
        if(onEnable){
            StartActions();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(performingActions){
            if(actions[actionIndex].time <= counter){
                actions[actionIndex].Actions.Invoke();
                actionIndex++;
            }
            counter += Time.deltaTime;

            if(actionIndex >= actions.Length){
                performingActions = false;
            }
        }
    }

    public void StartActions(){
        if(performingActions){
            return;
        }
        counter = 0.0f;
        performingActions = true;
        actionIndex = 0;
    }
}
