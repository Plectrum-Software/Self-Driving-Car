using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarAgent : MonoBehaviour
{
    public GenerationController controller;

    public float survivalTime = 0f;

    public NeuralNet net;

    public float forwardSpeed = 3f;
    public float turnSpeed = 3f;
    public LayerMask layerMask;

    public List<Transform> directions;

    public float maxDetectDistance = 3f;
    public void DetectDirection()
    {


    }

    public double[] Evaluate()
    {
        List<double> inputs = new List<double>();
        foreach(Transform dirTransform in directions)
        {
            RaycastHit hit;
            //bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo
            if (Physics.Raycast(dirTransform.position, dirTransform.forward, out hit, maxDetectDistance, layerMask))
            {
                inputs.Add(hit.distance/maxDetectDistance);
            }
        }
        double[] output = net.Compute(inputs.ToArray());
        return output;
    }


    // Start is called before the first frame update
    void Start()
    {
        net = new NeuralNet(5, 5, 2);
        //DetectFood();
    }
    // Update is called once per frame
    void Update()
    {
        double[] output = Evaluate();
        float turn = (float)output[0];
        turn = turn * 2f - 1f;
        float acceleration = (float)output[1];
        acceleration = acceleration * 2f - 1f;

        transform.Rotate(Vector3.up * turn * turnSpeed);
  

        transform.position += transform.forward * acceleration * forwardSpeed * Time.deltaTime;
    }
}
