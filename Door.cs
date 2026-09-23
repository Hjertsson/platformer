using Platformer;
using SFML.Graphics;
using SFML.System;

namespace platformer;

public class Door : Entity
{

    public string NextRoom;
    public bool Unlocked = false;
    
    public Door() : base("tileset")
    {
        sprite.TextureRect = new IntRect(180, 103, 18, 23);
        sprite.Origin = new Vector2f(9, 11);
    }

    public override void Update(Scene scene, float dt)
    {
        if (Unlocked)
        {
            sprite.Color = Color.Black; // Sätter texturens färg till svart
            if (scene.FindByType<Hero>(out Hero hero))
            {
                if (Collision.RectangleRectangle(this.Bounds, hero.Bounds, out _))
                {
                    scene.Load(NextRoom);
                }
            }
        }
        base.Update(scene, dt);
    }

}