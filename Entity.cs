using System.Runtime.InteropServices.Marshalling;
using SFML.Graphics;
using SFML.System;

namespace platformer;

public class Entity
{
    private readonly string textureName;
    protected readonly Sprite sprite;
    public bool Dead;
    
    public Entity()
    {
        
    }
    
    protected Entity(string textureName)
    {
        this.textureName = textureName;
        sprite = new Sprite();
    }

    public Vector2f Position
    {
        get => sprite.Position;
        set => sprite.Position = value;
    }

    public virtual FloatRect Bounds => sprite.GetGlobalBounds();

    public void Create(Scene scene)
    {
        
    }

    public void Update(Scene scene, float dt)
    {
        
    }

    public void Render(RenderTarget target)
    {
        
    }
}