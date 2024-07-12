# Introduction
This project is a Caro (also known as Gomoku or Five in a Row) game implemented in C# using .NET. The game allows two players to compete with each other on the same network, providing an engaging multiplayer experience.
# Features
**Multiplayer Support:** Play with another player on the same network.\
**User-Friendly Interface:** Easy-to-use graphical user interface (GUI) for an enjoyable gaming experience.\
**Real-Time Updates:** Synchronized game state between devices.
# Installation
Prerequisites\
.NET Framework Visual Studio or any C# IDE
# Project Structure
**chessRoom.cs:** The main form of the application, handling the game interface.\
**Server.cs:** Manages network connections and data transfer between devices.\
**Player.cs:** Represents player information and actions.
# Usage
### 1. Run the Game:

Start the application by running the compiled executable or through your IDE.
### 2. Enter Player Information:

Type your name in the provided field.
If you want to connect and join another player, enter the IP address of the opponent's device.
If you want to create a new room, leave the IP address field empty.
### 3. Network Configuration:

Ensure both devices are connected to the same network.
### 4. Play the Game:

Take turns placing your pieces on the board.
The first player to align five pieces in a row (horizontally, vertically, or diagonally) wins the game.

### Example:
![image](https://github.com/user-attachments/assets/67e87c73-595b-44c6-9329-9ff94df4e798)

Enter the name and IP of the person you want to play with (leave blank if none) and then select a room.
Afterward, the room interface will display the IP address bar below for other players who want to connect to the room.

![image](https://github.com/user-attachments/assets/d6c50e8d-c388-42f8-ac32-34b555d9a268)

Other player wants to connect:

![image](https://github.com/user-attachments/assets/60bde3fc-98ee-4b95-8d8e-0a48a4d0d2c0)

![image](https://github.com/user-attachments/assets/beb4b8e8-cf12-4840-903c-a888bf97faa6)

**Note:** The progress bar is used to indicate the time remaining for each player. When the current player's turn runs out, it will end and Each room can only have 2 players








