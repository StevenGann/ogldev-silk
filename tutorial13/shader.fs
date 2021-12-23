#version 410

layout (location = 0) in vec4 Color;

out vec4 FragColor;

void main()
{
    FragColor = Color;
}