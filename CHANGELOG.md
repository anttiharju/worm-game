# TODO
- Fix GPU usage issue properly and not with RTSS
- Entity pooling system
- Make game speed independent from frame rate (30fps as "ground truth", scales nicely to 60, 120)
- Scaling to multiple resolutions, maybe 1280x720 as minimum requirement

# Ideas to juice the game up
- Low internal rendering resolution (think nidhogg, towerfall ascension)
- Tweening (worm movement)

# 12.06.2020
- Basic worm (snake) class done
- Different scenes are in different files 

# 11.06.2020
- Visual Studio, Otter, ComTest setup done
- MIT license
- Otter\Utility\Debugger.cs line 128 changed Key.Tilde -> Key.Home so I can use the Otter in-game debug console (finnish keyboards don't really have a tilde)
- Known issue: nearly 100% GPU usage, as a temp fix I'll limit the framerate with RTSS