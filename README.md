# Core-Match-3-Mechanics

I focused more on the code and architecture (extensibility, testability, performance) rather than the game itself. As a result, visually, it appears as simple as possible with basic Match-3 mechanics.

**Demonstration of the results:**

[![Result](https://github.com/PapkovNikita/Core-Match-3-Mechanics/assets/3509865/8ae4af45-62ec-4e35-b6bd-5acdf1bffc23)](https://youtu.be/7gAi5-uHfJ8)

**Demonstration of tests running in `PlayMode`:**

[![PlayModeVideo](https://github.com/PapkovNikita/Core-Match-3-Mechanics/assets/3509865/e1e93024-31ca-4199-8e30-6d5d05e5b675)](https://youtu.be/ROUerlTH1hg)

![Screenshot_5](https://github.com/PapkovNikita/Core-Match-3-Mechanics/assets/3509865/256ff44a-cac0-4282-82ea-30c233d50042)

**Demonstration of tests running in `EditMode`:**

<img src="https://github.com/PapkovNikita/Core-Match-3-Mechanics/assets/3509865/f5a929cc-35c8-41a3-b7a5-9b4364035ac1" width="300">


## Architecture

For the architecture, I chose a 2-Tier Architecture, which includes a model layer (Game logic, Service Layer, Game settings, etc.) and UI. 

The model layer knows absolutely nothing about the game's interface layer. All interaction between the model and UI occurs through an event bus in a unidirectional manner, and if the game lacks an interface, nothing breaks. This makes testing convenient and quick.

For game state management, I went with a state machine as a flexible tool, which clearly outlines responsibilities.

<img src="https://github.com/PapkovNikita/Core-Match-3-Mechanics/assets/3509865/acb35573-2c3a-4567-b7e0-4cbd35d6ae0b" width="250"> 
<img src="https://github.com/PapkovNikita/Core-Match-3-Mechanics/assets/3509865/15c3140f-bd59-4bf0-90c0-d40a0e317529" width="550">

If needed, you can quickly and easily add a new state, for instance, for handling and activating special tiles (like bombs and so on).

You might also find the following classes interesting:

- **GameController** - The **entry-point** in this project
- **BoardService** - Manages everything that happens with the board (deletion, addition, moving of tiles)
- **BoardGenerator** - Responsible for generating tiles without initial matches
- **MatchDetectionService** - Handles match detection on the board
- **SwipeHandler** - Manages game swipes

## Drawbacks

Drawbacks of the current implementation:

- It's clear that a tile pool should be used.
- It's beneficial to create a separate model for tiles (with their position and state) and update the UI accordingly. This will greatly simplify the BoardView and essentially remove all its logic.
- The feature to check available moves for the user hasn't been implemented yet.

All these drawbacks can be fixed within 1-2 hours. I will try to address them as soon as I have some free time.
