using System.Runtime.InteropServices.Marshalling;
using SFML.Graphics;
using SFML.System;

namespace platformer;

public class Entity
{
    private readonly string textureName;
    protected readonly Sprite sprite;
    public bool Dead;
    
    protected Entity(string textureName)
    {
        this.textureName = textureName;
        sprite = new Sprite();
    }

    public Vector2f Position // TODO: Sätt en förklaring efter föreläsning
    {
        get => sprite.Position;
        set => sprite.Position = value;
    }

    public virtual FloatRect Bounds => sprite.GetGlobalBounds(); 

    public void Create(Scene scene)
    {
        sprite.Texture = scene.LoadTexture(textureName);
    }

    public virtual bool Solid => false; 
    public virtual void CheckHit(Scene scene){} // Är virtual för att kunna använda breakables override i Scene
    public virtual void Update(Scene scene, float dt) {}
    public virtual void Render(RenderTarget target)
    {
        target.Draw(sprite);
    }
}