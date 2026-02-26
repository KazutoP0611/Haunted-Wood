# Haunted Wood (2D Pixel Action Game)

#### 🌲 Haunted Wood
A 2D pixel-style action game built in Unity using Tilemap.  
This project was inspired by a childhood favorite, **KND : Tummy Trouble**, and focuses on exploration, structured randomization systems, and reusable enemy architecture.

The game features a forest-themed world, bow-and-arrow combat, and progression built around discovering a hidden key to unlock the boss room.

<img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/3c71eaa2-7ae3-4c36-a841-1857c289f5f8" />
<img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/138daaa9-3d2e-4687-afa1-463959c18416" />

---

#### ⚙️ Technical Highlights
- Engine: Unity 6 (6000.0.3f1)
- Programming Language: C#
- Unity Tilemap for level construction
- Object-Oriented enemy architecture
- Controlled random spawn system
- Precomputed item drop logic
- Map block positioning system
- Trigger-based and distnace calculoation block detection
- Boss cutscene implementation

---

#### 🎥 Gameplay Video
[Watch Gameplay Video](https://www.youtube.com/watch?v=Fq_zkmkj1_s)

---

#### 🎮 Core Gameplay

##### 🏹 Combat & Enemy System
- Bow-and-arrow ranged combat
- Three enemy types:
  
  - **Skeleton Soldier**
    <img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/e37c29a2-4f54-4351-a7da-c38521b023ea" />

  - **Grim Reaper**
    <img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/36fa4dcb-98f9-4c5f-aad2-307e6b9d8da2" />

  - **Boss Enemy**
    <img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/5f52e5ff-a663-4d0d-ac24-c31fe9c39d6a" />

- All enemies inherit from a shared base class
- Differences handled through parameters, stats, and animations

This structure reduces duplication and allows scalable enemy expansion.

#### 🎲 Randomized Spawn System

##### 📦 Box Placement Logic
- Multiple predefined spawn points are placed across the map using `Transform` references.
- Each spawn point evaluates a random chance to spawn either:
  - A normal box
  - A boss key box 🔑

To prevent progression failure:
- The system checks whether the boss key box has been placed.
- If the final spawn point is reached and no boss key box has spawned, the system forces that location to spawn the boss key box.

This guarantees progression while maintaining randomness.

##### 🎁 Item Drop System
- When a normal box is spawned, it pre-determines at game initialization whether it will drop a healing item.
- Drop chances are calculated once at startup rather than on impact.
- This allows:
  - Predictable debugging
  - Clear tracking of total healing items in a run
  - Stable runtime behavior

The boss key box always drops the key without fail.

---

#### 🗺 Map Block Transition System

Instead of moving the camera between areas, the map itself is repositioned dynamically.

<img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/226a8b45-e5aa-4f36-aa52-cb430287e024" />

##### ⚙️ How It Works
- The world is divided into area blocks.
- Each area component initializes itself with a manager at game start.
- When the player enters a new area (`OnTriggerEnter`), a distance threshold check is performed before triggering the transition callback.
- The manager calculates the target position using the area's predefined offset.
- The map object is then repositioned accordingly.

This approach:
- Simplifies camera logic
- Maintains controlled transitions
- Keeps the world modular and scalable

---

#### 🎬 Boss Encounter
- Entering the boss room triggers a cutscene.
- Boss entrance animation sequence before combat begins.
- Transition from exploration pacing to high-intensity encounter.

  <img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/d8dcd48c-a8f4-44d0-95af-7a137f3fe835" />

---

#### 🧠 Gameplay Focus
- Exploration and discovery
- Controlled randomness with guaranteed progression
- Reusable enemy architecture
- Structured 2D system design using Unity Tilemap

---

This project emphasizes modular system design, deterministic randomization safeguards, and structured world transition logic within a pixel-style 2D action game.
