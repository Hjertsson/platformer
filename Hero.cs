using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace platformer;

public class Hero : Entity
{
    public const float WalkSpeed = 100.0f;
    public const float JumpForce = 250.0f;
    public const float GravityForce = 400.0f;
    private float verticalSpeed;
    private bool isGrounded;
    private bool isUpPressed;
    
    
    private bool faceRight = false;
    public Hero() : base("characters")
    {
        sprite.TextureRect = new IntRect(0, 0, 24, 24);
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
            faceRight = false;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
        {
            scene.TryMove(this, new Vector2f(WalkSpeed * dt, 0));
            faceRight = true;
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
        {
            if (isGrounded && !isUpPressed)
            {
                verticalSpeed = -JumpForce;
                isUpPressed = true;
            }
            else
            {
                isUpPressed = false;
            }

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

    }

    public override FloatRect Bounds
    {
        get
        {
            var bounds = base.Bounds;
            bounds.Left += 3;
            bounds.Width -= 6;
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

    public override void Render(RenderTarget target)
    {
        sprite.Scale = new Vector2f(faceRight ? -1 : 1, 1); // Om faceRight = true är x = -1, annars x = 1. Y = 1 alltid.
        base.Render(target);
    }
}