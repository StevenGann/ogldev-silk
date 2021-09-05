﻿using Silk.NET.Abstractions;
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

        private static VBO Vbo;
        private static EBO Ibo;

        //private static VAO Vao;
        private static uint ShaderProgram;

        private const string pVSFileName = "shader.vs";
        private const string pFSFileName = "shader.fs";

        private static float Scale = 0.0f;
        private static int gWorldLocation;

        private static unsafe void OnRender(double Delta)
        {
            Gl.Clear((uint)ClearBufferMask.ColorBufferBit);

            Scale += 0.01f;

            Matrix4X4<float> World = new Matrix4X4<float>(
                MathF.Sin(Scale), 0.0f, 0.0f, 0.0f,
                0.0f, MathF.Sin(Scale), 0.0f, 0.0f,
                0.0f, 0.0f, MathF.Sin(Scale), 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f
                );

            Gl.UniformMatrix4(gWorldLocation, 1, Silk.NET.OpenGL.Boolean.True, (float*)&World);

            Gl.EnableVertexAttribArray(0);

            Vbo.Bind();
            Ibo.Bind();

            Gl.VertexAttribPointer(0, 3, GLEnum.Float, false, 0, null);

            Gl.UseProgram(ShaderProgram);

            Gl.DrawElements(GLEnum.Triangles, 12, GLEnum.UnsignedInt, null);

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

            CreateIndexBuffer();

            CompileShaders();
        }

        private static void CreateVertexBuffer()
        {
            float[] vertices = new float[]
            {
                -1.0f, -1.0f, 0.0f,
                0.0f, -1.0f, 1.0f,
                1.0f, -1.0f, 0.0f,
                0.0f, 1.0f, 0.0f
            };

            Vbo = new VBO(Gl, vertices, GLEnum.StaticDraw);
        }

        private static void CreateIndexBuffer()
        {
            uint[] Indices =
            { 0, 3, 1,
              1, 3, 2,
              2, 3, 0,
              0, 1, 2 };

            Ibo = new EBO(Gl, Indices);
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
            ShaderProgram = Gl.CreateProgram();

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

            Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), null);
            Gl.EnableVertexAttribArray(0);

            Gl.UseProgram(ShaderProgram);

            gWorldLocation = Gl.GetUniformLocation(ShaderProgram, "gWorld");
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