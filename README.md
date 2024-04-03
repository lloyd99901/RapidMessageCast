<p align="center">
    <img src="RapidMessageCast/RapidMessageCast Manager/Resources/imageres_20.png" alt="RMC Icon" />
</p>

# RapidMessageCast
RapidMessageCast is a messaging tool designed to help with rapid communication to computers across a network, particularly in emergency scenarios. Leveraging the native msg.exe utility found in the System32 directory of Windows operating systems, RapidMessageCast enables users to send messages to multiple computers simultaneously, ensuring swift dissemination of critical information.

> **DEVELOPMENT NOTE**
> 
> This is a work in progress. Expect many bugs or untidy code since this is in indev phase.

## Features:

### Speed and Reliability.
RapidMessageCast is great at sending messages to multiple computers. By leveraging the native msg.exe utility in Windows, it bypasses the need for complex setups or installations of remote access software, allowing for immediate message broadcasting. This also means that computers that have non-responding remote access software still get the message.

### Automation.
Automation tools or scheduling frameworks can leverage RapidMessageCast to schedule message broadcasts at predefined intervals or times. This capability is particularly useful for sending routine notifications, reminders, or updates to users or system administrators without manual intervention.
RapidMessageCast can be integrated with monitoring and alerting systems to automate the messaging of alerts and notifications. When monitoring systems detect anomalies, performance issues, or security breaches, they can automatically trigger RapidMessageCast to notify relevant personnel or teams.

### It's simple.
RapidMessageCast offers a straightforward interface. Users can quickly initiate message broadcasts with minimal effort, making it an ideal choice for urgent communication needs.
It also prioritizes functionality over frills, omitting flashy startup animations or unnecessary elements. When sending a message is your priority, it's primed for immediate action without delay

### Platform Compatibility.
RapidMessageCast is designed specifically for Windows operating systems, ensuring seamless integration and compatibility with existing infrastructure. It provides a reliable solution for organizations that predominantly use Windows environments.

### Two programs: Editor and Run
While it may seem confusing, these two programs allow both the user and automation programs to use RapidMessageCast without complex code or arugments.

With RapidMessageCast Editor you can:
- Edit the message
- Edit what computers are sent the message
- Import computer names via Active Directory (Intergration with Active Directory to get all computers on a network or OU group.)
- Save the message to a file that RapidMessageCast can use
- Run RapidMessageCast

With RapidMessageCast Run you can:
- Run it and it will immediately begin sending messages to all of the saved computer names. If no arguments are presented to point to a Message file, it will choose the file default.msg. This feature caters to automation programs that lack the functionality to add arguments, ensuring effortless integration of RapidMessageCast.
- Once done it will close out.

## Requirements
Both the sender and the recipient computers must have a Windows operating system that supports the MSG program in order to utilize RapidMessageCastRun effectively.
