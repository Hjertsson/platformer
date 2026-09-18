using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace platformer;

class Program
{
    static void Main(string[] args)
    {
        static void Main(string[] args)
        {
            using (var window = new RenderWindow(
                       new VideoMode(800, 600), "Platformer"))
            {
                window.Closed += (o, e) => window.Close();
// TODO: Initialize
                Clock clock = new Clock();
                while (window.IsOpen)
                {
                    window.DispatchEvents();
                    float deltaTime = clock.Restart().AsSeconds();
// TODO: Updates
                    window.Clear();
// TODO: Drawing
                    window.Display();
                }
            }
        }
    }
}