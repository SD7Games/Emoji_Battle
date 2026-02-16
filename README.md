# Emoji Battle

A **shipped mobile puzzle-battle game** built in **Unity 6**.  
Inspired by classic tic-tac-toe and expanded with emoji customization, AI opponents, basic progression, and a mobile-first UI.

📱 **Released on Google Play**  
🔗 [Open on Google Play](https://play.google.com/store/apps/details?id=com.sd7gamestudio.emojibattle)

---

## ✅ Project Status

**Emoji Battle is a completed and released project.**  
The current version represents a **finished gameplay feature set** and a **production-ready architecture**.

The project is no longer under active feature development and is preserved as a **reference implementation** of clean gameplay architecture, SRP, and MVC-style separation in Unity.

---

## 🧱 Architecture & Refactor Summary

The project was **fully refactored** with a strong focus on:
- **MVC-style structure**
- **Single Responsibility Principle (SRP)**
- Clear separation between **game logic**, **UI**, and **infrastructure**
- Reduced coupling and removal of god-objects

This refactor reflects my current approach to maintainable gameplay systems in Unity.

---

## 🎮 Implemented Features

- **Custom Emoji System**  
  9 colours × 87 emojis, rarity levels, unlock rules driven by ScriptableObjects.
- **AI Opponents (Strategy + Factory)**  
  Easy / Normal / Hard AI via `IAIStrategy` and factory selection.  
  New difficulty levels can be added without modifying existing logic.
- **Core Game Logic (UI-independent domain)**  
  Turn handling, win/draw detection, and board state logic fully separated from UI.
- **Player Progression & Saving**  
  Unlock and progression system persisted using **JSON / PlayerPrefs**.
- **Mobile-Friendly UI**  
  Lobby, emoji selection, popups, and animations with no gameplay rules inside UI.
- **Clean Architecture Refactor**  
  MVC-style separation, SRP across systems, and explicit dependency direction.
- **Mobile Optimization**  
  Lightweight assets, controlled allocations, no unnecessary runtime allocations.

---

## 🧠 Tech & Architecture

- **Unity 6**, **C#**
- **MVC-style architecture** (Model / View / Controller)
- **SRP** across gameplay, AI, and UI
- **Strategy + Factory patterns** for AI behaviour
- **ScriptableObjects** for configuration and emoji data
- **Event-based communication** between systems
- **Domain logic independent from UI**
- Coroutines for timing and animations
- Player progress stored via JSON / PlayerPrefs
- Shader Graph for basic UI and VFX effects

---

## 🧩 Architecture Overview

### Model (Domain / Data)
- Board state and win/draw evaluation  
- AI decision logic (strategies)  
- Progression and persistence models  

### View (Presentation)
- UI screens, widgets, visual states, animations  
- No gameplay rules inside UI components  

### Controller (Application Flow)
- Scene bootstrap and initialization  
- Game flow orchestration  
- Connects Domain ↔ Presentation  

> **Key principle:** gameplay logic does not depend on UI, and UI components do not contain game rules.

---

## 📂 Project Structure

```text
Assets/
  Scripts/
    App/             # Entry points, bootstrap, scene flow
    Domain/          # Pure game logic (board, AI, progression)
    Infrastructure/  # Data models, saving, profiles
    Presentation/    # UI views and visual logic
  Sprites/
  Prefabs/
```

---

## 🚀 Build & Run

1. Install **Unity 6** (latest available version).
2. Clone the repository:
   ```bash
   git clone https://github.com/SD7games/Emoji_Battle.git
   ```
3. Open the project via Unity Hub.
4. Main scenes:
   - `Lobby`
   - `Game`
5. Platforms:
   - **Android** (released)
   - WebGL (experimental)
   - iOS (not published)

---

## 📸 Screenshots

<p align="center">
  <img src="BootstrapScene.png" width="260" />
</p>

<p align="center">
  <img src="LobbyScene.png" width="260" />
  <img src="MainScene.png" width="260" />
</p>

<p align="center">
  <b>Splash → Loading → Lobby → Gameplay</b>
</p>

---

## 🎯 Purpose of This Project

This project serves as:
- a **shipped mobile game**
- a **reference example of clean Unity gameplay architecture**
- a demonstration of **SRP, MVC-style separation, and AI patterns**
- a portfolio piece showcasing **end-to-end feature delivery**

---

## 👨‍💻 Developer

**Oleksandr Tokarev**  
Unity & C# Game Developer based in Finland

---

## License

This project is licensed under the **MIT License** (source code only).

⚠️ **Important**
- The license applies to source code only.
- Game assets (art, audio, icons, branding) are not included.
- Republishing this project with the same branding or assets is not permitted.
