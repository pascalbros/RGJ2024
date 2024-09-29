# How to build a level

Every non-numeric character in the file is an element in the level (spaces are ignored). 
A number after an object indicates how many times that object can be used, otherwise it can be used forever.

|Char|Meaning|
|--|--|
|`.`|Empty|
|`P`|Player starting point|
|`E`|Exit|
|`#`|Wall|
|`O`|Mirror|
|`*`|Warp|
|`>`|Right|
|`<`|Left|
|`-`|Right & left|
|`^`|Up|
|`V`|Down|
|`\|`|Up & down|
|`J`|Up & left|
|`L`|Up & right|
|`T`|Down & left|
|`F`|Down & right|
|`@`|Rotate clockwise|
|`G`|Rotate counterclockwise|

*Note: a level can contain only 0 or 2 warps.*

A line with `!` means that the level is complete. 
The next two rows represents the starting objects (use `.` to represent an empty slot).

## Example
```
O < . . . 
. . # # . 
. . # # . 
. . . @1. 
P . # # E 
!
>
G2
```
In this level the player start at the bottom left angle of the level, starting with the right arrow and conterclockwise rotation with 2 uses.
The end is in the bottom right angle. In the to left angle there is a mirror and next to it there is a left arrow.
Near the end there is a clockwise rotation with a single use.
