using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SnakeController : MonoBehaviour
{
    [SerializeField] float RotationThrust = 150f;
    [SerializeField] float AccelerationSpeed = 5f;
    [SerializeField] GameObject BodyUnit;
    [SerializeField] int GapBetweenBodyParts = 150;
    [SerializeField] GameObject Coin;

    //[SerializeField] GameController GameController;


    [SerializeField] GameObject WallRight;
    [SerializeField] GameObject WallLeft;
    [SerializeField] GameObject WallUp;
    [SerializeField] GameObject WallDown;

    [SerializeField] int MaxHealth = 100;
    private int CurrentHealth;

    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> AllPositionsHistory = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        AllPositionsHistory.Insert(0, transform.position);

        int index = 0;
        foreach (var bodyPart in BodyParts)
        {
            Vector3 point = AllPositionsHistory[Mathf.Min(index * GapBetweenBodyParts, AllPositionsHistory.Count - 1)];
            Vector3 moveDirection = point - bodyPart.transform.position;
            bodyPart.transform.position += AccelerationSpeed * Time.deltaTime * moveDirection;
            bodyPart.transform.LookAt(point);
            index++;
        }
    }

    private void Move()
    {
        float rotationSpeed = RotationThrust * Time.deltaTime;

        // move forward
        transform.position += transform.forward * AccelerationSpeed * Time.deltaTime;

        // steer
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(rotationSpeed * Vector3.down);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(rotationSpeed * Vector3.up);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Coin"))
        {
            Destroy(collider.gameObject);
            AddRandomCoin();
            GrowSnake();
            print("Cooin");
        }

        if (collider.CompareTag("Die"))
        {
            //foreach (GameObject bodyPart in BodyParts)
            //{
            //    Destroy(bodyPart);
            //{

            //AllPositionsHistory.Clear();
            //print("`Died");
        }
    }

    public void GrowSnake()
    {
        GameObject newBodyPart = Instantiate(BodyUnit);
        BodyParts.Add(newBodyPart);
    }

    public void AddRandomCoin()
    {
        GameObject newCoin = Instantiate(Coin);
        newCoin.transform.position = new Vector3(Random.Range(WallLeft.transform.position.x, WallRight.transform.position.x), 1, Random.Range(WallUp.transform.position.z, WallDown.transform.position.z));
    }
}
