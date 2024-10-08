using UnityEngine;
public class GlobalScript: MonoBehaviour
{
    public float magnitude = 1;
    public ForJoystick joystick;
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
    private bool[] ismovingBalloonsList;
    private bool[] onSquareBalloonsList;
    private int[] chargeSquareList;
    private Every[] squaresList;
    private Every[] balloonsList;
    private bool canBringBalloon;
    private bool found;

    //private bool moving = false;
    private float posSpawn = 0;

    private void Start()
    {
        squaresList = new Every[squaresNumbersList.Length];
        chargeSquareList = new int[squaresNumbersList.Length];
        for (int i = 0; i < squaresNumbersList.Length; i++)
        {
            var square = Instantiate(prefabSquare);
            posSpawn += square.setSettings(posSpawn, squaresNumbersList[i], transform.position, 1, magnitude);
            squaresList[i] = square;
            chargeSquareList[i] = 0;
        }
        posSpawn += 1*magnitude;

        balloonsList = new Every[balloonsNumbersList.Length];
        ismovingBalloonsList = new bool[balloonsNumbersList.Length];
        onSquareBalloonsList = new bool[balloonsNumbersList.Length];
        for (int i = 0; i < balloonsNumbersList.Length; i++)
        {
            var balloon = Instantiate(prefabBalloon);
            posSpawn += balloon.setSettings(posSpawn, balloonsNumbersList[i], transform.position, 0, magnitude) + 1;
            balloonsList[i] = balloon;
            ismovingBalloonsList[i] = false;
            onSquareBalloonsList[i] = false;
        }
    }
    void Update()
    {
        //Debug.Log(joystick.getOnClick());
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!joystick.getOnClick())
        {
            canBringBalloon = true;
            for (int i = 0; i < ismovingBalloonsList.Length; i++)
            {
                canBringBalloon &= !ismovingBalloonsList[i];
                //Debug.Log(movingBalloonsList[i]);
            }
            for (int i = 0; i < balloonsList.Length; i++)
            {
                if (canBringBalloon & !onSquareBalloonsList[i] & Vector2.Distance(mousePosition, balloonsList[i].getPos()) <= balloonsList[i].getScale() * 2)
                {
                    ismovingBalloonsList[i] = true;
                }
                if (!Input.GetMouseButton(0) || balloonsList[i].getUp())
                {
                    ismovingBalloonsList[i] = false;
                }

                for (int j = 0; j < squaresList.Length; j++)
                {
                    if (Vector2.Distance(balloonsList[i].getPos(), squaresList[j].getPos()) <= squaresList[j].getScale() / 2)
                    {
                        ismovingBalloonsList[i] = false;
                        found = false;
                        for (int f = 0; f < squaresList[j].getListBalloons().Length; f++)
                        {
                            if (squaresList[j].getListBalloons()[f] == i)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            squaresList[j].setBalloonInList(chargeSquareList[j], i);
                            onSquareBalloonsList[i] = true;
                            chargeSquareList[j] += 1;
                            balloonsList[i].setPos((Vector2)squaresList[j].getPos() + new Vector2(
                                                            -squaresList[j].getScale() / 2 + chargeSquareList[j] * (squaresList[j].getScale() / (squaresNumbersList[j] + 1)),
                                                            squaresList[j].getScale() / 2)
                                                  );
                            //
                            if (chargeSquareList[j] == squaresNumbersList[j])
                            {
                                squaresList[j].Up(speedGoUp);
                                for (int g = 0; g < squaresList[j].getListBalloons().Length; g++)
                                {
                                    balloonsList[(squaresList[j].getListBalloons())[g]].Up(speedGoUp);
                                }
                            }
                            //
                        }
                    }
                    //else {
                    //    chargeSquareList[j] -= 1;
                    //    squaresList[j].setBalloonInList(chargeSquareList[j], -1);
                    //}
                    //else if() //доделать
                    //{ chargeSquareList[j] -= 1; }

                }
                if (ismovingBalloonsList[i])
                {
                    // Offsets the target position so that the object keeps its distance.
                    mousePosition += ((Vector2)balloonsList[i].getPos() - mousePosition).normalized * minDistance;
                    balloonsList[i].setPos(Vector2.SmoothDamp(balloonsList[i].getPos(), mousePosition, ref currentVelocity, smoothTime, maxMoveSpeed));
                }
            }
        }
        else
        {
            
            mousePosition += ((Vector2)joystick.getPos() - mousePosition).normalized * minDistance;
            var vector = new Vector2(Vector2.SmoothDamp(joystick.getPos(), mousePosition, ref currentVelocity, smoothTime, maxMoveSpeed).x, 0);
            var movingVector = joystick.getPos();
            joystick.setPos(vector);
            movingVector = movingVector - joystick.getPos();
            for(int i=0;i< squaresList.Length; i++)
            {
                squaresList[i].setPos(squaresList[i].getPos() + movingVector);
            }
            for (int i = 0; i < balloonsList.Length; i++)
            {
                balloonsList[i].setPos(balloonsList[i].getPos() + movingVector);
            }
        }



    }
}