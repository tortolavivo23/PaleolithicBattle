# Paleolithic Battle

## Links
- [Download](https://github.com/tortolavivo23/PaleolithicBattle/releases/tag/v1.0.0)
- [Trailer](https://www.youtube.com/watch?v=hYyVZEMlQNo)

## Game Background
Paleolithic Battle is a turn-based strategy video game inspired by titles in the genre such as Advance
Wars and Fire Emblem. Designed for players with little experience in this type of game, it incorporates
mini-games that facilitate strategic decision-making and make the gameplay more accessible. Set in a
Paleolithic world, it adopts an aesthetic that fits the era.

These mini-games are activated when engaging in combat and are simple, fast, and easy to
understand, with a duration of approximately five seconds. Their purpose is to test the player's speed
and mechanical skill, adding an extra layer of depth to the game's strategy.
The development of the game follows the agile Scrum methodology, organized into sprints ranging from
one to four weeks, where specific objectives are set to be achieved by the end of each cycle. The
implementation is carried out using the Unity game engine.

The result is a game with three clearly differentiated levels in terms of design, three distinct types of
troops, and five minigames. The overall experience is entertaining and represents a refreshing take on
the turn-based strategy genre.
In conclusion, a fun and functional game has been successfully created, tackling a large-scale project
that posed a real challenge. Despite the difficulties, a satisfactory final product has been delivered.

## Gameplay Instructions

Navigating through the various game menus is intuitive and does not require detailed instructions.  
However, during dialogue sequences, you must press the spacebar or click on the arrow located in the bottom-right corner of the screen to proceed.

The core gameplay is more complex, so the essential steps to interact with your troops during your turn are described below:

### During Your Turn, You Can Interact With Your Troops as Follows:

1. **Select a Troop**:  
   Click on one of your units to select it.  
   When selected, a contextual menu will appear on the right side of the screen showing the available actions for that unit.

2. **Move**:  
   If the unit has movement points available, you can move it to a valid tile on the map.

3. **Capture**:  
   If the unit is a melee type and is standing on a capturable tile (e.g., base, cave, or camp) that you do not control, the capture option will appear.  
   Once selected, the unit will perform the capture.

4. **Attack**:  
   If there is an enemy unit within attack range, the attack option will become available.  
   You can either perform a standard attack or, if available, choose to play a minigame that may affect the attack outcome.

### Action Restrictions

- Each unit can **attack or capture only once per turn**.
- These actions **must be performed after moving**, if you intend to move the unit first.

### Win and Lose Conditions

- **Victory**: Achieved by capturing the enemy base or eliminating all enemy units.
- **Defeat**: Occurs if the enemy captures your base or eliminates all your units.

## Development Details

The game was developed using **Unity**.

### Gameplay Architecture

The core gameplay is implemented using the **state machine pattern**, clearly separating the different phases of the player's turn and the enemy's turn.

### Enemy AI

The enemy AI is implemented using **behavior trees**, with different **heuristics** applied to each of the enemy's possible actions.

### Minigames

To avoid loading times, minigames are loaded using **additive scene loading**.  
Each minigame is designed to reflect **different video game genres**, providing variety and enhancing the overall gameplay experience.



