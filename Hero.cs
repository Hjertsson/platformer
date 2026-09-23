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
    private readonly IntRect standing = new IntRect(0, 0, 24, 24);
    private readonly IntRect midAir =  new IntRect(24, 0, 24, 24);
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
            IsRunning();
            faceRight = false;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
        {
            scene.TryMove(this, new Vector2f(WalkSpeed * dt, 0));
            IsRunning();
            faceRight = true;
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
        {
            if (isGrounded && !isUpPressed) // Om isGrounded är true och isUpPressed (pil up knappen) är false, så sätter vi sprite.textureRect (Spritens hitbox) till midair
                                            // Vilket stoppar sprng animationen. Sätter veticalSpeed till det negativa värdet av jumpForce.
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
        if (!Keyboard.IsKeyPressed(Keyboard.Key.Up) && // Om spelaren inte trycker på någon knapp, så ska spriten ha sin "idle" textur
            !Keyboard.IsKeyPressed(Keyboard.Key.Right) &&
            !Keyboard.IsKeyPressed(Keyboard.Key.Left))
        {
            sprite.TextureRect = standing;
        }
        if (!isGrounded) //Om karaktären är i luften, så sätts TextureRect till midair
        {
            sprite.TextureRect = midAir;
        }
        
        isGrounded = false;
        Vector2f velocity = new Vector2f(0, verticalSpeed * dt); // Sätter spelarens velocity
        if (scene.TryMove(this, velocity))
        {
            if (verticalSpeed > 0.0f) //Om karaktärens verticalspeed är 0 så betyder det att den är på marken, och isGrounded sätts till true
            {
                isGrounded = true;
                verticalSpeed = 0.0f;
            }
            else
            {
                verticalSpeed = 0.5f * verticalSpeed; //Om inte vertical speed är 0 så halveras verticcal speed
            }
        }
        verticalSpeed += GravityForce * dt;
        if (verticalSpeed > 500.0f) verticalSpeed = 500.0f; // Vertical speed får aldrig vara mer än 500
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
        bool left = sprite.Position.X < 0;
        bool top = sprite.Position.Y < 0;
        bool right = sprite.Position.X >= Program.SCREEN_WIDTH;
        bool bottom = sprite.Position.Y >= Program.SCREEN_HEIGHT;
        
        return left || right || top || bottom;
    }

    private void IsRunning()
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