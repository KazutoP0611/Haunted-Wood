# Haunted Wood (2D Pixel Action Game)

## 🎥 Gameplay Video
[Watch Gameplay Video](https://www.youtube.com/watch?v=Fq_zkmkj1_s)

---

## 🌲 Haunted Wood
A 2D pixel-style action game built in Unity using Tilemap.  
This project was inspired by a childhood favorite, **[KND : Tummy Trouble](https://www.gameszap.com/game/11008/knd-tummy-trouble.html)**, and focuses on exploration, structured randomization systems, and reusable enemy architecture.

<img width="434" height="262" alt="image" src="https://github.com/user-attachments/assets/fceec7f8-cc29-4288-a12a-78c519014087" />
<br><br>

The game features a forest-themed world, bow-and-arrow combat, and progression built around discovering a hidden key to unlock the boss room.

<img width="427" height="240" alt="image" src="https://github.com/user-attachments/assets/3c71eaa2-7ae3-4c36-a841-1857c289f5f8" />
<img width="427" height="240" alt="image" src="https://github.com/user-attachments/assets/138daaa9-3d2e-4687-afa1-463959c18416" />

---

## ⚙️ Technical Highlights
- Engine: Unity 6 (6000.3.6f1)
- Programming Language: C#
- Unity Tilemap for level construction
- Object-Oriented enemy architecture
- Controlled random spawn system
- Precomputed item drop logic
- Map block positioning system
- Trigger-based and distance calculation block detection
- Boss cutscene implementation

---

## 🎮 Core Gameplay

### 🏹 Combat & Enemy System
- Bow-and-arrow ranged combat

  <img width="427" height="240" alt="image" src="https://github.com/user-attachments/assets/5e9d91e1-6886-4059-9732-ccb605027eed" />
  <br><br>
  
- Three enemy types:
  
  - **Skeleton Soldier**
  <br><img width="427" height="240" alt="image" src="https://github.com/user-attachments/assets/e37c29a2-4f54-4351-a7da-c38521b023ea" />

  - **Grim Reaper**
  <br><img width="427" height="240" alt="image" src="https://github.com/user-attachments/assets/36fa4dcb-98f9-4c5f-aad2-307e6b9d8da2" />

  - **Boss Enemy**
  <br><img width="427" height="240" alt="image" src="https://github.com/user-attachments/assets/5f52e5ff-a663-4d0d-ac24-c31fe9c39d6a" />

- All enemies inherit from a shared base class
- Differences handled through parameters, stats, and animations

This structure reduces duplication and allows scalable enemy expansion.

---

## 🎲 Randomized Spawn System

### 📦 Box Placement Logic
- Multiple predefined spawn points are placed across the map using `Transform` references.
- Each spawn point evaluates a random chance to spawn either:
  - A normal box
  - A boss key box 🔑

To prevent progression failure:
- The system checks whether the boss key box has been placed.
- If the final spawn point is reached and no boss key box has spawned, the system forces that location to spawn the boss key box.

This guarantees progression while maintaining randomness.

### 🎁 Item Drop System
- When a normal box is spawned, it pre-determines at game initialization whether it will drop a healing item.
- Drop chances are calculated once at startup rather than on impact.
- This allows:
  - Predictable debugging
  - Clear tracking of total healing items in a run
  - Stable runtime behavior

The boss key box always drops the key without fail.

<img width="427" height="240" alt="image" src="https://github.com/user-attachments/assets/c0915b8f-394e-45d0-ad09-b295eae1a63a" />

---

## 🗺 Map Block Transition System

Instead of moving the camera between areas, the map itself is repositioned dynamically.

<img width="427" height="240" alt="image" src="https://github.com/user-attachments/assets/226a8b45-e5aa-4f36-aa52-cb430287e024" />
<br>
<img width="427" height="348" alt="image" src="https://github.com/user-attachments/assets/a38f4683-d20c-43b8-ad43-731459a95ddf" />

### ⚙️ How It Works
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

## 🎬 Boss Encounter
- Entering the boss room triggers a cutscene.
- Boss entrance animation sequence before combat begins.
- Transition from exploration pacing to high-intensity encounter.

<img width="590" height="480" alt="image" src="https://github.com/user-attachments/assets/7db4932b-d691-421f-86c7-9816a3f5d7ae" />

---

## 🧠 Gameplay Focus
- Exploration and discovery
- Controlled randomness with guaranteed progression
- Reusable enemy architecture
- Structured 2D system design using Unity Tilemap

---

This project emphasizes modular system design, deterministic randomization safeguards, and structured world transition logic within a pixel-style 2D action game.
