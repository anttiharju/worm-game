# Worm Blocks (2D game)
Educational project. It's more of a simulation you can interact with rather than a game.
- It's scalable, thousands of worms can be simulated on-screen (depending on hardware, settings and desired framerate).
- [Pooler.cs](WormGame/Pooling/Pooler.cs) has generics, [Blocks.cs](WormGame/Entities/Blocks.cs) BlockSpawner has recursion and there's also [tests.](WormGameTest/)
- See [CHANGELOG.md](CHANGELOG.md) for a more in-depth look on the development process.
- Over 2 500 lines of code (documentation included).
## Benchmarks
| 200x100 grid, worm length 6  | 3333 worms   | Filled with blocks |
|------------------------------|--------------|--------------------|
| i7-4790k and GTX 1080, 144Hz | 37,6 AVG FPS | 90,9 AVG FPS       |
| Ryzen 5 3550U, 60Hz          | 12,4 AVG FPS | 52,6 AVG FPS       |
## Video
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
