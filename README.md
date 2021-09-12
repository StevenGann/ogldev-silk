# ogldev-silk
Port of the _OGLdev Modern OpenGL Tutorials_ examples from C++ to C#, featuring Silk.NET
****

## Notes:

I've done as much as possible to keep the OpenGL code 1:1 with OGLdev's C++, but there's a couple things that are definitely different.

- **VAOs:** The original Tutorials don't use a VAO until Tutorial10, instead opting to depend on GLUT-Weirdness. As Silk.NET does not expose this functionallity and it is also not good OpenGL-Practice I use a VAO in these episodes, but explain it in more detail when apropriate. [Source: Robbe and Redstone#8025 Silk.NET Discord]

- **Windowing:** I elected to use Silk.NET's SDL bindings for windowing rather than GLUT simply because Silk.NET doesn't have bindings to GLUT, and SDL is what Silk.NET's own examples use.

- **Program Structure:** Silk.NET's window management is structured differently from GLUT's, making use of event handlers like `OnLoad()`. For the overall structure of each program I used the same approach as Silk.NET's examples. 

Other things were kepts exactly the same.

- **Naming Conventions:** When it comes to naming variables and functions I chose to copy OGLdev's naming conventions as closely as possible, which means the code is a little ugly by C# standards.

- **Shaders:** GLSL works the same whether you're using C++ or C#, so the shaders are 100% copy-pasted from OGLdev's example code with no changes at all.

****

## Progress:

\# | Title | Status | Links
-- | -------------------------- | ------ | -------------
 1 | Open a window              | Ported | [Source](https://github.com/StevenGann/ogldev-silk/tree/main/tutorial01) / [Tutorial](https://www.ogldev.org/www/tutorial01/tutorial01.html)
 2 | Hello dot!                 | Ported | [Source](https://github.com/StevenGann/ogldev-silk/tree/main/tutorial02) / [Tutorial](https://www.ogldev.org/www/tutorial02/tutorial02.html)
 3 | First triangle             | Ported | [Source](https://github.com/StevenGann/ogldev-silk/tree/main/tutorial03) / [Tutorial](https://www.ogldev.org/www/tutorial03/tutorial03.html)
 4 | Shaders                    | Ported | [Source](https://github.com/StevenGann/ogldev-silk/tree/main/tutorial04) / [Tutorial](https://www.ogldev.org/www/tutorial04/tutorial04.html)
 5 | Uniform variables          | Ported | [Source](https://github.com/StevenGann/ogldev-silk/tree/main/tutorial05) / [Tutorial](https://www.ogldev.org/www/tutorial05/tutorial05.html)
 6 | Translation transformation | Ported | [Source](https://github.com/StevenGann/ogldev-silk/tree/main/tutorial06) / [Tutorial](https://www.ogldev.org/www/tutorial06/tutorial06.html)
 7 | Rotation transformation    | Ported | [Source](https://github.com/StevenGann/ogldev-silk/tree/main/tutorial07) / [Tutorial](https://www.ogldev.org/www/tutorial07/tutorial07.html)
 8 | Scaling transformation     | Ported | [Source](https://github.com/StevenGann/ogldev-silk/tree/main/tutorial08) / [Tutorial](https://www.ogldev.org/www/tutorial08/tutorial08.html)
 9 | Interpolation              | Ported | [Source](https://github.com/StevenGann/ogldev-silk/tree/main/tutorial09) / [Tutorial](https://www.ogldev.org/www/tutorial09/tutorial09.html)
10 | Indexed draws              | Ported | [Source](https://github.com/StevenGann/ogldev-silk/tree/main/tutorial10) / [Tutorial](https://www.ogldev.org/www/tutorial10/tutorial10.html)
11 | Concatenating transformations | Ported | [Source](https://github.com/StevenGann/ogldev-silk/tree/main/tutorial11) / [Tutorial](https://www.ogldev.org/www/tutorial11/tutorial11.html)
12 | Perspective Projection     | Bugged | [Source](https://github.com/StevenGann/ogldev-silk/tree/main/tutorial12) / [Tutorial](https://www.ogldev.org/www/tutorial12/tutorial12.html)

****

## References:

**The Tutorials:** [OGLdev Modern OpenGL Tutorials](https://www.ogldev.org/) (Original C++ code available there)

**Silk.NET Examples:** [Silk.NET OpenGL Tutorials](https://github.com/dotnet/Silk.NET/tree/main/examples/CSharp/OpenGL%20Tutorials)

Special thanks to **WholesomeIsland** for their handy Silk.NET abstractions, parts of which I'm using in place of some of GLUT's features:
[GitHub](https://github.com/WholesomeIsland/Silk.NET.OpenGL.Abstractions)
