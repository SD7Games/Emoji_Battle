Emoji Battle

A mobile puzzle-battle game built in Unity 6.
Inspired by classic tic-tac-toe, expanded with emoji customization, AI opponents and a clean, maintainable architecture.

The project was fully refactored with a focus on MVC-style structure, Single Responsibility Principle, and clear separation between game logic, UI and infrastructure.

🎮 Features
✅ Implemented

Custom Emoji System
9 colours × 87 emojis, rarity levels and basic unlock rules, driven by ScriptableObjects.

AI Opponents (Strategy Pattern)
Easy / Normal / Hard difficulties implemented via IAIStrategy interface and selected through a factory, allowing easy extension without modifying existing logic.

Core Game Logic (Domain-Driven)
Turn handling, win/draw detection and board state logic fully separated from UI and presentation code.

Player Progression & Saving
Simple progression and unlock system, persisted via JSON and PlayerPrefs.

Mobile-Friendly UI Layer
Lobby, emoji selection, popups and animations implemented as presentation components without gameplay logic.

Clean Architecture Refactor
MVC-style separation, SRP across systems, no god-objects, minimal coupling between modules.

Initial Mobile Optimization
Lightweight assets, controlled allocations, no unnecessary runtime object creation.

🔧 In Progress

Extended progression system (additional unlock conditions, streak-based rewards).

Centralized popup system (settings, victory/defeat, progression updates).

Rewarded and interstitial ads integration.

Loot-box reward popup.

Sound effects and full Audio Mixer setup.

🧭 Planned

Google Play release with basic analytics.

Optional WebGL build.

Multiplayer mode (concept).

Leaderboards & simple social sharing.

iOS build support.

🧠 Tech & Architecture

Unity 6 / C#

MVC-style architecture (Model / View / Controller separation)

Single Responsibility Principle (SRP) across gameplay, AI and UI

Strategy + Factory patterns for AI behaviour

ScriptableObjects for emoji data and configuration

Event-based communication between systems

Domain logic independent from UI

Coroutines for timing and animations

Player progress stored in JSON / PlayerPrefs

Shader Graph (basic UI / VFX effects)

📂 Project Structure
/Assets
    /Scripts
        /App            # Entry points, bootstrap, scene flow
        /Domain         # Pure game logic (board, AI, progression)
        /Infrastructure # Data models, saving, profiles
        /Presentation   # UI views and visual logic
    /SO                 # ScriptableObjects (emoji data, configs)
    /Sprites
    /Prefabs


Key idea:
Gameplay logic does not depend on UI.
UI components do not contain game rules.

🚀 How to Build / Run

Install Unity 6 (latest available version).

Clone the repository:

git clone https://github.com/SD7games/Emoji_Battle.git


Open the project via Unity Hub → select Unity 6.

Main scenes:

Lobby

Game

Platforms:

Android (primary)

WebGL (experimental, planned)

iOS (planned)

📸 Screenshots
<p align="center"> <img src="SpleshScene.png" width="260" /> <img src="BootstrapScene.png" width="260" /> </p> <p align="center"> <img src="LobbyScene.png" width="260" /> <img src="MainScene.png" width="260" /> </p> <p align="center"> <b>Splash → Loading → Lobby → Gameplay</b> </p>
🛠 Roadmap

Improve UI feedback and animations

Implement ads (rewarded / interstitial)

Add loot-box reward popup

Add sound effects and polish audio

Final optimization

Prepare for Google Play release

👨‍💻 Developer

Oleksandr Tokarev
Unity & C# Game Developer based in Finland

Focused on clean architecture, maintainable gameplay systems and finished, playable projects.
Open to work and collaboration.
