<p align="center">
    <img src="static/images/RMCBanner.png" alt="RMC Banner" />
</p>

# RapidMessageCast
 > PC Messager, Email, Remote Program Execution (PSExec), and Wake-On-LAN Support

[![GitHub issues](https://img.shields.io/github/issues/lloyd99901/RapidMessageCast)](https://github.com/lloyd99901/RapidMessageCast/issues)
[![GitHub stars](https://img.shields.io/github/stars/lloyd99901/RapidMessageCast)](https://github.com/lloyd99901/RapidMessageCast/stargazers)
[![GitHub license](https://img.shields.io/github/license/lloyd99901/RapidMessageCast)](https://github.com/lloyd99901/RapidMessageCast/blob/master/LICENSE)

RapidMessageCast is a messaging tool designed to help with rapid communication to computers across a network. This program enables users to send messages to multiple computers simultaneously, ensuring swift dissemination of critical information, like in emergency scenarios.

> **IN DEVELOPMENT NOTICE**
> 
> This is a work in progress. Expect bugs, untidy code, and missing features since this is in indev phase.


![MainWindow](https://raw.githubusercontent.com/lloyd99901/RapidMessageCast/master/static/images/RMCManager.png)

## Features:
- Message multiple PC's fast (msg.exe)
- Send automatic emails or prewritten emails. (runs Outlook/email program of choice or uses SMTP)
- PSExec Support (Remote Program Execution).
- Quick save and loads of RMSG files that allows the user to quickly load and broadcast saved messages. (e.g. reminders, emergency messages, notices, etc).
- Quick load message and PC list from a txt file
- Load PC list from Active Directory
- View broadcast history with a comprehensive broadcast log.
- Schedule messages to be broadcasted.
- Tailored for automation programs.

## Why use RMC?

### Speed.
RapidMessageCast is great at sending messages to multiple computers. By leveraging the native msg.exe utility in Windows, it bypasses the need for complex setups or installations of remote access software, allowing for immediate message broadcasting. This also means that computers that have non-responding remote access software still get the message.

### Automation.
Automation tools or scheduling frameworks can leverage RapidMessageCast to schedule message broadcasts at predefined intervals or times. This capability is particularly useful for sending routine notifications, reminders, or updates to users or system administrators without manual intervention.
RapidMessageCast can be integrated with monitoring and alerting systems to automate the messaging of alerts and notifications. When monitoring systems detect anomalies, performance issues, or security breaches, they can automatically trigger RapidMessageCast to notify relevant personnel or teams.

### Broadcast fast.
RapidMessageCast offers an interface that allows the user to be quick with their broadcast. Users can quickly initiate message broadcasts with minimal effort, making it an ideal choice for urgent communication needs.
It also prioritizes functionality over frills, omitting flashy startup animations. When sending a message is your priority, it's primed for immediate action without delay.

### Free forever and open-source
This program is free and open source; that will never change. If you have an issue with the program and you know how to fix it, this code is ready to be modified.

### GUI and CLI
RapidMessageCast GUI and CLI are two ways of broadcasting your message.

The GUI gives them the full control over the features of RMC, the user can save their RMSG file and then can transfer it to the CLI.

![MainWindow](https://raw.githubusercontent.com/lloyd99901/RapidMessageCast/master/static/images/RMCManager.png)

Once RapidMessageCast CLI is run, it will then read the RMSG and:
- It will immediately begin sending messages to all of the saved computer names. 
- If no arguments are presented to point to a Message file, it will choose the file default.msg. This feature caters to automation programs that lack the functionality to add arguments, ensuring effortless integration of RapidMessageCast.
- Once done it will close out automatically.
  
![Dispatcher](https://raw.githubusercontent.com/lloyd99901/RapidMessageCast/master/static/images/ExampleRMCDispatcher.png)

### Platform Compatibility.
RapidMessageCast is designed specifically for Windows operating systems, ensuring seamless integration and compatibility with existing infrastructure. It provides a reliable solution for organizations that predominantly use Windows environments.

## Requirements
- MSG.exe is not included in Windows Home editions. Professional/enterprise editions are required.
Windows 7 and above

- PSExec has to be downloaded by the user manually and placed in the same folder where RMC GUI is.

- Both the sender and the recipient computers must have a Windows operating system that supports the MSG program in order to utilize RapidMessageCastRun effectively.

## Developer Notes

- Source code is edited on Visual Studio 2022
- If you have any improvements, suggestions, or bug reports, please do report it.
