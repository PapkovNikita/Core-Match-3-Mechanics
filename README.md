# Core-Match-3-Mechanics

I focused more on the code and architecture (extensibility, testability, performance) rather than the game itself. As a result, visually, it appears as simple as possible with basic Match-3 mechanics.

<table>
    <tr>
        <td>
          Demonstration of the results:
        </td>
        <td>
          Integrational test example: <br> <i>(Performs 1000000 random swaps)</i>
        </td>
        <td>
          Unit tests:
        </td>
    </tr>
    <tr>
        <td>
          <img src="https://github.com/PapkovNikita/Core-Match-3-Mechanics/assets/3509865/8ae4af45-62ec-4e35-b6bd-5acdf1bffc23">
        </td>
        <td>
          <img src="https://github.com/PapkovNikita/Core-Match-3-Mechanics/assets/3509865/e1e93024-31ca-4199-8e30-6d5d05e5b675">
        </td>
        <td>
          <img src="https://github.com/PapkovNikita/Core-Match-3-Mechanics/assets/3509865/f5a929cc-35c8-41a3-b7a5-9b4364035ac1">
        </td>
    </tr>
</table>

## Stack

- **Unity 2021.3.16f1**
- **VContainer** (a lightweight, fast DIContainer with code generation capability)
- **UniTask** (a more convenient and modern alternative to coroutines and callbacks)
- **UniTaskPubSub** (an asynchronous MessageBroker to fully separate the UI and Model layers)
- **DoTween** (for various animations)

For testing:
- **Unity Test Framework**
- **Moq**

## Architecture

For the architecture, I chose a 2-Tier Architecture, which includes a **model layer** (Game logic, Service Layer, Game settings, etc.) and **UI**. 

The model layer knows absolutely nothing about the game's interface layer. All interaction between the model and UI occurs through an event bus in a unidirectional manner, and if the game lacks an interface, nothing breaks. This makes testing convenient and quick.

For game state management, I went with a state machine as a flexible tool, which clearly outlines responsibilities.

<img src="https://github.com/PapkovNikita/Core-Match-3-Mechanics/assets/3509865/dac786f9-dc3b-4a7b-aaa5-a96057cc48ff" width="250"> 
<img src="https://github.com/PapkovNikita/Core-Match-3-Mechanics/assets/3509865/15c3140f-bd59-4bf0-90c0-d40a0e317529" width="550">

If needed, you can quickly and easily add a new state, for instance, for handling and activating special tiles (like bombs and so on).

You might also find the following classes interesting:

- **GameController** - The **entry-point** in this project
- **BoardService** - Manages everything that happens with the board (deletion, addition, moving of tiles)
- **BoardGenerator** - Responsible for generating tiles without initial matches
- **MatchDetectionService** - Handles match detection on the board
- **SwipeHandler** - Manages game swipes
