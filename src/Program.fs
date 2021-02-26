module Main
open System

open OpenTK.Windowing.Common
open OpenTK.Windowing.Desktop
open OpenTK.Graphics.OpenGL
open OpenTK.Mathematics
open OpenTK.Windowing.GraphicsLibraryFramework;

open Shaders
open _Camera

type Scene(GWS, NWS) =
    inherit GameWindow(GWS, NWS)

    let vertices = [|
        // R     G     B
        -1.5f; -1.5f; 0.0f; 1.0f; 0.0f; 0.0f;
         1.5f; -1.5f; 0.0f; 0.0f; 1.0f; 0.0f;
         0.0f;  1.5f; 1.5f; 0.0f; 0.0f; 1.0f
    |]

    let shader = new Shader("src/glsl/vertexShader.glsl", "src/glsl/fragmentShader.glsl")
    let vao = GL.GenVertexArray()
    let vbo = GL.GenBuffer()


    override this.OnMouseMove(e) =
        if (canRotate = true) then 

            xRotation <- xRotation + (float (lastMousePosition.Y - e.Y)) * 0.5
            yRotation <- yRotation - (float (lastMousePosition.X - e.X)) * 0.5
        else printfn ""

        lastMousePosition <- new Vector2(e.X, e.Y)
        printfn ""

    override this.OnMouseDown(e) =
        if (e.Button = MouseButton.Right) then 
            canRotate <- true
        else printfn ""

    override this.OnMouseUp(e) =
        if (e.Button = MouseButton.Right) then 
            canRotate <- false
        else printfn ""


    override this.OnUpdateFrame(e) =
        base.OnUpdateFrame(e)

        Update()


        let right = Vector3.Cross(eye, up).Normalized()

        if (this.IsKeyDown(Keys.W)) then
            position <- new Vector3(position.X + eye.X,
                                    position.Y + eye.Y,
                                    position.Z + eye.Z) * speed
            printfn "w"

        else if (this.IsKeyDown(Keys.S)) then 
            position <- new Vector3(position.X - eye.X,
                                    position.Y - eye.Y,
                                    position.Z - eye.Z) * speed
            printfn "s"
        else printfn ""

        if (this.IsKeyDown(Keys.A)) then 
            position <- new Vector3(position.X - right.X,
                                    position.Y - right.Y,
                                    position.Z - right.Z) * speed
            printfn "a"

        else if (this.IsKeyDown(Keys.D)) then 
            position <- new Vector3(position.X + right.X,
                                    position.Y + right.Y,
                                    position.Z + right.Z) * speed
            printfn "a" 
        else printfn ""

    override this.OnRenderFrame(e) =
        base.OnRenderFrame(e)
        GL.Clear(ClearBufferMask.ColorBufferBit)
        shader.Use()
        GL.BindVertexArray(vao)

        let projectionLoc = GL.GetUniformLocation(shader.program, "projection")
        let viewLoc = GL.GetUniformLocation(shader.program, "view")
        let modelLoc = GL.GetUniformLocation(shader.program, "model")

        GL.UniformMatrix4(projectionLoc, false, ref projection)
        GL.UniformMatrix4(viewLoc, false, ref view)
        GL.UniformMatrix4(modelLoc, false, ref model)
        GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length/2/3)

        this.SwapBuffers()

    override this.OnLoad() =
        base.OnLoad()

        Update()
        shader.CreateShader()

        GL.BindVertexArray(vao)
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo)
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * 4, vertices, BufferUsageHint.StaticDraw)

        GL.BindVertexArray(vao)

        // sizeof(float) in C# is 4, hense the mulitplication of 4
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * 4, 0)
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * 4, 3 * 4)
        GL.EnableVertexAttribArray(0)
        GL.EnableVertexAttribArray(1)

        GL.ClearColor(0.0f ,0.0f ,0.0f ,1.0f)

    member this.KeyDown(Key) =
        if this.IsKeyDown(Key) then true
        else false


[<EntryPoint>]
let main argv =

    let NWS = new NativeWindowSettings()

    NWS.Size <- new Vector2i(1000,720)
    NWS.Title <- "Window"
    NWS.StartFocused <- true
    NWS.StartVisible <- true
    NWS.APIVersion <- new Version(3,2)
    NWS.Flags <- ContextFlags.ForwardCompatible
    NWS.Profile <- ContextProfile.Core

    let wind = new Scene(GameWindowSettings.Default, NWS)
    wind.Run()
    0
