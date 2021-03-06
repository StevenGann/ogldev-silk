using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;

namespace tutorial10
{
    internal static class Program
    {
        private static IWindow window;
        private static GL Gl;

        private static uint Vbo;
        private static uint Ibo;
        private static uint Vao;

        private const string pVSFileName = "shader.vs";
        private const string pFSFileName = "shader.fs";

        private static float Scale = 0.0f;
        private static int gWorldLocation;

        private static unsafe void OnRender(double Delta)
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit);

            Scale += 0.01f;

            Matrix4X4<float> World = new Matrix4X4<float>(
                MathF.Cos(Scale), 0.0f, -MathF.Sin(Scale), 0.0f,
                0.0f, 1.0f, 0.0f, 0.0f,
                MathF.Sin(Scale), 0.0f, MathF.Cos(Scale), 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f
                );

            Gl.UniformMatrix4(gWorldLocation, 1, true, (float*)&World);

            Gl.BindVertexArray(Vao);

            Gl.EnableVertexAttribArray(0);
            Gl.BindBuffer(GLEnum.ArrayBuffer, Vbo);
            Gl.VertexAttribPointer(0, 3, GLEnum.Float, false, 0, null);
            Gl.BindBuffer(GLEnum.ElementArrayBuffer, Ibo);

            Gl.DrawElements(GLEnum.Triangles, 12, DrawElementsType.UnsignedInt, null);

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

            Vao = Gl.GenVertexArray();

            CreateVertexBuffer();

            CreateIndexBuffer();

            CompileShaders();
        }

        private static unsafe void CreateVertexBuffer()
        {
            Span<float> Vertices = new(new float[]
            { -1.0f, -1.0f, 0.0f,
                0.0f, -1.0f, 1.0f,
                1.0f, -1.0f, 0.0f,
                0.0f, 1.0f, 0.0f });

            Gl.GenBuffers(1, out Vbo);
            Gl.BindBuffer(GLEnum.ArrayBuffer, Vbo);
            fixed (void* v = Vertices)
            {
                Gl.BufferData(GLEnum.ArrayBuffer, (nuint)(sizeof(float) * Vertices.Length), v, GLEnum.StaticDraw);
            }
        }

        private static unsafe void CreateIndexBuffer()
        {
            Span<uint> Indices = new(new uint[]
            { 0, 3, 1,
              1, 3, 2,
              2, 3, 0,
              0, 1, 2 });

            Gl.GenBuffers(1, out Ibo);
            Gl.BindBuffer(GLEnum.ElementArrayBuffer, Ibo);
            fixed (void* i = Indices)
            {
                Gl.BufferData(GLEnum.ElementArrayBuffer, (nuint)(sizeof(uint) * Indices.Length), i, GLEnum.StaticDraw);
            }
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

        private static unsafe void CompileShaders()
        {
            uint ShaderObj = Gl.CreateProgram();

            if (ShaderObj == 0)
            {
                throw new Exception("Error creating shader program");
            }

            string vs, fs;

            vs = System.IO.File.ReadAllText(pVSFileName);

            fs = System.IO.File.ReadAllText(pFSFileName);

            AddShader(ShaderObj, vs, GLEnum.VertexShader);
            AddShader(ShaderObj, fs, GLEnum.FragmentShader);

            Gl.LinkProgram(ShaderObj);

            Gl.GetProgram(ShaderObj, GLEnum.LinkStatus, out var linkStatus);
            if (linkStatus == 0)
            {
                throw new Exception($"Error linking shader {Gl.GetProgramInfoLog(ShaderObj)}");
            }

            Gl.ValidateProgram(ShaderObj);
            Gl.GetProgram(ShaderObj, GLEnum.ValidateStatus, out var validateStatus);
            if (validateStatus == 0)
            {
                throw new Exception($"Invalid shader program: {Gl.GetProgramInfoLog(ShaderObj)}");
            }

            Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), null);
            Gl.EnableVertexAttribArray(0);

            Gl.UseProgram(ShaderObj);

            gWorldLocation = Gl.GetUniformLocation(ShaderObj, "gWorld");
        }

        private static void Main()
        {
            //Create a window.
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(1024, 768);
            options.Title = "Tutorial 10";

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