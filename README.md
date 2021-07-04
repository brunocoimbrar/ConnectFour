# ConnectFour

![LineCoverageBadge](Docs/badge_linecoverage.svg)

[Try the WebGL demo here.](https://coimbrastudios.github.io/ConnectFour/)

## Architecture

![MainArchitecture](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/brunocoimbrar/ConnectFour/main/Docs/MainArchitecture.plantUML)

- `World`: Main scene script to coordinate the work of the systems. Can be initialized by code to allow testing.
- `System`: Used to split the World logic. They can be tested in an isolated context.
- `Object`: Defines a GameObject's main class as we can't extend GameObject.
- `Interfaces`: are used to have a clear difference between what should only be managed by the World/System from what can be accessed from any place.

## Third-Party

Link: https://github.com/greggman/better-unity-webgl-template

Reason: Allow the game to fully expand to the available screen-space.
