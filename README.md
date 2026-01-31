# Rumblebash
A phone game created with the Xamarin interface and the C# language
The game is about a collection of cards. A player is given a set of cards depicting monsters, placing them on a 2X3 board (Represented in code by an array of "SlotViews" and have them kill each other. Collecting the highest amount of kills and pass stages of pre-determined decks.

##Stats
Each card has stats and an ability to fuel it's power for violence
*AT - Attack: The attack power of the card, a card deals damage based on it's AT when striking
*HP - Hitpoints: How much damage a card can take before it dies
*SP - Speed: How fast a card attacks. The turn order in which cards on the board attack is determined by their speed stats

##Abilities
Abilities are unique to each card, triggering stat changes or extra AOE, or anything unique a card is set out to do, you can create abilities in the AbilityManager.cs folder, and implement a new class inheriting from the Ability class with various activation conditions
*OnStrike - When the ability bearer attacks. Variables -> bearer (The slot the ability bearer is placed on), target (The slot the target of the attack is placed on), dmg (The amount of damage dealt by the attack)
*OnHit - When the ability bearer gets hit. Variables -> bearer (The slot the ability bearer is placed on), attacker (The slot the card that attacked the bearer is placed on), dmg (The amount of damage dealt by the attack)
*OnDie - When the ability bearer dies. Variables -> bearer (The slot the ability bearer is placed on), attacker (The slot the card that attacked the bearer is placed on), dmg (The amount of damage dealt by the attack)
*OnStartOfTurn - When the turn starts. I.E - After every card attacks and the player gets to place a new card. Variables -> bearer (The slot the ability bearer is placed on)
*OnPlace - When the card gets placed on the board. Variables -> bearer (The slot the ability bearer is placed on)
*OnDrawnFromDeck - When the cards shows up in the player's deck. Variables -> bearer (The card that has the ability)
*OnUpdateAT - When the bearer's AT stat gets updated. Variables -> bearer (The slot the ability bearer is placed on), amount (The amount in which the stat changed), changer (The slot on which the card who changed the bearer's stat is placed)
*OnUpdateHP - When the bearer's HP stat gets updated. Variables -> bearer (The slot the ability bearer is placed on), amount (The amount in which the stat changed), changer (The slot on which the card who changed the bearer's stat is placed)
*OnUpdateSP - When the bearer's SP stat gets updated. Variables -> bearer (The slot the ability bearer is placed on), amount (The amount in which the stat changed), changer (The slot on which the card who changed the bearer's stat is placed)

Keep in mind that when a card is referenced it's referenced by the Slot on which it is placed. The "SlotView". You can use the GetPositionInArray method to get it's position in the board array for stronget manipulation

##Interacting with slots
The SlotView class which represents the slots cards can be placed on has multiple methods for messing with it and the monsers placed on it
*PlaceCard - Pick a card to place on the slot.
*CardStrike - If the slot has a monster placed and a target, said monster attacks it's target
*CardHit - The monster on the slot gets hit for a given amount of damage which gets subtracted from their health, you can also set what the attacking slot is.
*CardDead - The monster on the slot dies, damage and attacker are given as fields for the OnDie abilities

There are also methods for changing a card's stats directly: Set(Stat) and Change(Stat). Set(Stat) gets an int and sets the stat to exactly that number while change adds the number to the stat

##Adding new cards
To add a card, add an image for the card portrait of the card and a design for it as a monster when summoned. Add the card to the CSV of cards alongside it's name, stats, ability, and an idle animation of choice along with the speed of said animation
You can find the csv in the Assets folder



The game is split across 4 Worlds each with 20 levels of puzzles. For each: 3 stars can be awarded. 1 for getting a certain amount of kills, and 2 for unique missions set out in each level, such as doing a cetrain amount of damage
