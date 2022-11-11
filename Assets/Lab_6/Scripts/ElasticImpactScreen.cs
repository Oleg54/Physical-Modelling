using System;
using UnityEngine;

public class ElasticImpactScreen : ParametersScreenBase
{
    private MovementBody[] _bodies;
    private ElasticImpactBehaviour _elasticImpactBehaviour;

    protected override PhysicBehaviourBase _physicBehaviourBase => _elasticImpactBehaviour;

    protected override void InitCallback()
    {
        _bodies = FindObjectsOfType<MovementBody>();
    }

    protected override void UpdatePhysicBehaviour()
    {
        float m1 = FindInputParameter(InputParameterType.Lab6_M1);
        float m2 = m1;
        float speed2 = CurrentTaskNumber is TaskNumber.Number_3 ? FindInputParameter(InputParameterType.Lab6_Speed_2) : 0f;
        bool impactIsForward = CurrentTaskNumber != TaskNumber.Number_2;

        switch (Convert.ToInt32(FindInputParameter(InputParameterType.Lab6_Relatice_Mass_Type)))
        {
            case 1:
                m2 = m1 * 10;
                break;

            case 2:
                m2 = m1 / 10;
                break;
        }

        _elasticImpactBehaviour = new ElasticImpactBehaviour(
            _bodies[0],
            _bodies[1],
            m1,
            m2,
            FindInputParameter(InputParameterType.Lab6_Speed_1),
            speed2,
            impactIsForward);
    }

    private void FixedUpdate()
    {
        if (_activityState != ActivityState.Started)
        {
            _bodies[0].Rigidbody.velocity = Vector3.zero;
            _bodies[1].Rigidbody.velocity = Vector3.zero;
        }

        UpdateView();
    }
}
