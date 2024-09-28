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
|`&#124;`|Up & down|
|`J`|Up & left|
|`L`|Up & right|
|`T`|Down & left|
|`F`|Down & right|
|`@`|Rotate clockwise|
|`G`|Rotate counterclockwise|

A line with `!` means that the level is complete. 
The next two rows represents the starting objects (use `.` to represent an empty slot).