#version 330 core
layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec3 aColor;
layout(location = 2) in vec2 aTextcoords;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
out vec3 outColor;
out vec2 Textcoords;
void main(void)
{
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
    outColor = aColor;
    Textcoords = aTextcoords;
}  