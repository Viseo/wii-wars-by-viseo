# wii-wars-by-viseo
Augmented reality game with wiimotes!

<pre>
db   d8b   db                        db   d8b   db  .d8b.  d8888b. .d8888. 
88   I8I   88                        88   I8I   88 d8' `8b 88  `8D 88'  YP 
88   I8I   88                        88   I8I   88 88ooo88 88oobY' `8bo.   
Y8   I8I   88    88       88         Y8   I8I   88 88~~~88 88`8b     `Y8b. 
`8b d8'8b d8'   .88.     .88.        `8b d8'8b d8' 88   88 88 `88. db   8D 
 `8b8' `8d8'    Y88P    Y8888P       `8b8' `8d8'  YP   YP 88   YD `8888Y' 
</pre>

It is an IoT project powered by Azure Event hubs that allows up to 4 warriors (at the moment) to fight with one saber each. The saber could be turned off and when "on" each has a different color. An IoT watch allow the changing of the colors, and it aims to display the scores.
The warriors and the wiimote positions are recorded respectively via a WebCam and the internal accelerometer of the wiimotes. The video is then dynamically changed (compositing) to add the saber's laser.

ARCHITECTURE
This project is based on a "Lambda Architecture" (http://lambda-architecture.net).
This allows a generic, scalable and fault-tolerant data processing architecture :
- "slow" data that are in "batch layer", leaveraging Azure Machine Learning to predict how should a player behave in order to have the best score
- "rapid" data that are in "speed layer", leaveraging Azure Stream Analytics to display "near-real time" information, say about the number of kilometer the hand travels during the day
It allows the connection of Nintendo wiimotes today and is designed to be open to other IoT devices,
such as (and not limited to) X-Box, Microsoft HoloLens, ...


Other technologies are:
* Augmented reality
* Android watch (not pushed yet into Github)
* WPF

Soon:
* Azure Machine Learning
* Azure Azure Stream Analytics
* Azure HDInsight
* Windows 10 Universal Apps

