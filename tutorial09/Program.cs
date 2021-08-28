using Silk.NET.Abstractions;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;

namespace tutorial09
{
    internal static class Program
    {
        private static IWindow window;
        private static GL Gl;

        private static VBO Vbo;

        private const string pVSFileName = "shader.vs";
        private const string pFSFileName = "shader.fs";

        private static float Scale = 0.0f;
        private static int gWorldLocation;

        private static unsafe void OnRender(double Delta)
        {
            Gl.Clear((uint)ClearBufferMask.ColorBufferBit);

            Scale += 0.001f;

            Matrix4X4<float> World = new Matrix4X4<float>(
                MathF.Sin(Scale), 0.0f, 0.0f, 0.0f,
                0.0f, MathF.Sin(Scale), 0.0f, 0.0f,
                0.0f, 0.0f, MathF.Sin(Scale), 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f
                );

            Gl.UniformMatrix4(gWorldLocation, 1, Silk.NET.OpenGL.Boolean.True, World.M11);

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

            Console.WriteLine($"GL version: {Gl.GetStringS(GLEnum.Version)}");

            Gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            CreateVertexBuffer();

            CompileShaders();
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

        private static void AddShader(uint ShaderProgram, string pShaderText, GLEnum ShaderType)
        {
            uint ShaderObj = Gl.CreateShader(ShaderType);

            if (ShaderObj == 0)
            {
                throw new Exception($"Error creating shader type {ShaderType}");
            }

            Gl.ShaderSource(ShaderObj, pShaderText);
            Gl.CompileShader(ShaderObj);

            string infoLog = Gl.GetShaderInfoLog(ShaderObj);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                throw new Exception($"Error compiling shader {infoLog}");
            }

            Gl.AttachShader(ShaderProgram, ShaderObj);
        }

        private static void CompileShaders()
        {
            uint ShaderProgram = Gl.CreateProgram();

            if (ShaderProgram == 0)
            {
                throw new Exception("Error creating shader program");
            }

            string vs, fs;

            vs = System.IO.File.ReadAllText(pVSFileName);

            fs = System.IO.File.ReadAllText(pFSFileName);

            AddShader(ShaderProgram, vs, GLEnum.VertexShader);
            AddShader(ShaderProgram, fs, GLEnum.FragmentShader);

            Gl.LinkProgram(ShaderProgram);

            Gl.GetProgram(ShaderProgram, GLEnum.LinkStatus, out var linkStatus);
            if (linkStatus == 0)
            {
                throw new Exception($"Error linking shader {Gl.GetProgramInfoLog(ShaderProgram)}");
            }

            Gl.ValidateProgram(ShaderProgram);
            Gl.GetProgram(ShaderProgram, GLEnum.ValidateStatus, out var validateStatus);
            if (validateStatus == 0)
            {
                throw new Exception($"Invalid shader program: {Gl.GetProgramInfoLog(ShaderProgram)}");
            }

            Gl.UseProgram(ShaderProgram);

            gWorldLocation = Gl.GetUniformLocation(ShaderProgram, "gWorld");
        }

        private static void Main()
        {
            //Create a window.
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(1024, 768);
            options.Title = "Tutorial 09";

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