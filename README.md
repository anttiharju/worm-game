# Worm Blocks (2D game)
Educational project. It's more of a simulation you can interact with rather than a game. The point was to learn Git and practise C#.
- The main development goal was scalability: i7-4790k + GTX 1080 can simulate thousands of worms *on-screen* at 60fps.
    - Scale and performance depend a lot on configuration: what gamerules are enabled, field dimensions, worm length, etc.
- Pooling system should be relatively easy to modify for use with Unity3d.
- Please have a look at [CHANGELOG.md.](CHANGELOG.md)
- TODO: youtube video here

## Game mechanics
- Field
    - Fruits and worms spawn on it
- Players
    - Can posess worms
    - Up to five (4x gamepad + keyboard)
        - Gamepad: left stick to move, right bumper to posess and join
        - Keyboard: WASD to move, space to posess and join
- Worms
    - Go straight until they hit something
    - Controlled like snake in the snake game
    - Grow longer by eating fruits
    - Turn into blocks when stuck
- Blocks
    - Can't move
    - Disappear if in next to another block of the same color.
   
# Setup
- Follow the guide on [otter2d.com](http://otter2d.com/example.php?p=3).
- Project uses Otter 1.0.0

# Tools
- C# + [Otter](http://otter2d.com/)
- Visual Studio Community 2019
- [ComTest](https://trac.cc.jyu.fi/projects/comtest/wiki/ComTestInEnglish) (not required to run tests)