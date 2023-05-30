using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAIHandler : MonoBehaviour
{

    public enum AIMode { followPlayer, followWaypoints };

    [Header("AI settings")]
    public AIMode aiMode;

    //Local variables
    Vector3 targetPosition = Vector3.zero;
    Transform targetTransform = null;

    TopDownCarController topDownCarController;

    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame and is frame dependent
    void FixedUpdate()
    {
        Vector2 inputVector = Vector2.zero;

        inputVector.x = 1.0f;
        inputVector.y = 1.0f;

        topDownCarController.SetInputVector(inputVector);
        
    }
}
