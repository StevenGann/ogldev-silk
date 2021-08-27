# ogldev-silk
Port of the _OGLdev Modern OpenGL Tutorials_ examples from C++ to C#, featuring Silk.NET
****

## Notes:

I've done as much as possible to keep the OpenGL code 1:1 with OGLdev's C++, but there's a couple things that are definitely different.

- **Windowing:** I elected to use Silk.NET's SDL bindings for windowing rather than GLUT simply because Silk.NET doesn't have bindings to GLUT, and SDL is what Silk.NET's own examples use.

- **Program Structure:** Silk.NET's window management is structured differently from GLUT's, making use of event handlers like `OnLoad()`. For the overall structure of each program I used the same approach as Silk.NET's examples. 

Other things were kepts exactly the same.

- **Naming Conventions:** When it comes to naming variables and functions I chose to copy OGLdev's naming conventions as closely as possible, which means the code is a little ugly by C# standards.

- **Shaders:** GLSL works the same whether you're using C++ or C#, so the shaders are 100% copy-pasted from OGLdev's example code with no changes at all.

****
**The Tutorials:** [OGLdev Modern OpenGL Tutorials](https://www.ogldev.org/) (Original C++ code available there)

**Silk.NET Examples:** [Silk.NET OpenGL Tutorials](https://github.com/dotnet/Silk.NET/tree/main/examples/CSharp/OpenGL%20Tutorials)

Special thanks to **WholesomeIsland** for their handy Silk.NET abstractions, parts of which I'm using in place of some of GLUT's features:
[GitHub](https://github.com/WholesomeIsland/Silk.NET.OpenGL.Abstractions)
