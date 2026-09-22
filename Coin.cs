using Platformer;
using SFML.Graphics;
using SFML.System;

namespace platformer;

public class Coin : Entity
{
    public int coinsCollected = 0;
    
    public Coin() : base("tileset")
    {
        sprite.TextureRect = new IntRect(198, 126, 18, 18);
        sprite.Origin = new Vector2f(9, 11);
        
    }

    public override void Update(Scene scene, float dt)
    {
        if (scene.FindByType<Hero>(out Hero hero))
        {
            if (Collision.RectangleRectangle(Bounds, hero.Bounds, out _))
            {
                Dead = true;
                scene.coinsCollected++;
            }            
            
        }
        base.Update(scene, dt);
    }


}