using Silk.NET.Abstractions;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;
using System.Numerics;

namespace tutorial03
{
    internal static class Program
    {
        private static IWindow window;
        private static GL Gl;

        private static VBO Vbo;

        private static void OnRender(double Delta)
        {
            Gl.Clear((uint)ClearBufferMask.ColorBufferBit);

            Gl.EnableVertexAttribArray(0);

            Vbo.Bind();

            Gl.VertexAttribPointer(0, 3, GLEnum.Float, Silk.NET.OpenGL.Boolean.False, 0, 0);

            Gl.DrawArrays(GLEnum.Triangles, 0, 3);

            Gl.DisableVertexAttribArray(0);
        }

        private static void OnUpdate(double Delta)
        {
        }

        private static void OnLoad()
        {
            Gl = GL.GetApi(window);

            CreateVertexBuffer();
        }

        private static void CreateVertexBuffer()
        {
            float[] vertices = new float[]
            {
                -1.0f, -1.0f, 0.0f,
                1.0f, -1.0f, 0.0f,
                0.0f, 1.0f, 0.0f
            };

            Vbo = new VBO(Gl, vertices, GLEnum.StaticDraw);
        }

        private static void Main()
        {
            //Create a window.
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(1024, 768);
            options.Title = "Tutorial 03";

            window = Window.Create(options);

            //Assign events.
            window.Load += OnLoad;
            window.Update += OnUpdate;
            window.Render += OnRender;

            //Run the window.
            window.Run();
        }
    }
}