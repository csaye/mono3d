# MonoGame 3D

A 3D raycasting engine built in MonoGame.

![Grid GIF](https://user-images.githubusercontent.com/27871609/114318182-18328480-9ac9-11eb-9f59-6159689adf67.gif)

## Controls

W/S: move forwards/backwards\
A/D: move left/right\
Space/LShift: move up/down\
Arrow keys: look up/down/left/right

## Installation/Running

Ensure [Visual Studio](https://visualstudio.microsoft.com/downloads/) and [MonoGame](https://www.monogame.net/downloads/) are installed.

Clone the project by running `git clone https://github.com/csaye/monogame-3d`

Open in Visual Studio and press the big play button or use F5 to run.

## Settings

[Drawing.cs](Mono3D/Drawing.cs)
```cs
public const int Grid = 4; // Size of grid
public const int GridWidth = 128; // Width of grid
public const int GridHeight = 128; // Height of grid
```

[Map.cs](Mono3D/Map.cs)
```cs
private const int Width = 32; // Width of map (x)
private const int Height = 16; // Height of map (y)
private const int Length = 32; // Length of map (z)

private const bool ShowSky = false; // Whether sky is shown
private const bool ShowColors = false; // Whether colors are shown

private const int SmoothingIters = 3; // Noise smoothing iterations
private const float SmoothingFactor = 0.7f; // Noise smoothing factor

private const float RayStepDist = 0.2f; // Distance between ray steps
private const float MaxDepth = 32; // Maximum ray depth
```

[Player.cs](Mono3D/Player.cs)
```cs
private const float Speed = 3; // Player movement base speed
private const float Spin = 1; // Player rotation base speed
private const float BaseFov = Pi / 4; // Base field of view
```
