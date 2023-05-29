using UnityEngine;
using System.Collections;

public enum ControlMode
{
    KeyBoard,
    Touch
}

public class CarController : MonoBehaviour
{
    public ControlMode CarControlMode;

    public float MaxSpeed = 7.0f;
    public float MaxSteer = 2.0f;
    public float Breaks = 0.2f;

    [SerializeField]
    private float Acceleration = 0.0f;
    private float Steer = 0.0f;
    private bool AccelFwd, AccelBwd;
    private bool TouchAccel, TouchBack, TouchBreaks;
    private bool SteerLeft, SteerRight;

    private bool IsOnGravel = false; // variable for gravel control


    private void FixedUpdate()
    {
        if (CarControlMode == ControlMode.KeyBoard)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                Accel(1); // Accelerate in forward direction
            else if (Input.GetKey(KeyCode.DownArrow))
                Accel(-1); // Accelerate in backward direction
            else if (Input.GetKey(KeyCode.Space))
            {
                if (AccelFwd)
                    StopAccel(1, Breaks); // Apply brakes while in forward direction
                else if (AccelBwd)
                    StopAccel(-1, Breaks); // Apply brakes while in backward direction
            }
            else
            {
                if (AccelFwd)
                    StopAccel(1, 0.1f); // Apply brakes slowly if no key is pressed while in forward direction
                else if (AccelBwd)
                    StopAccel(-1, 0.1f); // Apply brakes slowly if no key is pressed while in backward direction
            }
        }

        if (CarControlMode == ControlMode.Touch)
        {
            if (TouchAccel)
                Accel(1);
            else if (TouchBack)
                Accel(-1);
            else if (TouchBreaks)
            {
                if (AccelFwd)
                    StopAccel(1, Breaks);
                else if (AccelBwd)
                    StopAccel(-1, Breaks);
            }
            else
            {
                if (AccelFwd)
                    StopAccel(1, 0.1f);
                else if (AccelBwd)
                    StopAccel(-1, 0.1f);
            }
        }
    }

    // Functions to be called from Onscreen buttons for touch input.
    public void SetTouchAccel(bool TouchState)
    {
        TouchAccel = TouchState;
    }

    public void SetTouchBack(bool TouchState)
    {
        TouchBack = TouchState;
    }

    public void SetTouchBreaks(bool TouchState)
    {
        TouchBreaks = TouchState;
    }

    public void SetSteerLeft(bool TouchState)
    {
        SteerLeft = TouchState;
    }

    public void SetSteerRight(bool TouchState)
    {
        SteerRight = TouchState;
    }

    public void Accel(int Direction)
    {
        if (Direction == 1)
        {
            AccelFwd = true;
            if (Acceleration <= MaxSpeed)
            {
                Acceleration += 0.05f;
            }

            if (CarControlMode == ControlMode.KeyBoard)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                    transform.Rotate(Vector3.forward * Steer); // Steer left
                if (Input.GetKey(KeyCode.RightArrow))
                    transform.Rotate(Vector3.back * Steer); // Steer right
            }
            else if (CarControlMode == ControlMode.Touch)
            {
                if (SteerLeft)
                    transform.Rotate(Vector3.forward * Steer);
                else if (SteerRight)
                    transform.Rotate(Vector3.back * Steer);
            }
        }
        else if (Direction == -1)
        {
            AccelBwd = true;
            if ((-1 * MaxSpeed) <= Acceleration)
            {
                Acceleration -= 0.05f;
            }

            if (CarControlMode == ControlMode.KeyBoard)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                    transform.Rotate(Vector3.back * Steer); // Steer left (while in reverse direction)
                if (Input.GetKey(KeyCode.RightArrow))
                    transform.Rotate(Vector3.forward * Steer); // Steer right (while in reverse direction)
            }
            else if (CarControlMode == ControlMode.Touch)
            {
                if (SteerLeft)
                    transform.Rotate(Vector3.forward * Steer);
                else if (SteerRight)
                    transform.Rotate(Vector3.back * Steer);
            }
        }

        if (Steer <= MaxSteer)
            Steer += 0.01f;

        if (IsOnGravel)
            transform.Translate(Vector2.up * Acceleration * Time.deltaTime * 0.5f); // slow down on gravel
        else
            transform.Translate(Vector2.up * Acceleration * Time.deltaTime);
    }

    public void StopAccel(int Direction, float BreakingFactor)
    {
        if (Direction == 1)
        {
            if (Acceleration >= 0.0f)
            {
                Acceleration -= BreakingFactor;

                if (CarControlMode == ControlMode.KeyBoard)
                {
                    if (Input.GetKey(KeyCode.LeftArrow))
                        transform.Rotate(Vector3.forward * Steer);
                    if (Input.GetKey(KeyCode.RightArrow))
                        transform.Rotate(Vector3.back * Steer);
                }
                else if (CarControlMode == ControlMode.Touch)
                {
                    if (SteerLeft)
                        transform.Rotate(Vector3.forward * Steer);
                    else if (SteerRight)
                        transform.Rotate(Vector3.back * Steer);
                }
            }
            else
                AccelFwd = false;
        }
        else if (Direction == -1)
        {
            if (Acceleration <= 0.0f)
            {
                Acceleration += BreakingFactor;

                if (CarControlMode == ControlMode.KeyBoard)
                {
                    if (Input.GetKey(KeyCode.LeftArrow))
                        transform.Rotate(Vector3.back * Steer);
                    if (Input.GetKey(KeyCode.RightArrow))
                        transform.Rotate(Vector3.forward * Steer);
                }
                else if (CarControlMode == ControlMode.Touch)
                {
                    if (SteerLeft)
                        transform.Rotate(Vector3.forward * Steer);
                    else if (SteerRight)
                        transform.Rotate(Vector3.back * Steer);
                }
            }
            else
                AccelBwd = false;
        }

        if (Steer >= 0.0f)
            Steer -= 0.01f;

        if (IsOnGravel)
            transform.Translate(Vector2.up * Acceleration * Time.deltaTime * 0.5f); // slow down on gravel
        else
            transform.Translate(Vector2.up * Acceleration * Time.deltaTime);
    }

    // when the car enters the gravel area
    private void EnterGravel()
    {
        IsOnGravel = true;
    }

    // when the car exits the gravel area
    private void ExitGravel()
    {
        IsOnGravel = false;
    }

    // register collision with gravel
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Gravel"))
        {
            EnterGravel();
        }
    }

    // register exiting gravel area
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Gravel"))
        {
            ExitGravel();
        }
    }
}
