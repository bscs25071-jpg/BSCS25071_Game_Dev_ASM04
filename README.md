# Arena Collector — ITU CS-464 Assignment 04

- **Student Roll No:** BSCS25071
- **Course:** CS-464 Game Development (ITU)
- **Engine:** Unity 6 LTS (Universal Render Pipeline)
- **Gameplay Video (90–180s):** https://drive.google.com/file/d/13f-JD5xygO2LP8OQKrsmoGujbVD3755z/view?usp=sharing
- https://drive.google.com/file/d/13f-JD5xygO2LP8OQKrsmoGujbVD3755z/view?usp=sharing

---

## 1. Game Overview

**Arena Collector** is a 3D top-down arena arcade game where the player navigates five progressively challenging levels, collects coins to reach the target score, avoids spatial hazards, and tracks their fastest completion times.
## 2. Audio & Project Settings Table

| Setting / Parameter | Value / Configuration | Purpose |
| :--- | :--- | :--- |
| **Game Title (Product Name)** | `Arena Collector` | Configured in Project Settings and Main Menu / Credits |
| **Reference Resolution** | `1920 x 1080` (Scale With Screen Size) | Consistent HUD and Menu scaling across 16:9 and 4:3 |
| **AudioMixer Groups** | `Master` → `Music`, `SFX` | Independent volume control for BGM and sound effects |
| **Exposed Mixer Parameters** | `MusicVolume`, `SfxVolume` | Controlled via logarithmic dB sliders (`Mathf.Log10(v) * 20f`) |
| **Default Volume Levels** | `0.8` (80%) | Saved and restored via `PlayerPrefs` (`SaveData`) |
| **SFX Pitch Randomization** | `±2` semitones (`Mathf.Pow(2f, s / 12f)`) | Prevents repetitive audio fatigue during rapid coin pickups |
| **Active AudioListeners** | `1` (`Main Camera` only) | Ensures clean 3D spatialization with zero duplicate warnings |
| **Build Scene Order** | `MainMenu`, `Level01`–`Level05`, `Credits`, `Loading` | Sequential progression with async loading bar and Credits wrap |
## 3. 3D Positional Audio Configuration & Rationale
Two continuous 3D sound sources (`Spatial Blend = 1.0`) are placed in the arena to provide spatial awareness:
1. **Hazard (`Logarithmic Rolloff`, Min Distance: `2`, Max Distance: `12`)**
   - **Reason:** Logarithmic rolloff keeps the warning hum subtle at a distance while ramping up sharply as the player gets dangerously close, giving an urgent proximity cue without cluttering the entire map.
2. **ArenaBeacon (`Linear Rolloff`, Min Distance: `3`, Max Distance: `18`)**
   - **Reason:** Linear rolloff with a wider `18`-unit range provides a smooth, predictable directional landmark across the arena so the player can orient themselves by sound.
## 4. Design & Implementation Questions
### Q1: Why convert linear slider values (`0.0001` to `1.0`) to decibels (`Mathf.Log10(value) * 20f`) for the AudioMixer?
Human hearing perceives loudness logarithmically rather than linearly. Passing a raw linear slider value directly into an AudioMixer causes almost all audible change to happen at the very bottom of the slider. Converting with `Mathf.Log10(value) * 20f` maps the slider smoothly from `-80 dB` (silent) to `0 dB` (full volume) so every movement of the slider feels natural to the player.
### Q2: Why decouple damage feedback using a C# event  instead of calling UI, audio, and particles directly inside `Health.cs`?
Using a C# event keeps `Health.cs` independent of specific UI panels, audio clips, or particle prefabs. Multiple listeners—such as the HUD health bar, hit particle burst, damage sound effect, and camera shake—subscribe in `OnEnable()` and unsubscribe in `OnDisable()`, making the code modular and preventing memory leaks or null reference errors when objects are destroyed.
### Q3: How does the scene flow prevent duplicate background music and support smooth transitions across all levels?
`LevelLoader` routes transitions through an asynchronous `Loading` scene (`SceneManager.LoadSceneAsync`) with a live progress bar, advancing sequentially from `Level01` through `Level05`, opening `Credits` after the final level, and wrapping back to `MainMenu`. `AudioManager` uses a Singleton pattern with `DontDestroyOnLoad`, destroying any duplicate instance on scene load so a single `PersistentMusic` source plays uninterrupted.

### Q4: How are player progress and best completion times persisted across sessions?
`SaveData` wraps `PlayerPrefs` with safe default fallbacks (`UnlockedLevel = 1`, `BestTime_1` to `BestTime_5 = 0`, and audio volumes at `0.8f`). When a level is completed, `GameManager` records the elapsed time if it beats the previous record and unlocks the next level. `LevelSelectUI` reads this data in `OnEnable()` to disable locked buttons and display each unlocked level's best time.
## 5. Credits & Attributions

- **Developer:** BSCS25071 (ITU CS-464)
- **Game Engine & Rendering:** Unity 6 LTS 
- **UI & Typography:** Unity TextMeshPro
- **3D & UI Assets:** Synty / Kenney Standard Game Assets & Custom Particle Prefabs
- **Audio:** Custom / Royalty-Free SFX & Background Music Loops
