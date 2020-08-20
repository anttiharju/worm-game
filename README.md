# Worm Blocks (2D game)
Educational project. It's more of a simulation you can interact with rather than a game. The project had three goals: scalability, learn git by using it frequently and practise C#.
- Scalable, thousands of worms can be simulated on-screen (depending on hardware, settings and desired framerate).
- Benchmarks (200x100 grid, worm length 6, fruits off for worms and on for blocks):
    - i7-4790k and GTX 1080, 144Hz: 3333 simultaneous worms 37,6 AVG FPS, all worms turned into blocks: 90,9 AVG FPS
    - Surface Laptop 3, 60Hz: 3333 simultaneous worms 12,4 AVG FPS, all worms turned into blocks: 52,6 AVG FPS
        - If played as a game the field wouldn't be anywhere near as large and the worst case scenario of thousands of worms would never happen.
- [Pooler.cs](WormGame/Pooling/Pooler.cs) has generics, [Blocks.cs](WormGame/Entities/Blocks.cs) BlockSpawner has recursion and there's also [tests.](WormGameTest/)
- See [CHANGELOG.md](CHANGELOG.md) for a more in-depth look on the development process.
- Over 2 500 lines of code (documentation included).

[![YouTube video](https://img.youtube.com/vi/QqxTP1VZjGs/0.jpg)](https://www.youtube.com/watch?v=QqxTP1VZjGs "Worm Blocks v0.5")

# Game mechanics
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