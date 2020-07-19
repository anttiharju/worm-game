# Worm Blocks (2d game)
Educational project. The point wasn't to make a good game, but learn git and practise C#. I tried to make it as scalable as possible so decent computers can handle several thousand worms. ~~Please see [concept.svg](https://raw.githubusercontent.com/anvemaha/worm-bricks/master/concept.svg) to understand the game.~~ <- outdated.
- Field
    - Fruits and worms spawn on it
- Worms
    - Wander around aimlessly on the field
    - Controlled like snake in the snake game
    - Grow longer by eating fruits
    - Can be transformed into blocks
- Blocks
    - Can't move
    - Disappear if in contact with another block of the same color.
- Players
    - Up to four
        - Controlled with gamepads
    - Can posess worms


# Tools
- Visual Studio 2019
- C# + [Otter](http://otter2d.com/)
- [ComTest](https://trac.cc.jyu.fi/projects/comtest/wiki/ComTestInEnglish) 