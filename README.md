# Worm bricks (2d game)
Educational project. The goal is to become a better developer by actually working on a project. Please see [concept.svg](https://raw.githubusercontent.com/anvemaha/worm-bricks/master/concept.svg) to understand the game.
- Play area
    - Like in Tetris, maybe wider.
    - Fruits spawn on the grid.
- Worms
    - Pop in on the upper half of the grid.
    - Controlled like snake in the snake game.
    - Grow longer by eating fruits.
    - Can be transformed into bricks.
- Bricks
    - Controlled like tetrominoes in Tetris.
    - Full horizontal rows in the play area explode, increasing score.
- Ghosts
    - Controlled by players.
    - No collision (free movement).
    - Can posess worms.
    - Can't eat fruits.
    - Multiple players.

# Tools
- Visual Studio 2019
- C# + [Otter](http://otter2d.com/)
- [ComTest](https://trac.cc.jyu.fi/projects/comtest/wiki/ComTestInEnglish) 

# Issues
- High GPU usage (50-100%) even on a GTX 1080 causing coil whine, unless limited with other software like [RTSS.](https://www.guru3d.com/files-details/rtss-rivatuner-statistics-server-download.html) Probably caused by a bug in Otter, but I haven't investigated it that much.
