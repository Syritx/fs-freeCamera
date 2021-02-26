module _Camera

open System
open OpenTK.Mathematics;
open OpenTK.Windowing.Common;
open OpenTK.Windowing.Desktop;
open OpenTK.Windowing.GraphicsLibraryFramework;


let mutable position = new Vector3(0.0f,0.0f,0.0f)
let mutable eye = new Vector3(0.0f,0.0f,-1.0f)
let mutable up = new Vector3(0.0f,1.0f,0.0f)

let mutable projection: Matrix4 = Matrix4.Identity
let mutable view: Matrix4 = Matrix4.Identity
let mutable model: Matrix4 = Matrix4.Identity

let mutable xRotation = 1.0
let mutable yRotation = 1.0
let mutable canRotate = false
let mutable lastMousePosition = new Vector2(0.0f,0.0f)
let mutable speed = 1.0f

let Clamp(value, minv, maxv) =
    if value < minv then minv
    else if value > maxv then maxv
    else value

let Update() =
    
    xRotation <- Clamp(xRotation, -89.0, 89.0)

    printfn "%f" xRotation

    eye.X <- (float32 (Math.Cos(MathHelper.DegreesToRadians(xRotation)))) * (float32 (Math.Cos(MathHelper.DegreesToRadians(yRotation))))
    eye.Y <- (float32 (Math.Sin(MathHelper.DegreesToRadians(xRotation))))
    eye.Z <- (float32 (Math.Cos(MathHelper.DegreesToRadians(xRotation)))) * (float32 (Math.Sin(MathHelper.DegreesToRadians(yRotation))))
    eye <- Vector3.Normalize(eye)
    
    projection <- Matrix4.CreatePerspectiveFieldOfView(1.39626f, 1.0f, 0.01f, 2000.0f)
    view <- Matrix4.LookAt(position, new Vector3(position.X + eye.X,
                                                 position.Y + eye.Y,
                                                 position.Z + eye.Z), up)
    

    