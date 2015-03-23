Feature: CheckSaberStatus
	In order to ensure I have the best ever laser Saber
	As a great fighter
	I want to chech automatically all available features of my Laser Saber

Scenario:  Check Colors 
	Given I have Saber
	When I choose a given color while the laser is ON
		| Is_Saber_On | Given_Color |
		| 1           | Red         |
		| 1           | Blue        |
		| 1           | Green       |
		| 1           | Violet      |
		| 0           | N/A         |
	Then the laser should be colored accordingly

Scenario:  Get a new Saber 
	Given I have Saber
	When first have it
	Then the Saber should Off

Scenario:  Check to turn on and off 
	Given I have Saber 
	When I play around with its switch to turn it ON and OFF alternatively
	Then its final state should be ON or OFF occording to its orginial state and the number of time the switch is pressed
	| Original_State | Number_of_press | Final State |
	| 0              | 1               | 1           |
	| 1              | 1               | 0           |
	| 0              | 2               | 0           |
	| 1              | 2               | 1           |
	| 0              | 3               | 1           |
	| 1              | 3               | 0           |
