module Shaders

open System.IO
open System.Text
open OpenTK.Graphics.OpenGL

type Shader =
    struct

        val vertexShaderPath: string
        val fragmentShaderPath: string

        val program: int
        val vertexShader: int
        val fragmentShader: int

        new (VertexShaderPath, FragmentShaderPath) = {

            vertexShaderPath = VertexShaderPath;
            fragmentShaderPath = FragmentShaderPath;
            program = GL.CreateProgram()

            vertexShader = GL.CreateShader(ShaderType.VertexShader)
            fragmentShader = GL.CreateShader(ShaderType.FragmentShader)
        }

        member this.CreateShader() =
            printfn "%i" this.program

            let vreader = new StreamReader(this.vertexShaderPath, Encoding.UTF8)
            let freader = new StreamReader(this.fragmentShaderPath, Encoding.UTF8)
            let vertexShaderSource = vreader.ReadToEnd()
            let fragmentShaderSource = freader.ReadToEnd()

            GL.ShaderSource(this.vertexShader, vertexShaderSource)
            GL.ShaderSource(this.fragmentShader, fragmentShaderSource)
            GL.CompileShader(this.vertexShader)
            GL.CompileShader(this.fragmentShader)

            GL.AttachShader(this.program, this.vertexShader)
            GL.AttachShader(this.program, this.fragmentShader)
            GL.LinkProgram(this.program)

        member this.Use() =
            GL.UseProgram(this.program)
    end