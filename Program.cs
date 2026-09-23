using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Text;

namespace platformer;

class Program
{
    public const int SCREEN_HEIGHT = 300;
    public const int SCREEN_WIDTH = 400;
    static void Main(string[] args)
    {
        Scene scene = new Scene();
        scene.Load("level0");
        
        using (var window = new RenderWindow(
                   new VideoMode(800, 600), "Platformer"))
        {
            window.Closed += (o, e) => window.Close();
            window.SetView(new View(
                new Vector2f(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2), //För att få kameran att följa spelaren så lägg denna i while loopen och ändra första värdet till spelarens position
                new Vector2f(SCREEN_WIDTH, SCREEN_HEIGHT)));
            Clock clock = new Clock();
            while (window.IsOpen)
            {
                window.DispatchEvents();
                float dt = clock.Restart().AsSeconds();
                if (dt > 0.1) dt = 0.1f;
                scene.UpdateAll(dt);
                window.Clear();
                scene.RenderAll(window);
                window.Display();
            }
        }
    }
}
