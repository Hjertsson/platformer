using Platformer;
using SFML.Graphics;

namespace platformer;

public class Breakable : Platform
{
    public Breakable()
    {
        sprite.TextureRect = new IntRect(0, 36, 18, 18);
    }
    
    public override void CheckHit(Scene scene)
    {
        if (scene.FindByType(out Hero hero)) // Ger oss access till hero-objektet
        {
            if (Collision.RectangleRectangle(Bounds, hero.Bounds, out _)) // Om det skett en kollision mellan breakableobjektet och hero.
            {
                if (hero.Position.Y - hero.Bounds.Height /2 >= Position.Y + 6 
                    && hero.Position.X - hero.Bounds.Width / 2 >= Position.X - Bounds.Width
                    && hero.Position.X + hero.Bounds.Width /2 <= Position.X + Bounds.Width) // Om kollisionen skedde när hero var under breakable objektet
                {
                    Dead = true; // Ta bort breakableobjektet
                }
            }            
        }
    }
}