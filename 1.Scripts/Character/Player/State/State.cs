using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/State")]
public class State : ScriptableObject
{
    public GenericBehaviour[] behaviours;
    public Transition[] transitions;

    public void DoAction(BehaviourController controller)
    {
        for(int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].Action(controller);
        }
    }

    public void CheckTransitions(BehaviourController controller)
    {
        for(int i = 0; i < transitions.Length; i++)
        {
            bool decision = transitions[i].Checkbehaviour.Decide(controller);

            if(decision)
            {
                controller.EqualCurrent(transitions[i].trueBehaviour);
            }
            else
            {
                controller.EqualCurrent(transitions[i].falseBehaviour);
            }

            if (controller.currentBehaviour != this)
            {
                controller.currentBehaviour.ChangeState(controller);
                break;
            }
        }
    }

    public void ChangeState(BehaviourController controller)
    {

        for(int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].Setting(controller);
        }

        for(int i = 0; i < transitions.Length; i++)
        {
            transitions[i].Checkbehaviour.Setting(controller);
        }
    }


}
