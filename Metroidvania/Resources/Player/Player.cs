using Godot;
using System;

public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    private int ACCELERATION = 512;
    [Export]
    private int MAX_SPEED = 64;
    [Export]
    private float FRICTION = 0.25f;
    [Export]
    private int GRAVITY = 200;
    [Export]
    private float JUMP_FORCE = 128;
    [Export]
    private float MAX_SLOPE_ANGLE = 46;

    [Export]
    public Vector2 motion = Vector2.Zero;
    private Vector2 snapVector = Vector2.Zero;
    private AnimationPlayer animationPlayer = null;
    private Sprite sprite = null;

    private bool justJumped = false;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animationPlayer = (AnimationPlayer)GetNode("SpriteAnimator");
        sprite = (Sprite)GetNode("Sprite");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        justJumped = false;

        Vector2 inputVector = GetInputVector();
        ApplyHorizontalForce(inputVector, delta);
        ApplyFriction(inputVector);
        UpdateSnapVector();
        JumpCheck();
        ApplyGravity(delta);
        UpdateAnimations(inputVector);
        Move();
        // below is for topdown movement
        // // If inputVector is not a zero vector (0,0) that means that the player wants to move
        // if (inputVector != Vector2.Zero)
        // {
        //     // set the motion to the inputVector multiplied by the acceleration multipled by delta
        //     // delta needs to be applied to acceleration and gravity but not motion
        //     motion += inputVector * ACCELERATION * delta;

        //     // Clamp the vector's length so the character doesn't move too fast 
        //     motion = motion.Clamped(MAX_SPEED);
        // }
        // else
        // {
        //     // set motion by Linear Interpolate
        //     // The first parameter is the vector we want to approach (Vector2.zero)
        //     // The second parameter is the amount of motion reduction each frame (FRICTION = 0.25)
        //     // DOCS: Returns the result of the linear interpolation between
        //     // this vector and `to` by amount `weight`.
        //     motion = motion.LinearInterpolate(Vector2.Zero, FRICTION);
        // }

        // motion = MoveAndSlide(motion);
    }

    public Vector2 GetInputVector()
    {
        Vector2 inputVector = Vector2.Zero;
        float rightInputStrength = Input.GetActionStrength("ui_right");
        float leftInputStrength = Input.GetActionStrength("ui_left");

        float downInputStrength = Input.GetActionStrength("ui_down");
        float upInputStrength = Input.GetActionStrength("ui_up");

        // x input will be 1, -1, or 0
        // EX (if pressing right)
        // 1 - 0 = 1
        // EX (if pressing left)
        // 0 - 1 = -1
        // EX (if pressing both)
        // 1 - 1 = 0
        // This gives us the x direction the player should be moving in
        inputVector.x = rightInputStrength - leftInputStrength;

        // y input will be 1, -1, or 0
        // EX (if pressing down)
        // 1 - 0 = 1
        // EX (if pressing up)
        // 0 - 1 = -1
        // EX (if pressing both)
        // 1 - 1 = 0
        // This gives us the y direction the player should be moving in (not needed for playformers)
        // inputVector.y = downInputStrength - upInputStrength;

        return inputVector;
    }

    public void ApplyHorizontalForce(Vector2 inputVector, float delta)
    {
        if (inputVector.x != 0)
        {
            motion.x += inputVector.x * ACCELERATION * delta;
            // Clamps a `value` so that it is not less than `min` and not more than `max`.
            // Returns the clamped value
            motion.x = Mathf.Clamp(motion.x, -MAX_SPEED, MAX_SPEED);
        }
    }

    public void ApplyFriction(Vector2 inputVector)
    {
        if (inputVector.x == 0 && IsOnFloor())
        {
            motion.x = Mathf.Lerp(motion.x, 0, FRICTION);
        }
    }


    public void UpdateSnapVector()
    {
        if (IsOnFloor())
        {
            snapVector = Vector2.Down;
        }
    }

    public void Move()
    {
        //Check if is on floor
        bool wasOnFloor = IsOnFloor();

        //Check if was in air
        bool wasInAir = !IsOnFloor();

        Vector2 lastPosition = Position;
        Vector2 lastMotion = motion;

        //Move Character
        //MoveAndSlideWithSnap is good for characters walking on a slope
        motion = MoveAndSlideWithSnap(motion, snapVector * 4, Vector2.Up, false, 4, Mathf.Deg2Rad(MAX_SLOPE_ANGLE));
        //Landing
        //Check if we were in the air and we are now on the floor then we've landed 
        if (wasInAir && IsOnFloor())
        {
            // set the motion.x speed to the previous motion.x speed
            // This ensures that we keep the previous momentum on slopes after a jump
            motion.x = lastMotion.x;
        }
        //Just left ground
        //Check if the character was just on the floor AND is now not on the Floor AND they if the character has not just jumped 
        if (wasOnFloor && !IsOnFloor() && !justJumped)
        {
            motion.y = 0;

            //Small jump of edge fix
            Position = new Vector2(Position.x, lastPosition.y);
        }

        // Prevent Sliding (hack)
        // Check if character is on the floor AND the Floor isn't moving AND the current x motion is less than 1 (very small motion)
        if (IsOnFloor() && GetFloorVelocity().Length() == 0 && Math.Abs(motion.x) < 1)
        {
            Position = new Vector2(lastPosition.x, Position.y);
        }
    }

    public void JumpCheck()
    {

        if (IsOnFloor())
        {
            if (Input.IsActionJustPressed("ui_up"))
            {
                motion.y = -JUMP_FORCE;
                justJumped = true;
                snapVector = Vector2.Zero;
            }
        }
        else
        {
            // motion.y < -JUMP_FORCE / 2
            // when the player is falling down, they cannot press the jump button
            // motion.y will be greater than -JUMP_FORCE/2
            if (Input.IsActionJustReleased("ui_up") && motion.y < -JUMP_FORCE / 2)
            {
                motion.y = -JUMP_FORCE / 2;
            }
        }
    }

    public void ApplyGravity(float delta)
    {
        if (!IsOnFloor())
        {
            motion.y += GRAVITY * delta;
            motion.y = Mathf.Min(motion.y, JUMP_FORCE);
        }
    }

    public void UpdateAnimations(Vector2 inputVector)
    {
        Vector2 spriteScaleTemp = sprite.Scale;

        sprite.FlipH = inputVector.x < 0 ? true : false;

        if (inputVector.x != 0)
        {
            //we sign this because if a person uses a controller then the value will be a float eg 0.2
            spriteScaleTemp.x = Mathf.Sign(inputVector.x);
            animationPlayer.Play("Run");
        }
        else
        {
            animationPlayer.Play("Idle");
        }

        if (!IsOnFloor())
        {
            animationPlayer.Play("Jump");
        }
    }


}
