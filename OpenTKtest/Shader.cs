using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OpenTKtest
{
    public class Shader
    {
        public readonly int Handle;
        private readonly Dictionary<string, int> _uniformLocations;
        public Shader(string vertPath, string fragPath)
        {
            //创建顶点着色器
            var shaderSource = LoadSource(vertPath);
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            //绑定GLSL代码
            GL.ShaderSource(vertexShader, shaderSource);

            //检测是否编译错误
            CompileShader(vertexShader);


            //创建片段着色器
            shaderSource = LoadSource(fragPath);
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, shaderSource);
            //检测是否编译错误
            CompileShader(fragmentShader);


            //创建编译程序
            Handle = GL.CreateProgram();

            //绑定着色器
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            //链接程序
            LinkProgram(Handle);

            //绑定后，分离和删除着色器
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);
            _uniformLocations = new Dictionary<string, int>();
            for (var i = 0; i < numberOfUniforms; i++)
            {
                //获取uniform的名字
                var key = GL.GetActiveUniform(Handle, i, out _, out _);
                //获取location
                var location = GL.GetUniformLocation(Handle, key);
                _uniformLocations.Add(key, location);
            }

        }
        private static void CompileShader(int shader)
        {
            // 编译着色器
            GL.CompileShader(shader);

            // 检测是否编译错误
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                // 获取错误信息
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }
        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);

            //检测是否连接错误
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                throw new Exception($"Error occurred whilst linking Program({program})");
            }


        }
        //使用程序
        public void Use()
        {
            GL.UseProgram(Handle);
        }
        public int getAttribLocation(string atrribName)
        {
            return GL.GetAttribLocation(Handle, atrribName);
        }
        // 加载着色器路径
        private static string LoadSource(string path)
        {
            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }
        public void SetInt(string name,int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }
        public void SetFloat(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true,ref data);
        }
        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(_uniformLocations[name], data);
        }

    }
}