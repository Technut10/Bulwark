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

TODO 3: moving the wrong thing

Your math for the step (direction * speed * Time.deltaTime) is right, but two things are off:

It’s being added to transform.forward, which is the rotation. That spins the projectile instead of moving it. Moving means changing transform.position.
You’re adding the position twice. movement already includes + transform.position, and then it gets added to something again. Just add the step to the position directly:
csharp
   transform.position += direction * speed * Time.deltaTime;

Also remove the owner.IsDestroyed() check. A projectile should keep flying even if the bot that fired it dies mid-flight. And if the owner is gone, owner becomes null, and that line would crash.

Small cleanup
Delete the empty Start() method. Unity still calls empty methods, so leaving them in adds a tiny cost for nothing.
/// <summary> comments go above methods and fields, as documentation. Inside a method, use regular // comments.
The big lesson

Where code runs matters as much as what it does. Launch runs once, Update runs every frame, and OnTriggerEnter runs on contact. Before writing a line, ask: “how often does this method run?”

And gameObject versus owner: gameObject is this object, and owner is whoever fired it. Mixing those up here would have deleted your bots!

Use gameObject, which always means “the object this script is attached to,” and here that’s the projectile:

csharp
Destroy(gameObject, lifetime);

A quick way to remember it: a projectile script should almost never destroy anything except itself.

Small comment cleanup: the line // Also make the projectile face its direction. ended up below the code it describes. Move it up with the TODO 1 comments so it still makes sense when someone reads it on GitHub.

The question is: “Did I just touch the bot that fired me?” Two things to check:

TODO 4: Ignore the owner
Do we even have an owner? It could be null.
Is the thing we touched part of the owner? IsChildOf answers that, and it also returns true if it’s the owner itself.

If both are true, leave the method so nothing else happens. You’ve used this before: it’s a guard clause, just like if (target == null) return;.