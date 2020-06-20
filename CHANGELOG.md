# TODO
- Fix GPU usage issue properly and not with RTSS

# 20.05.2020
- Been doing miscallennous work, time for a more major rework
    - Head.cs has gotten bloated and takes care of a lot of stuff that really should be handled by some sort of a manager
    - Currently no grid system exists and the play area is basically infinite
        - Something has to be setup and that could be handled by the aformentioned manager
    - Some sort of global constants like framerate and grid size, worm/brick + grid size should also be done
        - It will get increasingly difficult to retrofit those even though I know they have to be done at some point
        - Instead of the current tweening for the worms bodyparts that allows worms to move diagonally when moving fast enough I think I could bake some sort of a curve to an array when initilazing the game if I know the framerate.
            - We're going to need some sort of queuing for the keystrokes so the game still feels responsive but we're also going to need a less frequent update loop (maybe achieved with coroutines in the WormGame.cs or Manager class?) where the positions are updated.
                - I'm kind of worried that the collisions system I currently have in place doesn't scale too well although I think I could perhaps optimize it by dividing it to some sort of chunks, but premature optimization is the root of all evil so I'll put a pin in that.
    - I'm pretty happy with the pooling system. Even though the line to spawn something is a bit complicated, I like it.
        - We can simplify spawning by having a separate method for it in the manager class (that's how you're supposed to do it with OOP?)
    - At some point I'm going to have to worry about how to implement the ghosts, but that seems easy enough to retrofit so I'll push that aside for now
- Apparently I'm now using changelog as some sort of a blog. Deal with it.

# 15.06.2020
- Designing new systems (collision, movement) and how they interact
- Improved pooling system

# 14.06.2020
- Basic generic entity pooling done
- Worms now have their body parts in pools
- **NOTE**: I know I'm probably using too much inheritance and the smart way to do things would be the component system that Otter has. I checked it out and it seems awesome, BUT I'm going to go down the inheritance rabbit hole to see how much problems it's going to cause. Kind of like how I learned that rewriting git history in a branch already pushed to remote is a really bad idea by having tried it. **The goal is to become a better programmer and not to make a game.**

# 12.06.2020
- Basic WormGame done
- Different scenes are in different files 
- Began work on AreaGame (Grid) and Pooling
    - **NOTE:** The names I pick for things seem to often overlap with some premade Otter stuff. Probably if I just wanted to make a game I'd have to do much less work, but the point is not to learn Otter but rather learn keyboard shortcuts (web browser, windows, visual studio), git commands / github, markdown, windows terminal and get some practise in OOP.

# 11.06.2020
- Visual Studio, Otter, ComTest setup done
- MIT license
- Otter\Utility\Debugger.cs line 128 changed Key.Tilde -> Key.Home so I can use the Otter in-game debug console (finnish keyboards don't really have a tilde)
- Known issue: nearly 100% GPU usage, as a temp fix I'll limit the framerate with RTSS