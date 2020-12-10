This file details all the possible config options for MeteoricExpansion.


{
	//-- Destructive determines whether meteor strikes damage the world. If set to true, meteorites will cause craters, destroy structures, toss inventory contents across the ground, etc --// 
	"Destructive": [true][false],	Default: false

	//-- Determines the MINIMUM amount of time, in minutes, between meteor spawns. --//
	"MinimumMinutesBetweenMeteorSpawns": [ANY integer],		Default: 10

	//-- Determines the MAXIMUM amount of time, in minutes, between meteor spawns. MUST be a larger number than the minimum, otherwise it will be forced to 1 minute longer than minimum spawn --//
	"MaximumMinutesBetweenMeteorSpawns": [ANY integer],		Default: 30

	//-- Determines the MINIMUM range that a meteor will spawn from the player. Y distance is always worldheight - 10.
	"MinimumSpawnDistanceInChunks": [ANY integer],		Default: 1

	//-- Determines the MAXIMUM range that a meteor will spawn from the player. Will be forced to equal the minimum range if set below the minimum. Y distance is always worldheight - 10 --//
	"MaximumSpawnDistanceInChunks": [ANY integer],		Default: 6

	//-- Determines the MINIMUM amount of time, in seconds, that a meteor can live before exploding. --//
	"MinimumMeteorLifespanInSeconds": [ANY integer],	Default: 2

	//-- Determines the MAXIMUM amount of time, in seconds, that a meteor can live before exploding. --//
	"MaximumMeteorLifespanInSeconds": [ANY interger],	Default: 10
}



If you have made a mistake with your config file or something seems incorrect, please copy 'MeteoricExpansionConfig.json' 
from this ZIP archive and paste it into your ModConfig folder for Vintage Story. 

The Vintage Story mod config folder can be found here:
C:\Users\{Your Username}\AppData\Roaming\VintagestoryData\ModConfig