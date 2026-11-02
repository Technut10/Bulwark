# Notes for AI code.

---
Quaterinon.LookRotation(target.position): a position is a point measured from the center of the world, (0,0,0). So this pointed the bot way to the **World Center* would look to see the player, not the way the bot needs to look. Subtracting target.position - transform.position gives you the arrow from the bot to the player., which is what we want. And the result has to be assigned to transform.rotation, or nothing happens.

___

## Firing

--- 
	1. fireTimer -= Time.deltaTime 0 fireInterval: 
		a. This is a sneaky math bug. Time.deltaTime - fireInterval is about 0.016 - 1 = -0.984, and subtracting a negative adds. So the timer went up every frame and never fired. It should only subtract Time.deltaTime.

## The Reset

Without fireTimer = fireInterval, the bot would fire every frame once the time hit zero.

## Retreat

What changed:

.normalized makes the arrow exactly 1 meter long, so * retreatDistance means “4 meters in that direction.” Without it, the arrow’s length is the full distance to the player, and the math gets unpredictable.
transform.position + means “start from where I am.” Without it, the destination is measured from the center of the world.
retreatDistance, not retreatRange.

The big takeaway: a position is a point, and a direction is an arrow between two points. Mixing them up is the most common bug in game math, and you’ll use this idea constantly. Interviewers love vector questions, so it’s worth getting comfortable with.