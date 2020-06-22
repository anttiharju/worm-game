# TODO
- Fix GPU usage issue properly and not with RTSS

# 21.06.2020
- What got done from yesterdays notes:
    - Head.cs renamed to Worm.cs and it's no longer bloated.
    - Still no grid system but I really like the idea of infinite playspace although it seems to require smart design I'm not sure I'm capable of
    - We have a manager now! It's kind of bloated, but currently don't know how to divide it into nicer bits
        - Look into: does player, worm and manager handle stuff that only belongs to them?
    - Still using the same tweening, I don't want to mess with framerate stuff
        - Although body parts can no longer move diagonally (as much) thanks to Manager.WormUpdate()
    - We have a queuing system for controls! I tried to optimize it but also made it easier to debug by using chars instead of ints and it's probably not as lightweight as I would like it to be.
        - I know "premature optimization is root of all evil" but I was already having performance problems, although in hindsight were caused by the collision system
    - Haven't touched ghosts. They might turn out to be a can of worms performance-wise :s

- Notes for the future:
    - Current collision system is computationally too heavy. Sometimes worms don't execute all moves from their movement queue (controls).
    - The problem is that it's going through so many invidual objects, a few ideas to improve:
        - Divide the game area to some sort of chunks because an individual worm only really needs to be concerned about the worms nearby
            - I don't know how to do this efficiently (should research)
        - Use the collision system provided by Otter? Don't know if it really fits here
        - Make poolers so that we don't go through all pooled objects with foreach, but rather the ones that are enabled
            - this would involve using lists and don't know if that actually even improves things that much. It would be messier inside but probably nicer on the outside
        - Pooler for non-entity objects. Right now I'd need it for blocks.
- Not too happy with the way I have named the new things I've added, I'll think about them later on
- Now I'm going to clean up, document and add tests where it makes sense.
- Did some design work (in my head :s) on the blocks (see block.cs and leader.cs)
    - Idea was to use the same data structure as worms but I can use a better one if I can initialize the manager with a max worm length
    - Group of blocks would be called Bunch. Bunch can tell if the blocks can fall by one and just moves the blocks where they're needed.
    - Once the bunch has hit the bottom, it's no longer needed, BUT the blocks would stay on the game area
        - Putting a pin on this I need to figure out other stuff first
- Extreme Programming all the way lol (not sure if it makes sense here because I am the customer and the developer)
- My comments probably have a lot of spelling errors

# 20.06.2020
- Been doing miscallennous work, time for a more major rework
    - Head.cs has gotten bloated and takes care of a lot of stuff that really should be handled by some sort of a manager
    - Currently no grid system exists and the play area is basically infinite
        - Something has to be setup and that could be handled by the aformentioned manager
    - Some sort of global constants like framerate and grid size, worm/brick + grid size should also be done
        - It will get increasingly difficult to retrofit those even though I know they have to be done at some point
        - Instead of the current tweening for the worms bodyparts that allows worms to move diagonally when moving fast enough I think I could bake some sort of a curve to an array when initilazing the game if I know the framerate.
            - We're going to need some sort of queuing for the keystrokes so the game still feels responsive but we're also going to need a less frequent update loop (maybe achieved with coroutines in the WormGame.cs or Manager class?) where the positions are updated.
                - I'm kind of worried that the collisions system I currently have in place doesn't scale too well although I think I could perhaps optimize it by dividing it to some sort of chunks, but premature optimization is the root of all evil so I'll put a pin in that.
            - We'll worry about making things suitable for variable framerate later on
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