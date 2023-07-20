#version 330
in vec3 outColor;
out vec4 outputColor;
in vec2 Textcoords;
uniform sampler2D ourTexture;
void main()
{
    outputColor = texture(ourTexture, Textcoords);

}