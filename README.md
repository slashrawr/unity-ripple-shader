# unity-ripple-shader

This is a Unity Project with assets for a ripple shader. It contains the shader itself (HLSL) and a script (C#) for using on the object to create ripples on.

The default scene contains a dynamically generated mesh with configurable LOD and a particle system. Because the shader modifies individual vertices, the higher LOD the mesh has the better the ripple will look.

The CollisionScript is responsible for receiving the collisions and passing those points to the shader. The shader then manipulates the verticies based on their distance from the collision adjusting the normals accordingly and taking (basic) lighting into account.

<br/>

# How to Use

- The ripples are based on collisions so a collider is necessary on the object being rippled.
- Attach the CollisionScript to the object being rippled.
- Create a material specifying the ripple shader as the shader.
- The following settings can be configured (default values are recommended):
<br/>
<table>
    <tr>
        <td>Setting</td>
        <td>Description</td>
        <td>Location</td>
        <td>Default Value</td>
    </tr>
    <tr>
        <td>Initial Ripple Size</td>
        <td>The initial size or amplitude of the ripple.</td>
        <td>Script</td>
        <td>0.7</td>
    </tr>
    <tr>
        <td>Scale</td>
        <td>The overall size or scale of the ripple.</td>
        <td>Shader</td>
        <td>0.5</td>
    </tr>
    <tr>
        <td>Speed</td>
        <td>The effective animation speed of the ripple. Use to emulate density of the fluid.</td>
        <td>Shader</td>
        <td>0.3</td>
    </tr>
    <tr>
        <td>Frequency</td>
        <td>The number of times the ripple rebounds.</td>
        <td>Shader</td>
        <td>8</td>
    </tr>
    <tr>
        <td>RippleDiameter</td>
        <td>The max diameter of the ripple and number of ripples per collision.</td>
        <td>Shader</td>
        <td>3</td>
    </tr>
    <tr>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>
</table>

<br/>

# Features

- Supports the Universal Render Pipeline (URP).
- Configurable size, speed, frequency, magnitude, falloff.
- Supprots a single texture and diffuse color.
- Supports lighting.
- Includes a dynamic plane generator with configurable LOD.

<br/>

# Notes

- Because the shader acts on mesh verticies, the quality of the ripple will depend largely on the detail of the mesh - the more verticies, the better the ripple.
- Only works with global directional light at the moment.
- Ripples are always perpendicular to the surface.
- Due to limitations of HLSL the number of ripples has to be defined at compile-time. It's currently limited to 100 simulataneous ripples.
- The mesh generator script is purely to aid prototyping this shader. I will build a seperate project for it that will be comprehensive and complete.

<br/>

# Future Features

- Upgrade project to latest version of Unity.
- Handle multiple lights including point lights.
- Handle direction better.
- Make intial ripple size dynamic based on force of collision and size of coliding object.
- Tests and more debug features.
- Better comments.
- Caustics maybe?
- Animated textues.
- Work out some way to make the number of ripples truly dynamic. It's partly there but needs more work.

<br/>

# Links

[Repo](https://github.com/slashrawr/unity-ripple-shader) | [Author](https://github.com/slashrawr)