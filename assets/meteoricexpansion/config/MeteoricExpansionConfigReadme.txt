This file details all the possible config options for MeteoricExpansion.


{
	//-- Destructive determines whether meteor strikes damage the world. If set to true, meteorites will cause craters, destroy structures, toss inventory contents across the ground, etc --// 
	"Destructive": [true][false],	Default: false

	//-- If claims have been set for your base, then setting this to true will protect any blocks within the claim from being destroyed. This works for traders, too --//
	"ClaimsProtected": [true][false],	Default: true

	//-- Determines the MINIMUM amount of time, in minutes, between meteor spawns. Timer restarts at the start of each game session or when the server has been restarted. --//
	"MinimumMinutesBetweenMeteorSpawns": [ANY integer],		Default: 10

	//-- Determines the MAXIMUM amount of time, in minutes, between meteor spawns. MUST be a larger number than the minimum, otherwise it will be forced to 1 minute longer than minimum spawn. Timer restarts at the start of each game session or when the server has been restarted. --//
	"MaximumMinutesBetweenMeteorSpawns": [ANY integer],		Default: 30

	//-- Determines the MINIMUM range that a meteor will spawn from the player. Y distance is always worldheight - 10.
	"MinimumSpawnDistanceInChunks": [ANY integer],		Default: 1

	//-- Determines the MAXIMUM range that a meteor will spawn from the player. Will be forced to equal the minimum range if set below the minimum. Y distance is always worldheight - 10 --//
	"MaximumSpawnDistanceInChunks": [ANY integer],		Default: 6

	//-- Determines the MINIMUM amount of time, in seconds, that a meteor can live before exploding. --//
	"MinimumMeteorLifespanInSeconds": [ANY integer],	Default: 2

	//-- Determines the MAXIMUM amount of time, in seconds, that a meteor can live before exploding. --//
	"MaximumMeteorLifespanInSeconds": [ANY integer],	Default: 10

	//-- A smouldering rock will only cool if it has existed after this number of minutes --//
	"MinimumCraterSmoulderTimeInMinutes": [ANY integer],	Default: 2

	//-- A smouldering rock will cool before it has existed for this amount of minutes --//
	"MaximumCraterSmoulderTimeInMinutes": [ANY integer],	Default: 10
}

You can delete the config file and launch Vintage Story with the mod enabled to generate a new file if needed.

The Vintage Story mod config folder can typically be found here:
C:\Users\{Your Username}\AppData\Roaming\VintagestoryData\ModConfig