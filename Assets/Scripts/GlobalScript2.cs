using UnityEngine;
public class GlobalScript2: MonoBehaviour
{
    public Every prefabSquare;
    public Every prefabBalloon;
    public int[] squaresNumbersList;
    public int[] balloonsNumbersList;
    //public Transform square;
    //public Transform square2;

    private float maxMoveSpeed = 10;
    private float smoothTime = 0.2f;
    private float minDistance = 0.5f;
    private float speedGoUp = 0.04f;
    private Vector2 currentVelocity;

    //private bool[] goUpSquareList;
    private bool[] movingBalloonsList;
    private int[] chargeSquareList;
    private Every[] squaresList;
    private Every[] balloonsList;
    private bool canBringBalloon;

    //private bool moving = false;
    private float posSpawn = 0;
    private void Start()
    {
        squaresList = new Every[squaresNumbersList.Length];
        //goUpSquareList = new bool[squaresNumbersList.Length];
        for (int i = 0; i < squaresNumbersList.Length; i++)
        {
            var square = Instantiate(prefabSquare);
            posSpawn += square.setSettings(posSpawn, squaresNumbersList[i], transform.position, 1, 1);
            squaresList[i] = square;
            //goUpSquareList[i] = false;
        }
        posSpawn += 1;

        balloonsList = new Every[balloonsNumbersList.Length];
        movingBalloonsList = new bool[balloonsNumbersList.Length];
        for (int i = 0; i < balloonsNumbersList.Length; i++)
        {
            var balloon = Instantiate(prefabBalloon);
            posSpawn += balloon.setSettings(posSpawn, balloonsNumbersList[i], transform.position, 0, 1) + 1;
            balloonsList[i] = balloon;
            movingBalloonsList[i] = false;
        }
    }
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        canBringBalloon = true;
        for (int i = 0; i < movingBalloonsList.Length; i++)
        {
            canBringBalloon &= !movingBalloonsList[i];
            Debug.Log(movingBalloonsList[i]);
        }
        for (int i = 0; i < balloonsList.Length; i++)
        {
            if (canBringBalloon & Vector2.Distance(mousePosition, balloonsList[i].getPos()) <= balloonsList[i].getScale() * 2)
            {
                movingBalloonsList[i] = true;
            }
            if (!Input.GetMouseButton(0) || balloonsList[i].getUp())
            {
                movingBalloonsList[i] = false;
            }


            if (movingBalloonsList[i])
            {
                // Offsets the target position so that the object keeps its distance.
                mousePosition += ((Vector2)balloonsList[i].getPos() - mousePosition).normalized * minDistance;
                balloonsList[i].setPos(Vector2.SmoothDamp(balloonsList[i].getPos(), mousePosition, ref currentVelocity, smoothTime, maxMoveSpeed));
            }
            for (int j = 0; j < squaresList.Length; j++)
            {
                if (Vector2.Distance(balloonsList[i].getPos(), squaresList[j].getPos()) <= squaresList[j].getScale() / 2)
                {
                    squaresList[j].Up(speedGoUp);
                    balloonsList[i].setPos((Vector2)squaresList[j].getPos() + new Vector2(0, squaresList[j].getScale() / 2));
                    balloonsList[i].Up(speedGoUp);
                }
            }
        }
    }
}