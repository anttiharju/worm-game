# TODO
- Investigate GPU usage issue

# 01.07.2020
- Trying to do less massive commits, getting into the good habit of small commits.
    - Pretty sure my professor said during a lecture that small commits are a good practise.
- Improve collision visualizer
    - Use Console.Top and Console.Left to avoid freeze from Console.Clear()
- Add indexing to worms
    - I can now refer to worms body parts with an indexer, achieving cleaner code
- Fix collision system inaccuracy
    - I was using current position (tweened) instead of target position
- Rename project folder
    - Delete old project folder
        - I had to do a separate commit because I forgot to delete it lol
    - I just wanted to get rid of WBGame because it reminds me of Warner Bros and rocksteadys Arkham games
        - It's probably not a good practise rename entire folders, but at least I know to pick better ones from the start in future projects
- Delete CONTEXT.md
    - Changelog is all I need, plus I have some designs on actual papers.
- Fix worm indexer
    - I had confused enumerator and indexer, now I know which is which.
- Divide Helper to Mathf and Random
    - To make the code more readable
    - Also fixed FastAbs()
- Update documentation
    - Goal is to fix any known bugs and merge to master sometime soon.
    - I've noticed I've (by accident) pushed non-compiling code into the dev branch.
        - Just a note to myself to be more careful
- Cleanup **MERGE TO MASTER**
    - Woo! Finally documented all the code and merged it to master. TOMORROW I CAN FINALLY START WORKING ON NEW STUFF!
- Add tests to Mathf

# 30.06.2020
- Cleaned up the new collision system, it still has bugs but it shouldn't crash.
    - I'm going to commit now because I have other things that need my attention and the commit is probably already way too massive
        - I should get into the habit of smaller commits, as afaik that's the best practise and especially important when working with a team


# 29.06.2020
- ~~Interestingly 100% GPU usage (coil whine) without RTSS is no longer an issue. Not sure what caused it or what fixed it.~~
    - I guess now that the game has something to calculate it's not running insanely fast.
    - I should try to reproduce it and open an issue on the Otter2d repo?
    - I'll keep task manager open just in case
        - **GPU usage still is around 50 % which still is way too high, I enabled RTSS again and the usage is now below 10% and no coil whine.**
- I think I finally figured out how to move entities at the a constant speed no matter the framerate
    - Thank you Game.MeasureTimeInFrames = false;
- ~~Began work on ~~Field~~ Chunks (I would've called it Grid but Otter already has something called Grid)~~
    - ~~I'd also like to note that I'd prefer the name PlayArea, but it's too long and Area isn't nice.~~
    - ~~It just made sense to combine Field and ChunkManager~~
    - This became the new collision system, see below.
- Updated Pooler documentation
    - Pooler got updated yesterday; it now uses defragmenter and should be way more performant at large scales
- Deleted CONTROLS.md as they're now documented well enough in the Player class
- Renamed PLANNING.md to CONTEXT.md that I'll use as a brain dump from time to time
- Renamed methods in Worm so that they better reflect what they do
- Merged Manager to WormScene
- **New collision system**
    - Ditch multiple small chunks in favor of one big chunk that I'll refer to from now on as field.
    - I'll treat null values as empty, let's see if I'll regret this
    - The system has to constantly convert floats to ints and it hurts just a little bit, but it's probably fine.
        - I tried approach where Poolables had extra fields called GridX and GridY which would've been the worms position on the grid as ints and I would convert ints to floats when updating the position, but everything got really messy and confusing.

# 28.06.2020
- Thoughts:
    - I've been working on an entity based tetris blocks and while I probably could make it work, I'm worried it won't be very performant..
    - Took a closer look at Otter examples and surprisingly it has tilemaps which are exactly what I wanted.
        - I could make the tetris part of the game use tilemaps and probably most of the difficult code I written probably would work with minimal changes, I don't know.
        - I mean if I'm moving to tilemaps, why not also use it for worms?
        - I'd like to stick with the entity based approach I've chosen just for the sake of exercise and learning why it might be a bad idea.
        - Tilemap would solve all of my collision issues and I don't really have a use for infinite playfield that the entity based approach allows.
            - I should ditch the current (very basic) collision system in favor of chunk based approach.
                - One chunk would probably be like 8*8, every chunk would know its neighbours and the entities it contains
                    - It would probably have an array of poolables or I remember reading that Notch really regretted minecraft treating air blocks as null values maybe rather an int array and the integers in that would represent indexes in the chunks Poolable array
                    - I will probably get rid of tweening for the worm body parts to keep things simple.
                    - Player ghosts would operate outside of chunk system because they don't have collision.
                        - But they will still know the chunk they're in so they can posess worms efficiently
                    - Chunk system will probably have its own manager just to keep the current manager from getting bloated.
                    - I think worms will still stay intact as is and the chunk system will hook into from where collision is currently done.
                        - Although worm (head) has to know the chunk it is in to access the collision system but otherwise nothing
                - It will probably have some overhead, but will scale WAY better than the current system.
                    - And scalability is what I want. It's way more impressive to show A TON OF WORMS on the screen simultaneously rather than just a few with crappy controls
    - I should keep changelog more up-to-date with my changes, I've implemented lot of stuff I haven't really mentioned here and it's not a good look.
    - Before I can really start working on chunk collision I'll have to implement some kind of grid helper so I don't always have to do the math everywhere when I want to put something on the 'grid'
    
- TL;DR: No to tilemaps, embrace entities, pursue scalability with a chunk based collision system


# 27.06.2020
- I'm facing a problem:
    - The new pooling system although is very nice to access elsewhere, has all kinds of nasty side effects. I need back the poolables because entities have to be able to disable themselves because otherwise I'm going to have to compensate WAY TOO MUCH for that in other ways.
    - I'm going to remove lot of the documentation and go through everything step by step and make sure the foundation is solid for future work.
    - I'm dying to work on the teris part of the game
        - I'll probably do the special case for blocks / bunches in the pooler because that's just a more solid foundation
    - I create a new Vector2 array inside Blockify, I'll get rid of it.
    - Manager is kind of bloated?
    - Pooler can't have Poolables accessible via Indexer because it's hacky.
        - I'm going to have to implement ghosts avoid using Indexer


# 26.06.2020
- Okay so, starting to work on the tetris part of the game and some thoughts:
    - Because I really want to use pooling so I can avoid any lag spikes at runtime (new is the enemy), I'm kind of locked into using recursion with blocks. With worms it made sense, but with blocks it feels hacky.
        - I could build some special case into the pooler but that kind of breaks the generic nature of the pooler.
        - I'll try doing things the recursive way.


# 24.06.2020
- Pooler now uses two Lists instead of single array.
    - Now we don't have to if(poolable.enabled) at every point
    - Code might be messy right now
- Removed Poolables (now we just pool Entities)

- Plans for the next coding session: start implementing block rotation / controls, see if any problems arise with the current queuing system.


# 22.06.2020
- Solved the movement queue problem, I was using current position instead of target position when calculating the next target position for the worm


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