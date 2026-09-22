using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace platformer;

public class Hero : Entity
{
    private const float WalkSpeed = 100.0f;
    private const float JumpForce = 250.0f;
    private const float GravityForce = 400.0f;
    private float timer;
    private float verticalSpeed;
    private bool isGrounded;
    private bool isUpPressed;
    private IntRect standing = new IntRect(0, 0, 24, 24);
    private IntRect midAir =  new IntRect(24, 0, 24, 24);
    
    
    private bool faceRight = false;
    public Hero() : base("characters")
    {
        sprite.TextureRect = standing;
        sprite.Origin = new Vector2f(12, 12);
    }

    public override void Update(Scene scene, float dt)
    {
        if (IsOutOfBounds())
        {
            scene.Reload();
        }
        
        if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
        {
            scene.TryMove(this, new Vector2f(-WalkSpeed * dt, 0));
            isRunning();
            faceRight = false;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
        {
            scene.TryMove(this, new Vector2f(WalkSpeed * dt, 0));
            isRunning();
            faceRight = true;
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
        {
            
            if (isGrounded && !isUpPressed)
            {
                sprite.TextureRect = midAir;
                verticalSpeed = -JumpForce;
                isUpPressed = true;
            }
            else
            {
                isUpPressed = false;
            }
        
        }
        if (!Keyboard.IsKeyPressed(Keyboard.Key.Up) &&
            !Keyboard.IsKeyPressed(Keyboard.Key.Right) &&
            !Keyboard.IsKeyPressed(Keyboard.Key.Left))
        {
            sprite.TextureRect = standing;
        }
        

        if (!isGrounded)
        {
            sprite.TextureRect = midAir;
        }
        
        isGrounded = false;
        Vector2f velocity = new Vector2f(0, verticalSpeed * dt);
        if (scene.TryMove(this, velocity))
        {
            if (verticalSpeed > 0.0f)
            {
                isGrounded = true;
                verticalSpeed = 0.0f;
            }
            else
            {
                verticalSpeed = 0.5f * verticalSpeed;
            }
        }    
        
        
        verticalSpeed += GravityForce * dt;
        if (verticalSpeed > 500.0f) verticalSpeed = 500.0f;
        timer += dt;
    }

    public override FloatRect Bounds
    {
        get
        {
            var bounds = base.Bounds;
            bounds.Left += 3;
            bounds.Width -= 7;
            bounds.Top += 3;
            bounds.Height -= 3;
            return bounds;
        }
    }
    private bool IsOutOfBounds()
    {
        bool left = this.sprite.Position.X < 0;
        bool top = this.sprite.Position.Y < 0;
        bool right = this.sprite.Position.X >= Program.SCREEN_WIDTH;
        bool bottom = this.sprite.Position.Y >= Program.SCREEN_HEIGHT;
        
        return left || right || top || bottom;
    }

    private void isRunning()
    {
        switch (timer)
        {
            case < 0.2f :
                sprite.TextureRect = standing;
                break;
            case > 0.2f and < 0.4f :
                sprite.TextureRect = midAir;
                break;
            case > 0.4f:
                timer = 0;
                break;
        }
    }

    public override void Render(RenderTarget target)
    {
        sprite.Scale = new Vector2f(faceRight ? -1 : 1, 1); // Om faceRight = true är x = -1, annars x = 1. Y = 1 alltid.
        
        base.Render(target);
    }
}