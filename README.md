# wii-wars-by-viseo
Augmented reality game with wiimotes!

         ,-.-.     .=-.-.    .=-.-.                 ,-.-.     ,---.                       ,-,--.                                                      ,-.-.    .=-.-.    ,-,--.        ,----.      _,.---._     
,-..-.-./  \==\   /==/_ /   /==/_ /        ,-..-.-./  \==\  .--.'  \       .-.,.---.    ,-.'-  _\            _..---.    ,--.-.  .-,--.         ,--.-./=/ ,/   /==/_ /  ,-.'-  _\    ,-.--` , \   ,-.' , -  `.   
|, \=/\=|- |==|  |==|, |   |==|, |         |, \=/\=|- |==|  \==\-/\ \     /==/  `   \  /==/_ ,_.'          .' .'.-. \  /==/- / /=/_ /         /==/, ||=| -|  |==|, |  /==/_ ,_.'   |==|-  _.-`  /==/_,  ,  - \  
|- |/ |/ , /==/  |==|  |   |==|  |         |- |/ |/ , /==/  /==/-|_\ |   |==|-, .=., | \==\  \            /==/- '=' /  \==\, \/=/. /          \==\,  \ / ,|  |==|  |  \==\  \      |==|   `.-. |==|   .=.     | 
 \, ,     _|==|  |==|- |   |==|- |          \, ,     _|==|  \==\,   - \  |==|   '='  /  \==\ -\           |==|-,   '    \==\  \/ -/            \==\ - ' - /  |==|- |   \==\ -\    /==/_ ,    / |==|_ : ;=:  - | 
 | -  -  , |==|  |==| ,|   |==| ,|          | -  -  , |==|  /==/ -   ,|  |==|- ,   .'   _\==\ ,\          |==|  .=. \    |==|  ,_/              \==\ ,   |   |==| ,|   _\==\ ,\   |==|    .-'  |==| , '='     | 
  \  ,  - /==/   |==|- |   |==|- |           \  ,  - /==/  /==/-  /\ - \ |==|_  . ,'.  /==/\/ _ |         /==/- '=' ,|   \==\-, /               |==| -  ,/   |==|- |  /==/\/ _ |  |==|_  ,`-._  \==\ -    ,_ /  
  |-  /\ /==/    /==/. /   /==/. /           |-  /\ /==/   \==\ _.\=\.-' /==/  /\ ,  ) \==\ - , /        |==|   -   /    /==/._/                \==\  _ /    /==/. /  \==\ - , /  /==/ ,     /   '.='. -   .'   
  `--`  `--`     `--`-`    `--`-`            `--`  `--`     `--`         `--`-`--`--'   `--`---'         `-._`.___,'     `--`-`                  `--`--'     `--`-`    `--`---'   `--`-----``      `--`--''     
  

It is an IoT project powered by Azure Event hubs that allows up to 4 warriors (at the moment) to fight with one saber each. The saber could be turned off and when "on" each has a different color. An IoT watch allow the changing of the colors, and it aims to display the scores.
The warriors and the wiimote positions are recorded respectively via a WebCam and the internal accelerometer of the wiimotes. The video is then dynamically changed (compositing) to add the saber's laser.

Other technologies are:
* Augmented reality
* Android watch (not pushed yet)
* WPF

Soon:
* Azure Machine Learning
* Azure HDInsight
* Windows 10 Universal Apps

