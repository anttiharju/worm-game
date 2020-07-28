# Worm Blocks (2d game)
Educational project. The point wasn't to make a good game, but to learn Git and practise C#.
- One of the development goals was to make the game scalable: on a i7-4790k the game can handle a few thousand worms simultaneously.
- Pooling system should be relatively easy to modify for use with Unity3d.
- Please have a look at [CHANGELOG.md.](CHANGELOG.md)

## Game mechanics
- Field
    - Fruits and worms spawn on it
- Players
    - Can posess worms
    - Up to five (4x gamepad + keyboard)
        - Gamepad: left stick and right bumper
        - Keyboard: arrow keys and space bar
- Worms
    - Go straight until they hit something
    - Controlled like snake in the snake game
    - Grow longer by eating fruits
    - Turn into blocks when stuck
- Blocks
    - Can't move
    - Disappear if in next to another block of the same color.
    
# Tools
- Visual Studio Community 2019
- C# + [Otter](http://otter2d.com/)
- [ComTest](https://trac.cc.jyu.fi/projects/comtest/wiki/ComTestInEnglish)
    - Not required to run tests