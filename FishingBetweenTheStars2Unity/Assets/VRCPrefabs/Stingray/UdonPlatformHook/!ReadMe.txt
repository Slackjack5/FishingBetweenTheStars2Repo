Created by Superbstingray

Udon Player Platform Hook

V0.53 -  04/08/21  -  Made in UdonGraph SDK3-2021.08.04.15.07

Unity 2019 and SDK version 2021.08.04.15.07 or above is required.

Prefab that functions as a drag and drop solution for making players correctly follow any moving Platforms / Vehicles / GameObjects in your scene when standing on them. Makes players "Functionally" behave as if they were parented to the game object they are standing on. Behavior can be enabled or disabled based on layers.

(Prefab Functionality)
Moves the player by teleporting them with an offset from the GameObject they are standing on creating a parenting effect. Players will move and seamlessly teleport with colliders they stand on.

(Usage)
Drag the prefab into the root of your scene to use. Ideally you should set the affected layers by changing the collidable layers in the particle collision settings, though this isn't required.
Set your moving objects/platforms to layer 11(Environment) or a custom layer and customize the collidable layers in the Particle System. You should keep the prefab on layer 5(UI) and the particle collision collidable with layer 5(UI) as this layer is used to disable it’s behavior when not in use.

You only need one of these prefabs in your scene. Do not add more than one.

(Additional)

This prefab will teleport players every frame whilst they are standing on a valid platform object if you have other functions in your world that move or teleport the player around they may not function properly while the player is hooked to an object. Due to this you should manually assign the layers that this prefab uses and avoid using the default layer.

Velocity when walking off moving objects can be wrong due to how the VRChat player controller handles its own velocity.

Won't work with CyanEmu versions v0.3.10 or below as it wont support player origin tracking so you will need to verify behavior in game.
