# Emoji Battle

A mobile puzzle-battle game built in **Unity 6**.  
Inspired by classic tic-tac-toe, expanded with emoji customization, AI opponents, basic progression and a mobile-friendly UI.

✅ **Refactor note:** the project was **fully refactored** with a focus on **MVC-style structure**, **SRP (Single Responsibility Principle)**, and clear separation between **game logic**, **UI**, and **infrastructure**.

---

## 🎮 Features

### ✅ Implemented

- **Custom Emoji System**  
  9 colours × 87 emojis, rarity levels, basic unlock rules (ScriptableObjects).
- **AI Opponents (Strategy + Factory)**  
  Easy / Normal / Hard via `IAIStrategy` + factory selection.  
  Easy to extend without changing existing logic.
- **Core Game Logic (Domain separated from UI)**  
  Turn handling, win/draw detection, board state logic separated from UI/presentation.
- **Player Progression & Saving**  
  Simple unlock/progression system saved via **JSON / PlayerPrefs**.
- **Mobile-Friendly UI**  
  Lobby, emoji selection, popups, basic animations without gameplay rules inside UI.
- **Clean Architecture Refactor**  
  MVC-style separation, SRP across systems, reduced coupling, avoided god-objects.
- **Initial Mobile Optimization**  
  Lightweight assets, controlled allocations, no unnecessary runtime object creation.

---

### 🔧 In Progress

- Extended progression system (additional unlock conditions, streak-based rewards)
- Centralized popup system (settings, victory/defeat, progression updates)
- Rewarded + interstitial ads integration
- Loot-box reward popup
- SFX + full Audio Mixer setup

---

### 🧭 Planned

- Google Play release + basic analytics
- Optional WebGL build
- Multiplayer mode (concept)
- Leaderboards & simple social sharing
- iOS build support

---

## 🧠 Tech & Architecture

- **Unity 6**, **C#**
- **MVC-style architecture** (Model / View / Controller separation)
- **SRP** across gameplay, AI and UI
- **Strategy + Factory patterns** for AI behaviour
- **ScriptableObjects** for emoji data/configuration
- **Event-based communication** between systems
- **Domain logic independent from UI**
- Coroutines for timing/animations
- Player progress stored in JSON / PlayerPrefs
- Shader Graph (basic UI / VFX effects)

---

## 🧩 Architecture Overview

### Model (Domain / Data)
- Board state + win/draw checking  
- AI decision logic (strategies)  
- Progression + saving models  

### View (Presentation)
- UI screens, widgets, visual state, animations  
- No game rules inside UI  

### Controller (App / Flow)
- Scene bootstrap  
- Game flow orchestration  
- Connects Domain ↔ Presentation  

> **Key idea:** gameplay logic does not depend on UI, and UI components do not contain game rules.

---

## 📂 Project Structure

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

## 🚀 How to Build / Run  
1. Install Unity 6 (latest available version).  
2. Clone the repository:  
   `git clone https://github.com/SD7games/Emoji_Battle.git`  
3. Open the project via Unity Hub → select Unity 6.  
4. Main scenes:  
   - `Lobby`  
   - `Game`  
5. Platforms:  
   - Android (primary)  
   - WebGL (experimental, planned)  
   - iOS (planned)

---

## 📸 Screenshots

<p align="center">
  <img src="SpleshScene.png" width="260" />
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

## 🛠 Roadmap  
- Improve UI feedback and animations  
- Implement ads (rewarded / interstitial)  
- Add loot-box reward popup  
- Add sound effects and polish audio  
- Final optimization  
- Prepare for Google Play release  

---

## 👨‍💻 Developer  
**Oleksandr Tokarev** — Unity & C# Game Developer based in Finland.  
Open to work and collaboration.  
