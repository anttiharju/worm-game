# Managers

## Manager
- Manager of Managers. All other managers are accessed through here.

## PoolerManager
- Contains Spawn() functions and everything related to spawning

## ChunkManager
- Things related to chunks and collision


# Ideas
- Optimize Poolers enabling in a way that we can defraq the pool from time to time (for example, if we know that we aren't going to spawn anything for awhile, although I haven't actually done any async stuff) and it know 'a safe start point' from where to instantly enable new entities instead of going through the pool every god damn time. (expensive as hell once we start having tens of thousands of entities in pools)
    - Goal is to have 160 * 90 * 2 = 28 800, rounded up ALMOST 30 000 ENTITIES IN POOLS and we can't have acceptable performance by foreaching through it every time
    - Do more planning on paper about the details
    - Premature optimization is the root of all evil but we're pursuing scale and this will be helpful even if the scale isn't achieved
- Disabling is already cheap but it's what causes pool fragmentation

# Coding plan (rough)
- Pooler optimization
- Grid system
- Chunk collision